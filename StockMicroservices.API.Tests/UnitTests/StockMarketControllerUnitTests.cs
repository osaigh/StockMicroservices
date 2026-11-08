using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockMicroservices.API.Controllers.api;
using StockMicroservices.API.Services;
using Xunit;
using DAOs = StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Tests.UnitTests
{
    public class StockMarketControllerUnitTests
    {
        private readonly List<DAOs.Stock> _stocks = Utilities.GetTestStocks();

        [Fact]
        public async Task Update_ValidRequest_UpdatesStockPrices()
        {
            var oldPrice = _stocks.First(stock => stock.Name == "Microsoft").Price;
            var service = GetStockMarketService();
            var controller = new StockMarketController(service);

            var result = await controller.Update();

            Assert.IsType<NoContentResult>(result);
            Assert.NotEqual(oldPrice, _stocks.First(stock => stock.Name == "Microsoft").Price);
        }

        private IStockMarketService GetStockMarketService()
        {
            var mockService = new Mock<IStockMarketService>();
            mockService
                .Setup(service => service.UpdateStockPrices())
                .Returns(Task.CompletedTask)
                .Callback(() =>
                {
                    foreach (var stock in _stocks)
                    {
                        stock.Price += 31;
                    }
                });

            return mockService.Object;
        }
    }
}
