using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StockMicroservices.API.Data;
using StockMicroservices.API.Errors;
using StockMicroservices.API.EventBusConsumer;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Services;
using StockMicroservices.EventBus.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using MassTransit.AspNetCoreIntegration;

namespace StockMicroservices.API
{
    public class Startup
    {
        public IWebHostEnvironment _WebHostEnvironment { get; }
        public IConfiguration _Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _Configuration = configuration;
            _WebHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Enum as strings
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // CamelCase naming
                options.JsonSerializerOptions.WriteIndented = true; // Indented JSON (optional)
            });

            string identityServerUrl = _Configuration.GetValue(typeof(string), "IdentityServerUrl").ToString();
            string apiScope = _Configuration.GetValue(typeof(string), "APIScope").ToString();

            //Database
            string connectionString = _Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<StockDbContext>(options => options.UseInMemoryDatabase("InMemoryDbFor"));

            //To run Stock.API.Test comeent out lines 47 - 64 and comment out the Authorization headers on the controllers
            services.AddAuthentication(options => options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer("Bearer",
                                  config =>
                                  {
                                      config.Authority = identityServerUrl;
                                      config.Audience = apiScope;
                                      config.RequireHttpsMetadata = false;
                                      config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                      {
                                          ValidateAudience = false,
                                          ValidateIssuer = false
                                      };
                                  });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("StockAPIPolicy",
                                  policy =>
                                  {
                                      policy.AuthenticationSchemes.Add("Bearer");
                                      policy.AddRequirements(new StockMicroservices.API.Authorization.StockAPIRequirement(apiScope));
                                  });

            });

            services.AddScoped<IAuthorizationHandler, StockMicroservices.API.Authorization.StockAPIRequirementHandler>();
            services.AddCors(config =>
            {
                config.AddPolicy("AllowAll",
                                 p =>
                                 {
                                     p.AllowAnyOrigin();
                                     p.AllowAnyHeader();
                                     p.AllowAnyMethod();

                                 });
            });


            //AutoMapper
            var mappingConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockAPI", Version = "v1" });
                                   });

            string _hostname = _Configuration["RabbitMq:Hostname"];
            string _username = _Configuration["RabbitMq:Username"];
            string _password = _Configuration["RabbitMq:Password"];
            string hostAddress = string.Format("amqp://{0}:{1}@{2}:5672", _username, _password, _hostname);

            //MassTransit-RabbitMq
            if (!_WebHostEnvironment.IsEnvironment("test"))
            {
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<StockUpdateConsumer>();

                    config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(hostAddress);

                        cfg.ReceiveEndpoint(EventBusConstants.STOCK_UPDATE_QUEUE, ep =>
                        {
                            ep.ConfigureConsumer<StockUpdateConsumer>(provider);
                        });
                    }));
                });
                //services.AddMassTransitHostedService();
                services.AddSingleton<IHostedService, MassTransitHostedService>();
            }

            //StockMarketService
            services.AddScoped<IStockMarketService, StockMarketService>();

            //Repositories
            services.AddScoped<IRepository<StockOrder>, StockOrderRepository>();
            services.AddScoped<IRepository<StockPosition>, StockPositionRepository>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
            services.AddScoped<IRepository<StockHistory>, StockHistoryRepository>();
            services.Configure<DatabaseSetting>(_Configuration.GetSection("Database"));

            services.AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StockDbContext stockDbContext)
        {
            if (env.IsEnvironment("local") || env.IsEnvironment("development"))
            {
                SeedData.InitializeDB(stockDbContext);
            }


            app.UseSwagger();

            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
                             });

            

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var message = (contextFeature.Error != null) ? contextFeature.Error.Message : "Internal Server Error (500)";
                        if(contextFeature.Endpoint != null)
                        {
                            message += String.Format(" {0}", contextFeature.Endpoint);
                        }

                        var errorMessage = new ErrorMessage
                        {
                            Message = message,
                            StackTrace = (contextFeature.Error != null) ? contextFeature.Error.StackTrace : string.Empty
                        };

                        var jsonString = JsonConvert.SerializeObject(errorMessage);

                        await context.Response.WriteAsync(jsonString);
                    }
                });
            });

            //app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}
