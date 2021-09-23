using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StockMicroservices.IdentityServer.Data;
using StockMicroservices.IdentityServer.Extensions;
using StockMicroservices.IdentityServer.Models;

namespace StockMicroservices.IdentityServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(Startup).Assembly.GetName().Name;
            //var filePath = Path.Combine(_environment.ContentRootPath, "stock.pfx");
            var cert = new X509Certificate2("stock.pfx", "password");
            services.AddControllersWithViews();
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var"))
                    .ProtectKeysWithCertificate(cert);
            services.AddDbContext<ApplicationDbContext>(config =>
                                                        {
                                                            config.UseInMemoryDatabase("Identity");
                                                        });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                                                                {
                                                                    config.Password.RequiredLength = 4;
                                                                    config.Password.RequireDigit = false;
                                                                    config.Password.RequireNonAlphanumeric = false;
                                                                    config.Password.RequireUppercase = false;
                                                                })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
                                                {
                                                    config.Cookie.Name = "Identity.Cookie";
                                                    config.LoginPath = "/Home/Login";
                                                    config.LogoutPath = "/Home/Logout";
                                                    config.Cookie.SameSite = SameSiteMode.None;
                                                    config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                                                });

            services.ConfigureNonBreakingSameSiteCookies();

            services.AddIdentityServer()
                    .AddAspNetIdentity<ApplicationUser>()
                    //.AddConfigurationStore(options =>
                    //                       {
                    //                           options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    //                                                                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    //                       })
                    //.AddOperationalStore(options =>
                    //                     {
                    //                         options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    //                                                                          sql => sql.MigrationsAssembly(migrationsAssembly));
                    //                     })
                    .AddSigningCredential(cert)
                    .AddInMemoryApiResources(IdentityServerConfiguration.GetApis())
                    .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                    .AddInMemoryClients(IdentityServerConfiguration.GetClients());
                    //.AddDeveloperSigningCredential();

            services.ConfigureCorsPolicy(new List<string>() { "https://localhost:44382", "https://localhost:3000" }, Configuration);

            
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SeedData.InitializeDatabase(app.ApplicationServices, true);
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapDefaultControllerRoute();
                             });
        }
    }
}
