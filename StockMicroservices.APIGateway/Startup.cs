using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;

namespace StockMicroservices.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var authenticationProviderKey = "stockIdentitykey";
            string identityServerUrl = Configuration.GetValue(typeof(string), "IdentityServerUrl").ToString();
            string apiScope = Configuration.GetValue(typeof(string), "APIScope").ToString();
            services.AddAuthentication(options => options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer("Bearer", config =>
                 {
                     config.Authority = identityServerUrl;
                     config.RequireHttpsMetadata = false;
                     config.Audience = apiScope;
                     config.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateAudience = false,
                         ValidateIssuer = false
                     };
                 });

            services.AddOcelot();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appError =>
                                    {
                                        appError.Run(async context =>
                                                     {
                                                         IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                                                         if (contextFeature != null)
                                                         {
                                                             if (contextFeature.Error != null)
                                                             {
                                                                 Console.WriteLine(contextFeature.Error.Message);
                                                             }
                                                         }
                                                     });
                                    });

            //app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseOcelot();
            //app.UseOcelot(new OcelotPipelineConfiguration
            //{
            //    AuthenticationMiddleware = async (cpt, est) =>
            //    {
            //        Console.WriteLine("Ocelot Auth");
            //        await est.Invoke();
            //    }
            //});
        }
    }
}
