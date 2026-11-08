using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace StockMicroservices.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityServerUrl = RequireSetting("IdentityServerUrl");
            var apiScope = RequireSetting("APIScope");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerUrl;
                    options.Audience = apiScope;
                    options.RequireHttpsMetadata = Configuration.GetValue("IdentityServer:RequireHttpsMetadata", false);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
                });

            services.AddAuthorization();
            services.AddOcelot(Configuration);
            services.AddHealthChecks();

            var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var feature = context.Features.Get<IExceptionHandlerFeature>();
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            message = "An unexpected error occurred.",
                            detail = env.IsDevelopment() ? feature?.Error?.Message : null,
                        }));
                    });
                });
            }

            app.UseCors("Default");
            app.UseRouting();

            app.UseHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            app.UseHealthChecks("/health/ready");

            app.UseAuthentication();
            app.UseOcelot().GetAwaiter().GetResult();
        }

        private string RequireSetting(string key)
        {
            var value = Configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Configuration value '{key}' is required.");
            }

            return value;
        }
    }
}
