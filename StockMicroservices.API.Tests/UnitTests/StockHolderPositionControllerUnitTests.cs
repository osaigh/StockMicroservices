using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using StockMicroservices.API.Controllers.api;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.API.Repository;
using DAOs = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;
using Xunit;

namespace StockMicroservices.API.Tests.UnitTests
{
    public class StockHolderPositionControllerUnitTests
    {
        #region Fields

        #endregion

        #region Constructor
        public StockHolderPositionControllerUnitTests()
        {

        }
        #endregion

        #region Methods

        [Theory]
        [InlineData("Jimmy")]
        public async Task Get_ValidStockHolderId_ReturnsStockHolderPositionCollection(string username)
        {
            //Arrange
            //AutoMapper
            var mapper = GetMapper();
            var mockRepo = new Mock<IRepository<DAOs.StockHolderPosition>>();
            mockRepo.Setup(repo => repo.SearchForAsync(It.IsAny<Expression<Func<DAOs.StockHolderPosition, bool>>>()))
                    .ReturnsAsync((Expression<Func<DAOs.StockHolderPosition, bool>> predicate) =>
                    {
                        var stockHolderPositions = Utilities.GetTestStockHolderPositions();

                        return (stockHolderPositions.AsQueryable()).Where(predicate);
                    });

            var stockHolderPositionController = new StockHolderPositionController(mockRepo.Object, mapper);

            //Act
            var stockHolderPositions = await stockHolderPositionController.Get(username);

            //Assert
            Assert.NotNull(stockHolderPositions);
            Assert.Equal(3, stockHolderPositions.Count());
        }

        [Theory]
        [InlineData("")]
        public async Task Get_InvalidStockHolderId_ReturnsEmptyCollection(string username)
        {
            //Arrange
            //AutoMapper
            var mapper = GetMapper();
            var mockRepo = new Mock<IRepository<DAOs.StockHolderPosition>>();
            mockRepo.Setup(repo => repo.SearchForAsync(It.IsAny<Expression<Func<DAOs.StockHolderPosition, bool>>>()))
                    .ReturnsAsync((Expression<Func<DAOs.StockHolderPosition, bool>> predicate) =>
                    {
                        var stockHolderPositions = Utilities.GetTestStockHolderPositions();

                        return (stockHolderPositions.AsQueryable()).Where(predicate);
                    });

            var stockHolderPositionController = new StockHolderPositionController(mockRepo.Object, mapper);

            //Act
            var stockHolderPositions = await stockHolderPositionController.Get(username);

            //Assert
            Assert.NotNull(stockHolderPositions);
            Assert.Empty(stockHolderPositions);
        }

        [Fact]
        public async Task Post_ValidStockHolderPosition_ReturnsStockHolderPosition()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();
            var stockHolderPositions = Utilities.GetTestStockHolderPositions();
            int index = 1;
            foreach (DAOs.StockHolderPosition shp in stockHolderPositions)
            {
                shp.Id = index;
                index++;
            }
            var _stockHolderPosition = new DTOs.StockHolderPosition()
            {
                StockHolderId = "Jimmy",
                Shares = 30,
                CostBasis = 1000,
                StockId = 1
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(_stockHolderPosition)
            };
            var mockRepo = new Mock<IRepository<DAOs.StockHolderPosition>>();
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.StockHolderPosition>())).ReturnsAsync((DAOs.StockHolderPosition newStockHolderPosition) =>
            {
                stockHolderPositions.Add((StockHolderPosition) (StockHolderPosition)newStockHolderPosition);
                newStockHolderPosition.Id = stockHolderPositions.Count + 1;
                return newStockHolderPosition;
            });
            var stockHolderPositionController = new StockHolderPositionController(mockRepo.Object, mapper);

            //Act
            var stockHolderPosition = await stockHolderPositionController.Post(apiRequest);

            //Assert
            Assert.NotNull(stockHolderPosition);
            Assert.NotEqual(0, stockHolderPosition.Id);
            Assert.Equal(_stockHolderPosition.StockHolderId, stockHolderPosition.StockHolderId);
        }

        [Fact]
        public async Task Post_InvalidStockHolderPosition_ReturnsNull()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();
            var stockHolderPositions = Utilities.GetTestStockHolderPositions();
            var gabeStockHolderPosition = new DTOs.StockHolderPosition()
            {
                StockHolderId = String.Empty,
                Shares = 30,
                CostBasis = 1000
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(gabeStockHolderPosition)
            };
            var mockRepo = new Mock<IRepository<DAOs.StockHolderPosition>>();
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.StockHolderPosition>())).ReturnsAsync((DAOs.StockHolderPosition newStockHolderPosition) =>
            {
                stockHolderPositions.Add((StockHolderPosition) (StockHolderPosition)newStockHolderPosition);
                newStockHolderPosition.Id = stockHolderPositions.Count + 1;
                return newStockHolderPosition;
            });
            var stockHolderPositionController = new StockHolderPositionController(mockRepo.Object, mapper);

            //Act
            var stockHolderPosition = await stockHolderPositionController.Post(apiRequest);

            //Assert
            Assert.Null(stockHolderPosition);
        }

        #endregion

        #region Helpers
        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.StockHolderPosition>(It.IsAny<DAOs.StockHolderPosition>())).Returns((DAOs.StockHolderPosition inputStockHolderPosition) =>
            {
                var dtoStockHolderPosition = new DTOs.StockHolderPosition
                {
                    StockHolderId = inputStockHolderPosition.StockHolderId,
                    Id = inputStockHolderPosition.Id,
                    StockId = inputStockHolderPosition.Id,
                    Shares = inputStockHolderPosition.Shares,
                    CostBasis = inputStockHolderPosition.CostBasis
                };

                return dtoStockHolderPosition;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.StockHolderPosition>>(It.IsAny<List<DAOs.StockHolderPosition>>())).Returns((List<DAOs.StockHolderPosition> inputStockHolderPositions) =>
            {
                return inputStockHolderPositions.Select(inputStockHolderPosition => new DTOs.StockHolderPosition()
                {
                    StockHolderId = inputStockHolderPosition.StockHolderId,
                    Id = inputStockHolderPosition.Id,
                    StockId = inputStockHolderPosition.Id,
                    Shares = inputStockHolderPosition.Shares,
                    CostBasis = inputStockHolderPosition.CostBasis
                })
                                        .ToList();
            });

            mockMapper.Setup(mapper => mapper.Map<DAOs.StockHolderPosition>(It.IsAny<DTOs.StockHolderPosition>())).Returns((DTOs.StockHolderPosition inputStockHolderPosition) =>
            {
                var daoStockHolderPosition = new DAOs.StockHolderPosition
                {
                    StockHolderId = inputStockHolderPosition.StockHolderId,
                    Id = inputStockHolderPosition.Id,
                    StockId = inputStockHolderPosition.Id,
                    Shares = inputStockHolderPosition.Shares,
                    CostBasis = inputStockHolderPosition.CostBasis
                };

                return daoStockHolderPosition;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DAOs.StockHolderPosition>>(It.IsAny<List<DTOs.StockHolderPosition>>())).Returns((List<DTOs.StockHolderPosition> inputStockHolderPositions) =>
            {
                return inputStockHolderPositions.Select(inputStockHolderPosition => new DAOs.StockHolderPosition()
                {
                    StockHolderId = inputStockHolderPosition.StockHolderId,
                    Id = inputStockHolderPosition.Id,
                    StockId = inputStockHolderPosition.Id,
                    Shares = inputStockHolderPosition.Shares,
                    CostBasis = inputStockHolderPosition.CostBasis
                })
                                        .ToList();
            });

            mockMapper.Setup(mapper => mapper.Map(It.IsAny<DTOs.StockHolderPosition>(), It.IsAny<DAOs.StockHolderPosition>()))
                      .Returns((DTOs.StockHolderPosition dtoStockPosition, DAOs.StockHolderPosition daoStockPosition) =>
                      {
                          daoStockPosition.Shares = dtoStockPosition.Shares;
                          daoStockPosition.CostBasis = dtoStockPosition.CostBasis;

                          return daoStockPosition;
                      });

            return mockMapper.Object;
        }

        //private IStockMarketService GetStockMarketService()
        //{
        //    var mockMarketService = new Mock<IStockMarketService>();
        //    mockMarketService.Setup(marketService => marketService.AddStockPosition(It.IsAny<DAOs.StockHolderPosition>()))
        //                     .ReturnsAsync((DAOs.StockHolderPosition input) =>
        //                                   {
        //                                       input.Id = 10;
        //                                       return input;
        //                                   });

        //    mockMarketService.Setup(marketService => marketService.RemoveStockPosition(It.IsAny<DAOs.StockHolderPosition>()))
        //                     .Callback((DAOs.StockHolderPosition input) =>
        //                       {
        //                           return;
        //                       });

        //    return mockMarketService.Object;

        //}
        #endregion
    }
}
