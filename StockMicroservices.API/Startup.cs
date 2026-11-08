using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using StockMicroservices.API.Authorization;
using StockMicroservices.API.Data;
using StockMicroservices.API.Errors;
using StockMicroservices.API.EventBusConsumer;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Services;
using StockMicroservices.EventBus.Common;

namespace StockMicroservices.API
{
    public class Startup
    {
        private const string ApiPolicy = "StockAPIPolicy";

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<DatabaseSetting>(_configuration.GetSection("Database"));
            var databaseSettings = _configuration.GetSection("Database").Get<DatabaseSetting>()
                ?? throw new InvalidOperationException("The 'Database' configuration section is missing.");

            if (string.IsNullOrWhiteSpace(databaseSettings.ConnectionString))
            {
                throw new InvalidOperationException("Database:ConnectionString must be configured.");
            }

            services.AddSingleton<IMongoClient>(_ => new MongoClient(databaseSettings.ConnectionString));
            services.AddSingleton<IStockDbContext, StockDbContext>();
            if (!_environment.IsEnvironment("test"))
            {
                services.AddHostedService<DatabaseSeeder>();
            }

            services.AddHealthChecks()
                .AddCheck<MongoHealthCheck>("mongodb", tags: new[] { "ready" });

            AddAuthentication(services);
            AddCors(services);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT issued by the Stock Identity Server.",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
            });

            AddEventBus(services);

            services.AddScoped<IStockMarketService, StockMarketService>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    var error = feature?.Error;

                    logger.LogError(error, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/problem+json";

                    var payload = new ErrorMessage
                    {
                        Message = env.IsDevelopment() ? error?.Message ?? "Internal Server Error (500)" : "An unexpected error occurred.",
                        StackTrace = env.IsDevelopment() ? error?.StackTrace : null,
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
                });
            });

            app.UseCors("Default");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = registration => registration.Tags.Contains("ready"),
                });
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var identityServerUrl = RequireSetting("IdentityServerUrl");
            var apiScope = RequireSetting("APIScope");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerUrl;
                    options.Audience = apiScope;
                    options.RequireHttpsMetadata = _configuration.GetValue("IdentityServer:RequireHttpsMetadata", false);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiPolicy, policy =>
                {
                    if (!_environment.IsEnvironment("test"))
                    {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    }

                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new StockAPIRequirement(apiScope));
                });
            });

            services.AddScoped<IAuthorizationHandler, StockAPIRequirementHandler>();
        }

        private void AddCors(IServiceCollection services)
        {
            var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? Array.Empty<string>();

            services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    if (allowedOrigins.Length == 0)
                    {
                        policy.AllowAnyOrigin();
                    }
                    else
                    {
                        policy.WithOrigins(allowedOrigins);
                    }

                    policy.AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        private void AddEventBus(IServiceCollection services)
        {
            if (_environment.IsEnvironment("test"))
            {
                return;
            }

            var host = RequireSetting("RabbitMq:Hostname");
            var port = _configuration.GetValue("RabbitMq:Port", 5672);
            var username = RequireSetting("RabbitMq:Username");
            var password = RequireSetting("RabbitMq:Password");

            services.AddMassTransit(bus =>
            {
                bus.AddConsumer<StockUpdateConsumer>();

                bus.UsingRabbitMq((context, config) =>
                {
                    config.Host(host, (ushort)port, "/", hostConfig =>
                    {
                        hostConfig.Username(username);
                        hostConfig.Password(password);
                    });

                    config.ReceiveEndpoint(EventBusConstants.STOCK_UPDATE_QUEUE, endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(2)));
                        endpoint.ConfigureConsumer<StockUpdateConsumer>(context);
                    });
                });
            });
        }

        private string RequireSetting(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Configuration value '{key}' is required.");
            }

            return value;
        }
    }
}
