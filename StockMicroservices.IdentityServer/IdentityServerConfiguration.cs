using System;
using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace StockMicroservices.IdentityServer
{
    public static class IdentityServerConfiguration
    {
        public const string ApiName = "StockMicroservicesAPI";

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource(ApiName, "Stock Microservices API")
                {
                    Scopes = { ApiName },
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[] { new ApiScope(ApiName, "Stock Microservices API") };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var webClientUrl = GetWebClientUrl(configuration);
            var webClientDevUrl = GetWebClientDevUrl(configuration);

            return new List<Client>
            {
                CreateBrowserClient("client_id_react", webClientUrl),
                CreateBrowserClient("client_id_react_dev", webClientDevUrl),
            };
        }

        public static string[] GetAllowedCorsOrigins(IConfiguration configuration)
        {
            var configured = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
            var origins = configured
                .Concat(new[] { GetWebClientUrl(configuration), GetWebClientDevUrl(configuration) })
                .Where(origin => !string.IsNullOrWhiteSpace(origin))
                .Select(origin => origin.TrimEnd('/'))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return origins;
        }

        private static Client CreateBrowserClient(string clientId, string origin)
        {
            return new Client
            {
                ClientId = clientId,
                ClientName = "Stock web client",
                RedirectUris = { $"{origin}/signincallback" },
                PostLogoutRedirectUris = { $"{origin}/signoutcallback" },
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    ApiName,
                },
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret = false,
                RequireConsent = false,
                AllowedCorsOrigins = { origin },
                AccessTokenLifetime = 3600,
                IdentityTokenLifetime = 300,
            };
        }

        private static string GetWebClientUrl(IConfiguration configuration)
        {
            return (configuration["WebClientUrl"] ?? "http://localhost:44100").TrimEnd('/');
        }

        private static string GetWebClientDevUrl(IConfiguration configuration)
        {
            return (configuration["WebClientDevUrl"] ?? "http://localhost:3000").TrimEnd('/');
        }
    }
}
