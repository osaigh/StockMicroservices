using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockMicroservices.IdentityServer.Models;

namespace StockMicroservices.IdentityServer.Data
{
    public static class SeedData
    {
        public const string DemoUserName = "bob";
        public const string DemoPassword = "password";

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeedData");
            var appDbContext = services.GetRequiredService<ApplicationDbContext>();

            if (appDbContext.Users.Any())
            {
                logger.LogInformation("Identity database already contains users; seeding skipped.");
                return;
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var user = new ApplicationUser
            {
                UserName = DemoUserName,
                Email = "bob@example.com",
                FirstName = "Bob",
                LastName = "Stack",
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(user, DemoPassword);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Could not create the demo user: {errors}");
            }

            var claimResult = await userManager.AddClaimAsync(user, new Claim("meth", "big-things"));
            if (!claimResult.Succeeded)
            {
                throw new InvalidOperationException("Could not add the demo user claim.");
            }

            logger.LogInformation("Created demo user '{User}' with password '{Password}'.", DemoUserName, DemoPassword);
        }
    }
}
