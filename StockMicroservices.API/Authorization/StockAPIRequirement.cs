using Microsoft.AspNetCore.Authorization;

namespace StockMicroservices.API.Authorization
{
    public class StockAPIRequirement : IAuthorizationRequirement
    {
        public string APIScope { get; }

        public StockAPIRequirement(string apiScope)
        {
            APIScope = apiScope;
        }
    }
}
