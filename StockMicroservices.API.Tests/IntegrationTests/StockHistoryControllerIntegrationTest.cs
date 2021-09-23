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
    public class StockHistoryControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region Fields

        private string url = "api/stockhistory/";
        private CustomWebApplicationFactory<Startup> _Factory;
        #endregion

        #region Constructor
        public StockHistoryControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockHistoryCollection()
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.NotNull(stockHistories);
            Assert.NotEmpty(stockHistories);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_ValidStockId_ReturnsStockHistoryCollection(int stockId)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.NotNull(stockHistories);
            Assert.NotEmpty(stockHistories);
        }

        [Theory]
        [InlineData(1098)]
        public async Task Get_InvalidStockId_ReturnsEmptyCollection(int stockId)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.NotNull(stockHistories);
            Assert.Empty(stockHistories);
        }

        #endregion
    }
}
