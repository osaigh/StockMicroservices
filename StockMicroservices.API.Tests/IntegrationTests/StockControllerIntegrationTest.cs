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
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_ValidStockId_ReturnsStock(int stockId)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);

            //Assert
            Assert.NotNull(stock);
            Assert.Equal(stockId, stock.Id);
        }

        [Theory]
        [InlineData(1098)]
        public async Task Get_InvalidStockId_ReturnsNull(int stockId)
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
