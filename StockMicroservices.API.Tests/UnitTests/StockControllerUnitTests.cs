using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using StockMicroservices.API.Controllers.api;
using StockMicroservices.API.Repository;
using Xunit;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;

namespace StockMicroservices.API.Tests.UnitTests
{
    public class StockControllerUnitTests
    {
        #region Fields

        #endregion

        #region Constructor
        public StockControllerUnitTests()
        {

        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Stock Repo
            var mockRepo = new Mock<IRepository<DAOs.Stock>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Utilities.GetTestStocks());

            //Controller
            var stockController = new StockController(mockRepo.Object, mapper);

            //Act
            var stocks = await stockController.Get();

            //Assert
            Assert.NotNull(stocks);
            Assert.Equal(2, stocks.Count());
        }

        [Theory]
        [InlineData(1)]
        public async Task Get_ValidStockId_ReturnsStock(int stockId)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Stock Repo
            var mockRepo = new Mock<IRepository<DAOs.Stock>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<object>())).ReturnsAsync((object id) =>
            {
                var _id = int.Parse(id.ToString());
                return Utilities.GetTestStocks()
                                .FirstOrDefault(s => s.Id == _id);
            });

            var stockController = new StockController(mockRepo.Object, mapper);

            //Act
            var stock = await stockController.Get(stockId);

            //Assert
            Assert.NotNull(stock);
            Assert.Equal(1, stock.Id);
        }

        [Theory]
        [InlineData(1098)]
        public async Task Get_InvalidStockId_ReturnsNull(int stockId)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Stock Repo
            var mockRepo = new Mock<IRepository<DAOs.Stock>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<object>())).ReturnsAsync((object id) =>
            {
                var _id = int.Parse(id.ToString());
                return Utilities.GetTestStocks()
                                .FirstOrDefault(s => s.Id == _id);
            });

            var stockController = new StockController(mockRepo.Object, mapper);

            //Act
            var stock = await stockController.Get(stockId);

            //Assert
            Assert.Null(stock);
        }

        #endregion

        #region Helpers
        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.Stock>(It.IsAny<DAOs.Stock>())).Returns((DAOs.Stock input) =>
            {
                var dtoStock = new DTOs.Stock
                {
                    Id = input.Id,
                    Name = input.Name,
                    Price = input.Price
                };

                return dtoStock;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.Stock>>(It.IsAny<List<DAOs.Stock>>())).Returns((List<DAOs.Stock> inputs) =>
            {
                return inputs.Select(input => new DTOs.Stock()
                {
                    Id = input.Id,
                    Name = input.Name,
                    Price = input.Price
                })
                                        .ToList();
            });

            return mockMapper.Object;
        }

        #endregion
    }
}
