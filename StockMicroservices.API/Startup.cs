using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StockMicroservices.API.Data;
using StockMicroservices.API.Errors;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Services;

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
            services.AddControllers();

            string identityServerUrl = _Configuration.GetValue(typeof(string), "IdentityServerUrl").ToString();
            string apiScope = _Configuration.GetValue(typeof(string), "APIScope").ToString();

            services.AddSingleton<IStockDbContext, StockDbContext>();

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

            //StockMarketService
            services.AddScoped<IStockMarketService, StockMarketService>();

            //Repositories
            services.AddScoped<IRepository<StockHolder>, StockHolderRepository>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
            services.Configure<DatabaseSetting>(_Configuration.GetSection("Database"));
            services.AddSingleton<IStockUpdateListener, StockUpdateListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStockDbContext stockDbContext, IStockUpdateListener stockUpdateListener)
        {
            SeedData.InitializeDatabase(app.ApplicationServices);
            //if (env.IsDevelopment())
            //{
            //    //app.UseDeveloperExceptionPage();
            //   //
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
                             });

            Task.Run(() =>
                     {
                         stockUpdateListener.StartListener();
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
        }
    }
}
