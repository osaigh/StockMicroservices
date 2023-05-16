using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace StockMicroservices.API.Authorization
{
    public class StockAPIRequirementHandler : AuthorizationHandler<StockAPIRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StockAPIRequirement requirement)
        {
            Debug.WriteLine(requirement.APIScope);
            Console.WriteLine(requirement.APIScope);
            int count = 0;
            foreach(var c in context.User.Claims)
            {
                count++;
            }
            if (!context.User.HasClaim(c => c.Value != null && c.Value == requirement.APIScope))
            {
                context.Fail();
                Debug.WriteLine("Failed validation "+ count);
                Console.WriteLine("Failed validation " + count);
            }
            else
            {
                Console.WriteLine("Succeeded validation");
                Debug.WriteLine("Succeeded validation");
                context.Succeed(requirement);
            }


            return Task.FromResult(0);
        }
    }
}
