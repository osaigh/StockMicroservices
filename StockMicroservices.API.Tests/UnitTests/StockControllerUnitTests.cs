using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockMicroservices.API.Controllers.api;
using StockMicroservices.API.Repository;
using Xunit;
using DAOs = StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Tests.UnitTests
{
    public class StockControllerUnitTests
    {
        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            var mockRepository = new Mock<IRepository<DAOs.Stock>>();
            mockRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(Utilities.GetTestStocks());

            var controller = new StockController(mockRepository.Object);

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stocks = Assert.IsAssignableFrom<IEnumerable<StockMicroservices.API.Models.Dtos.Stock>>(okResult.Value);
            Assert.Equal(2, stocks.Count());
        }

        [Theory]
        [InlineData("e656bb6f9a358bcc3ae63c61")]
        public async Task Get_ValidStockId_ReturnsStock(string stockId)
        {
            var mockRepository = new Mock<IRepository<DAOs.Stock>>();
            mockRepository
                .Setup(repository => repository.GetAsync(It.IsAny<object>()))
                .ReturnsAsync((object id) => Utilities.GetTestStocks().FirstOrDefault(stock => stock.Id == id.ToString()));

            var controller = new StockController(mockRepository.Object);

            var result = await controller.Get(stockId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stock = Assert.IsType<StockMicroservices.API.Models.Dtos.Stock>(okResult.Value);
            Assert.Equal(stockId, stock.Id);
        }

        [Theory]
        [InlineData("e656bb6f9a358bcc3ae63c68")]
        public async Task Get_UnknownStockId_ReturnsNotFound(string stockId)
        {
            var mockRepository = new Mock<IRepository<DAOs.Stock>>();
            mockRepository
                .Setup(repository => repository.GetAsync(It.IsAny<object>()))
                .ReturnsAsync((object id) => Utilities.GetTestStocks().FirstOrDefault(stock => stock.Id == id.ToString()));

            var controller = new StockController(mockRepository.Object);

            var result = await controller.Get(stockId);

            Assert.Null(result.Value);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Get_InvalidObjectId_ReturnsBadRequest()
        {
            var mockRepository = new Mock<IRepository<DAOs.Stock>>();

            var controller = new StockController(mockRepository.Object);

            var result = await controller.Get("not-an-object-id");

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
