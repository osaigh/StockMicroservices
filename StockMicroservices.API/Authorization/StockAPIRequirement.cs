using Microsoft.AspNetCore.Authorization;

namespace StockMicroservices.API.Authorization
{
    public class StockAPIRequirement : IAuthorizationRequirement
    {
        #region Properties
        public string APIScope { get; set; }
        #endregion

        #region Constructor
        public StockAPIRequirement(string apiScope)
        {
            this.APIScope = apiScope;
        }
        #endregion
    }
}
