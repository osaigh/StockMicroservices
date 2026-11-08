using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using DTOs = StockMicroservices.API.Models.Dtos;

namespace StockMicroservices.API.Tests.IntegrationTests
{
    public class StockMarketControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string StockMarketUrl = "api/stockmarket/";
        private const string StockUrl = "api/stock/";
        private const string StockId = "e656bb6f9a358bcc3ae63c61";

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public StockMarketControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Put_ValidRequest_UpdatesStockPrices()
        {
            var client = _factory.CreateClient();

            var before = await client.GetFromJsonAsync<DTOs.Stock>(StockUrl + StockId);
            Assert.NotNull(before);

            var response = await client.PutAsync(StockMarketUrl, new StringContent(string.Empty));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var after = await client.GetFromJsonAsync<DTOs.Stock>(StockUrl + StockId);
            Assert.NotNull(after);
            Assert.NotEqual(before!.Price, after!.Price);
        }
    }
}
