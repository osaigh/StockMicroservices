using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;
using Newtonsoft.Json;
using StockMicroservices.API.Models;
using Xunit;

namespace StockMicroservices.API.Tests.IntegrationTests
{
    public class StockHolderPositionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region Fields

        private string url = "api/stockholderposition/";
        private CustomWebApplicationFactory<Startup> _Factory;
        #endregion

        #region Constructor
        public StockHolderPositionControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData("Jimmy")]
        public async Task Get_ValidStockHolderId_ReturnsStockHolderPositionCollection(string username)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + username);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolderPositions = JsonConvert.DeserializeObject<List<DTOs.StockHolderPosition>>(jsonString);

            //Assert
            Assert.NotNull(stockHolderPositions);
            Assert.NotEmpty(stockHolderPositions);
        }

        [Theory]
        [InlineData("fake-username")]
        public async Task Get_InvalidStockHolderId_ReturnsEmptyCollection(string username)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + username);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolderPositions = JsonConvert.DeserializeObject<List<DTOs.StockHolderPosition>>(jsonString);

            //Assert
            Assert.NotNull(stockHolderPositions);
            Assert.Empty(stockHolderPositions);
        }

        [Fact]
        public async Task Post_ValidStockHolderPosition_ReturnsStockHolderPosition()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var _stockHolderPosition = new DTOs.StockHolderPosition()
            {
                StockHolderId = "Steve",
                Shares = 30,
                StockId = 2
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(_stockHolderPosition)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolderPosition = JsonConvert.DeserializeObject<DTOs.StockHolderPosition>(jsonString);

            //Assert
            Assert.NotNull(stockHolderPosition);

        }

        [Fact]
        public async Task Post_InvalidStockHolderPosition_ReturnsNull()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var _stockHolderPosition = new DTOs.StockHolderPosition()
            {
                StockHolderId = string.Empty,
                Shares = 30,
                Id = 345,
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(_stockHolderPosition)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolderPosition = JsonConvert.DeserializeObject<DTOs.StockHolderPosition>(jsonString);

            //Assert
            Assert.Null(stockHolderPosition);
        }
        #endregion
    }
}
