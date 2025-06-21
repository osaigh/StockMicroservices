using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;
using Newtonsoft.Json;
using Xunit;

namespace StockMicroservices.API.Tests.IntegrationTests
{
    public class StockControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region Fields

        private string url = "api/stock/";
        private CustomWebApplicationFactory<Startup> _Factory;
        #endregion

        #region Constructor
        public StockControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var server = _Factory.Server;
            //Act
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stocks = JsonConvert.DeserializeObject<List<DTOs.Stock>>(jsonString);

            //Assert
            Assert.NotNull(stocks);
            Assert.NotEmpty(stocks);
        }

        [Theory]
        [InlineData("e656bb6f9a358bcc3ae63c61")]
        [InlineData("057340ee83ddedcbeef5b1ab")]
        public async Task Get_ValidStockId_ReturnsStock(string stockId)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);

            //Assert
            Assert.NotNull(stock);
            Assert.Equal(stockId.ToString(), stock.Id.ToString());
        }

        [Theory]
        [InlineData("e6545b6f9a358bcc3ae63c63")]
        public async Task Get_InvalidStockId_ReturnsNull(string stockId)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);

            //Assert
            Assert.Null(stock);
        }

        #endregion
    }
}
