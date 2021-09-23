using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockMicroservices.IdentityServer.Extensions
{
    internal static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCorsPolicy(this IServiceCollection services, List<Client> clients, IConfiguration configuration)
        {
            List<string> allowed = clients.SelectMany(client => client.AllowedCorsOrigins).Distinct().ToList();
            string myself = configuration.GetValue("IdentityServer:Authority", string.Empty);

            if (!string.IsNullOrEmpty(myself) && !allowed.Contains(myself))
            {
                allowed.Add(myself);
            }

            services.AddCors(options =>
            {
                options.AddPolicy("ucwCorsPolicy",
                                  policy =>
                                  {
                                      policy.WithOrigins(allowed.ToArray())
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials();
                                  });
            });

            return services;
        }

        public static IServiceCollection ConfigureCorsPolicy(this IServiceCollection services, List<string> allowed, IConfiguration configuration)
        {
            ;
            string myself = configuration.GetValue("IdentityServer:Authority", string.Empty);

            if (!string.IsNullOrEmpty(myself) && !allowed.Contains(myself))
            {
                allowed.Add(myself);
            }

            services.AddCors(options =>
            {
                options.AddPolicy("ucwCorsPolicy",
                                  policy =>
                                  {
                                      policy.WithOrigins(allowed.ToArray())
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials();
                                  });
            });

            return services;
        }
    }
}
