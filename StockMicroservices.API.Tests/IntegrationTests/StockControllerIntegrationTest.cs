using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using DTOs = StockMicroservices.API.Models.Dtos;

namespace StockMicroservices.API.Tests.IntegrationTests
{
    public class StockControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Url = "api/stock/";
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public StockControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(Url);
            var stocks = await response.Content.ReadFromJsonAsync<List<DTOs.Stock>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stocks);
            Assert.NotEmpty(stocks!);
        }

        [Theory]
        [InlineData("e656bb6f9a358bcc3ae63c61")]
        [InlineData("057340ee83ddedcbeef5b1ab")]
        public async Task Get_ValidStockId_ReturnsStock(string stockId)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(Url + stockId);
            var stock = await response.Content.ReadFromJsonAsync<DTOs.Stock>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stock);
            Assert.Equal(stockId, stock!.Id);
        }

        [Fact]
        public async Task Get_UnknownStockId_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(Url + "e6545b6f9a358bcc3ae63c63");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_InvalidStockId_ReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(Url + "not-a-valid-id");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
