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
               
                //test user
                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!appDbContext.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = new ApplicationUser()
                    {
                        UserName = "bob",
                        FirstName = "Bob",
                        LastName = "Stack",
                    };
                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim("meth", "big-things")).GetAwaiter().GetResult();
                }

                
            }
        }
    }
}
