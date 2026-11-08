using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockMicroservices.API.Data;
using StockMicroservices.API.Tests.Persistence;

namespace StockMicroservices.API.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private MongoDbTestContext? _mongoDbContext;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");

            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(IStockDbContext));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                _mongoDbContext = new MongoDbTestContext($"stocks-{Guid.NewGuid():N}");
                services.AddSingleton<IStockDbContext>(_mongoDbContext);

                services.AddAuthentication(TestAuthHandler.SchemeName)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

                Utilities.InitializeDbForTestsAsync(_mongoDbContext).GetAwaiter().GetResult();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _mongoDbContext?.Dispose();
                _mongoDbContext = null;
            }
        }
    }

    public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim("sub", "test-user"),
                new System.Security.Claims.Claim("scope", "StockMicroservicesAPI"),
                new System.Security.Claims.Claim("name", "Test User"),
            };

            var identity = new System.Security.Claims.ClaimsIdentity(claims, SchemeName);
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
