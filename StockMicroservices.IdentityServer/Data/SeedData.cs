using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.IdentityServer.Models;

namespace StockMicroservices.IdentityServer.Data
{
    public class SeedData
    {
        public static void InitializeDatabase(IServiceProvider serviceProvider, bool isInMemoryDatabase = false)
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                //serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                //test user
                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!appDbContext.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = new ApplicationUser()
                    {
                        UserName = "Steam",
                        FirstName = "Steam ",
                        LastName = "Jack",
                    };
                    userManager.CreateAsync(user, "Qwert@1").GetAwaiter().GetResult();
                    //userManager.AddClaimAsync(user, new Claim("meth", "big-things")).GetAwaiter().GetResult();
                }

                if (isInMemoryDatabase)
                {
                    return;
                }
                //config
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfiguration.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfiguration.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
