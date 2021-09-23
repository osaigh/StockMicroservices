using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockMicroservices.API.Models;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;
using Xunit;

namespace StockMicroservices.API.Tests.IntegrationTests
{
    public class StockHolderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region Fields

        private string url = "api/stockholder/";
        private CustomWebApplicationFactory<Startup> _Factory;
        #endregion

        #region Constructor
        public StockHolderControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Get_NoArgument_ReturnsStockHolderCollection()
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolders = JsonConvert.DeserializeObject<List<DTOs.StockHolder>>(jsonString);

            //Assert
            Assert.NotNull(stockHolders);
            Assert.NotEmpty(stockHolders);
        }

        [Theory]
        [InlineData("Jimmy")]
        [InlineData("Priel")]
        [InlineData("Steve")]
        public async Task Get_ValidUsername_ReturnsStockHolder(string username)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + username);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(username.ToLower(), stockHolder.Username.ToLower());
        }

        [Theory]
        [InlineData("wrong-name")]
        public async Task Get_InvalidUsername_ReturnsNull(string username)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url + username);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);

            //Assert
            Assert.Null(stockHolder);
        }

        [Fact]
        public async Task Put_ValidStockHolder_ReturnsStockHolder()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var jimmyStockHolder = new DTOs.StockHolder()
            {
                Username = "Jimmy",
                Email = "jimmy@zend.com",
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(jimmyStockHolder)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(jimmyStockHolder.Username, stockHolder.Username);
            Assert.Equal(jimmyStockHolder.Email, stockHolder.Email);
        }

        [Fact]
        public async Task Put_InvalidStockHolder_ReturnsNull()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var abrahamStockHolder = new DTOs.StockHolder()
            {
                Username = "Abraham",
                Email = "abraham@zend.com",
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(abrahamStockHolder)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);


            //Assert
            Assert.Null(stockHolder);
        }

        [Fact]
        public async Task Post_ValidStockHolder_ReturnsStockHolder()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var gabiStockHolder = new DTOs.StockHolder()
            {
                Username = "Gabi",
                Email = "gabi@rely.com",
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(gabiStockHolder)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(gabiStockHolder.Username, stockHolder.Username);

        }

        [Fact]
        public async Task Post_InvalidStockHolder_ReturnsNull()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var gabiStockHolder = new DTOs.StockHolder()
            {
                Username = string.Empty,
                Email = "tret@yahoo.com",
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(gabiStockHolder)
            };

            string jsonIn = JsonConvert.SerializeObject(apiRequest);

            StringContent stringContent = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(url, stringContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stockHolder = JsonConvert.DeserializeObject<DTOs.StockHolder>(jsonString);

            //Assert
            Assert.Null(stockHolder);
        }
        #endregion
    }
}
