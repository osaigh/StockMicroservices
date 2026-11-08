using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockMicroservices.IdentityServer.Data;
using StockMicroservices.IdentityServer.Models;

namespace StockMicroservices.IdentityServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var keysPath = _configuration["IdentityServer:DataProtectionPath"];
            if (string.IsNullOrWhiteSpace(keysPath))
            {
                keysPath = Path.Combine(_environment.ContentRootPath, "keys");
            }

            Directory.CreateDirectory(keysPath);

            var dataProtection = services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
                .SetApplicationName("StockMicroservices");

            var certificate = LoadCertificate();
            if (certificate is not null)
            {
                dataProtection.ProtectKeysWithCertificate(certificate);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Identity"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Stock.Identity";
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Logout";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromHours(6);
                options.SlidingExpiration = true;
            });

            var identityServer = services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/Home/Login";
                    options.UserInteraction.LogoutUrl = "/Home/Logout";
                    options.Authentication.CookieLifetime = TimeSpan.FromHours(6);

                    var issuerUri = _configuration["IdentityServer:IssuerUri"];
                    if (!string.IsNullOrWhiteSpace(issuerUri))
                    {
                        options.IssuerUri = issuerUri;
                    }
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApis())
                .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryClients(IdentityServerConfiguration.GetClients(_configuration));

            if (certificate is not null)
            {
                identityServer.AddSigningCredential(certificate);
            }
            else
            {
                identityServer.AddDeveloperSigningCredential(
                    persistKey: true,
                    filename: Path.Combine(keysPath, "tempkey.jwk"));
            }

            services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.WithOrigins(IdentityServerConfiguration.GetAllowedCorsOrigins(_configuration))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                context.Response.Headers["Content-Security-Policy"] =
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline'; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "img-src 'self' data:; " +
                    "font-src 'self' data:; " +
                    "connect-src 'self'; " +
                    "frame-ancestors 'self'";

                await next();
            });

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseRouting();
            app.UseCors("Default");
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health");
            });
        }

        private X509Certificate2? LoadCertificate()
        {
            var certificatePath = _configuration["IdentityServer:CertificatePath"];
            if (string.IsNullOrWhiteSpace(certificatePath))
            {
                return null;
            }

            if (!Path.IsPathRooted(certificatePath))
            {
                certificatePath = Path.Combine(_environment.ContentRootPath, certificatePath);
            }

            if (!File.Exists(certificatePath))
            {
                throw new FileNotFoundException(
                    $"IdentityServer:CertificatePath points to a file that does not exist: {certificatePath}");
            }

            var password = _configuration["IdentityServer:CertificatePassword"] ?? string.Empty;
            return new X509Certificate2(certificatePath, password);
        }
    }
}
