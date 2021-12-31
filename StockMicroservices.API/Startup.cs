using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
        public IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ////Database
            //string connectionString = _Configuration.GetConnectionString("DefaultConnection");

            //services.AddAuthentication("Bearer")
            //        .AddJwtBearer("Bearer",
            //                      config =>
            //                      {
            //                          config.Authority = "https://localhost:44376/";
            //                          config.Audience = "StockAPI";
            //                      });

            //services.AddAuthorization(options =>
            //                          {
            //                              options.AddPolicy("StockAPIPolicy",
            //                                                policy =>
            //                                                {
            //                                                    policy.AuthenticationSchemes.Add("Bearer");
            //                                                    policy.RequireScope("StockAPI");
            //                                                });

            //                          });

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

            //if (_WebHostEnvironment.IsDevelopment())
            //{
            //    if (connectionString.Contains("%CONTENTROOTPATH%"))
            //    {
            //        connectionString = connectionString.Replace("%CONTENTROOTPATH%", _WebHostEnvironment.ContentRootPath);
            //    }
            //}
            //services.AddDbContext<StockDbContext>(options => options.UseSqlServer(connectionString));

            //services.AddDbContext<StockDbContext>(options => options.UseInMemoryDatabase("InMemoryDbFor"));

            services.AddDbContext<StockDbContext>(options => options.UseSqlServer(Configuration["ConnectionString"],sqlOptions => {
                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            }));

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
            services.AddScoped<IRepository<StockHolderPosition>, StockHolderPositionRepository>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
            services.AddScoped<IRepository<StockHistory>, StockHistoryRepository>();
            services.AddScoped<IRepository<StockPrice>, StockPriceRepository>();
            services.AddSingleton<IStockUpdateListener, StockUpdateListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StockDbContext stockDbContext, IStockUpdateListener stockUpdateListener)
        {
            SeedData.InitializeDB(stockDbContext);
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

                        var errorMessage = new ErrorMessage
                        {
                            Message = (contextFeature.Error != null) ? contextFeature.Error.Message : "Internal Server Error",
                            StackTrace = (contextFeature.Error != null) ? contextFeature.Error.StackTrace : string.Empty
                        };

                        var jsonString = JsonConvert.SerializeObject(errorMessage);

                        await context.Response.WriteAsync(jsonString);
                    }
                });
            });

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
