using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace StockMicroservices.API.Authorization
{
    public class StockAPIRequirementHandler : AuthorizationHandler<StockAPIRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StockAPIRequirement requirement)
        {
            var hasScope = context.User.Claims.Any(claim =>
                (string.Equals(claim.Type, "scope", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(claim.Type, "scp", StringComparison.OrdinalIgnoreCase)) &&
                claim.Value.Split(' ').Any(scope => scope == requirement.APIScope));

            if (hasScope)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
