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
            return new List<ApiResource>() { new ApiResource("ApiOne"), new ApiResource("ApiTwo"), new ApiResource("StockMicroservicesAPI") };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>() { new ApiScope("ApiOne"), new ApiScope("ApiTwo"), new ApiScope("StockMicroservicesAPI") };
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
                                             RedirectUris = { "http://localhost:44100/SignInCallback" },
                                             PostLogoutRedirectUris = { "http://localhost:44100/SignOutCallback" },
                                             AllowedGrantTypes = GrantTypes.Implicit,
                                             AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "StockMicroservicesAPI"},
                                             AllowAccessTokensViaBrowser = true,
                                             AllowOfflineAccess = true,
                                             RequireClientSecret = false,
                                             RequireConsent = false,
                                             AllowedCorsOrigins = { "http://localhost:44100" },
                                             AccessTokenLifetime = 86400,
                                         },
                                         new Client()
                                         {
                                             ClientId = "client_id_react2",
                                             RedirectUris = { "http://localhost:3000/SignInCallback" },
                                             PostLogoutRedirectUris = { "http://localhost:3000/SignOutCallback" },
                                             AllowedGrantTypes = GrantTypes.Implicit,
                                             AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "StockMicroservicesAPI"},
                                             AllowAccessTokensViaBrowser = true,
                                             AllowOfflineAccess = true,
                                             RequireClientSecret = false,
                                             RequireConsent = false,
                                             AllowedCorsOrigins = { "http://localhost:3000" },
                                             AccessTokenLifetime = 86400,
                                         },

                                     };
        }


        /*****Open Id Configuration *******/
        /*
         * https://localhost:44376/.well-known/openid-configuration* 
         */
    }
}
