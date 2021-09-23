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
            services.AddOcelot();
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

            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseOcelot();
        }
    }
}
