using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace StockMicroservices.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>() {  new ApiResource("StockMicroservicesAPI") };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
                   {
                       new IdentityResources.OpenId(),
                       new IdentityResources.Email(),
                       new IdentityResources.Profile(),
                   };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>(){
                                         new Client()
                                         {
                                             ClientId = "client_id_react",
                                             RedirectUris = { "https://localhost:44382/SignInCallback" },
                                             PostLogoutRedirectUris = { "https://localhost:44382/SignOutCallback" },
                                             AllowedGrantTypes = GrantTypes.Implicit,
                                             AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "StockMicroservicesAPI"},
                                             AllowAccessTokensViaBrowser = true,
                                             RequireClientSecret = false,
                                             RequireConsent = false,
                                             AllowedCorsOrigins = { "https://localhost:44382" },
                                             AccessTokenLifetime = 1,
                                         },
                                         new Client()
                                         {
                                             ClientId = "client_id_react2",
                                             RedirectUris = { "https://localhost:3000/SignInCallback" },
                                             PostLogoutRedirectUris = { "https://localhost:3000/SignOutCallback" },
                                             AllowedGrantTypes = GrantTypes.Implicit,
                                             AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "StockMicroservicesAPI"},
                                             AllowAccessTokensViaBrowser = true,
                                             RequireClientSecret = false,
                                             RequireConsent = false,
                                             AllowedCorsOrigins = { "https://localhost:3000" },
                                             AccessTokenLifetime = 1,
                                         },

                                     };
        }


        /*****Open Id Configuration *******/
        /*
         * https://localhost:44376/.well-known/openid-configuration* 
         */
    }
}
