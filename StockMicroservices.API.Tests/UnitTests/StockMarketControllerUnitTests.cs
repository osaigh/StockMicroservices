using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StockMicroservices.API.Controllers.api;
using StockMicroservices.API.Models;
using StockMicroservices.API.Services;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;
using Xunit;

namespace StockMicroservices.API.Tests.UnitTests
{
    public class StockMarketControllerUnitTests
    {
        #region Fields

        private List<DAOs.Stock> stocks = null;
        #endregion

        #region Constructor
        public StockMarketControllerUnitTests()
        {
            stocks = Utilities.GetTestStocks();
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Put_ValidRequest_UpdatesStockPrices()
        {
            //Arrange
            var microsoftStock = stocks.FirstOrDefault(s => s.Name.ToLower() == "microsoft");
            double oldPrice = microsoftStock.Price;
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        RequestId = Guid.NewGuid().ToString()
                                    };
            //MarketService
            var marketService = GetStockMarketService();

            var stockMarketController = new StockMarketController(marketService);

            //Act
            await stockMarketController.Update(apiRequest);

            //Assert
            Assert.NotEqual(oldPrice, microsoftStock.Price);
        }

        #endregion

        #region Helpers
        private IStockMarketService GetStockMarketService()
        {
            //StockMarketService
            var mockMapper = new Mock<IStockMarketService>();

            mockMapper.Setup(marketService => marketService.UpdateStockPrices()).Callback(() =>
                                                                                          {
                                                                                              foreach (var stock in stocks)
                                                                                              {
                                                                                                  double newPrice = stock.Price + 31;
                                                                                                  stock.Price = newPrice > 0 ? newPrice : 1;
                                                                                              }
                                                                                          });

            return mockMapper.Object;
        }

        #endregion
    }
}
