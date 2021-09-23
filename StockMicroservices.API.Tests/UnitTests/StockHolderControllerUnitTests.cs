using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StockHolderControllerUnitTests
    {
        #region Fields

        #endregion

        #region Constructor
        public StockHolderControllerUnitTests()
        {

        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockHolderCollection()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //StockHolder Repo
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Utilities.GetTestStockHolders());

            //Controler
            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolders = await stockHolderController.Get();

            //Assert
            Assert.NotNull(stockHolders);
            Assert.Equal(3, stockHolders.Count());
        }

        [Theory]
        [InlineData("Jimmy")]
        [InlineData("Priel")]
        [InlineData("Steve")]
        public async Task Get_ValidUsername_ReturnsStockHolder(string username)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //StockHolder Repo
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.GetAsync(username)).ReturnsAsync((string uname) =>
            {
                return Utilities.GetTestStockHolders().FirstOrDefault(s => s.Username.ToLower() == uname.ToLower());
            });
            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Get(username);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(username.ToLower(), stockHolder.Username.ToLower());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Get_InvalidUsername_ReturnsNull(string username)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync((string uname) =>
            {
                return Utilities.GetTestStockHolders().FirstOrDefault(s => s.Username.ToLower() == uname.ToLower());
            });
            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Get(username);

            //Assert
            Assert.Null(stockHolder);
        }

        [Fact]
        public async Task Put_ValidStockHolder_ReturnsStockHolder()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            var stockHoldersCollection = Utilities.GetTestStockHolders();
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
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();

            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                    .ReturnsAsync((object id) =>
                    {
                        DAOs.StockHolder _stockHolder = stockHoldersCollection.FirstOrDefault(
                            s => s.Username.ToLower() == id.ToString().ToLower());
                        return _stockHolder;
                    }
                    );

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<DAOs.StockHolder>())).ReturnsAsync((DAOs.StockHolder updatedStockHolder) =>
            {
                DAOs.StockHolder _stockHolder = stockHoldersCollection.FirstOrDefault(
                    s => s.Username.ToLower() == updatedStockHolder.Username.ToLower());
                if (_stockHolder != null)
                {
                    _stockHolder.Email = updatedStockHolder.Email;
                }
                return _stockHolder;
            });

            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Put(apiRequest);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(jimmyStockHolder.Username, stockHolder.Username);
            Assert.Equal(jimmyStockHolder.Email, stockHolder.Email);
        }

        [Fact]
        public async Task Put_InvalidStockHolder_ReturnsNull()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            var stockHoldersCollection = Utilities.GetTestStockHolders();
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
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<DAOs.StockHolder>())).ReturnsAsync((DAOs.StockHolder updatedStockHolder) =>
            {
                DAOs.StockHolder _stockHolder = stockHoldersCollection.FirstOrDefault(
                    s => s.Username.ToLower() == updatedStockHolder.Username.ToLower());
                if (_stockHolder != null)
                {
                    _stockHolder.Email = updatedStockHolder.Email;
                }
                return _stockHolder;
            });
            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Put(apiRequest);

            //Assert
            Assert.Null(stockHolder);
        }

        [Fact]
        public async Task Post_ValidStockHolder_ReturnsStockHolder()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            var stockHoldersCollection = Utilities.GetTestStockHolders();
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
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.StockHolder>())).ReturnsAsync((DAOs.StockHolder newStockHolder) =>
            {
                stockHoldersCollection.Add((StockHolder) (DAOs.StockHolder)newStockHolder);
                return newStockHolder;
            });
            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Post(apiRequest);

            //Assert
            Assert.NotNull(stockHolder);
            Assert.Equal(abrahamStockHolder.Username, stockHolder.Username);
            Assert.Equal(4, stockHoldersCollection.Count);
        }

        [Fact]
        public async Task Post_InvalidStockHolder_ReturnsNull()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            var stockHoldersCollection = Utilities.GetTestStockHolders();
            var abrahamStockHolder = new DTOs.StockHolder()
            {
                Username = string.Empty,
                Email = "abraham@zend.com",
            };

            var apiRequest = new ApiRequest()
            {
                RequestId = Guid.NewGuid().ToString(),
                JsonString = JsonConvert.SerializeObject(abrahamStockHolder)
            };
            var mockRepo = new Mock<IRepository<DAOs.StockHolder>>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<DAOs.StockHolder>())).ReturnsAsync((DAOs.StockHolder updatedStockHolder) =>
            {
                DAOs.StockHolder _stockHolder = stockHoldersCollection.FirstOrDefault(
                    s => s.Username.ToLower() == updatedStockHolder.Username.ToLower());
                if (_stockHolder != null)
                {
                    _stockHolder.Email = updatedStockHolder.Email;
                }
                return _stockHolder;
            });

            var stockHolderController = new StockHolderController(mockRepo.Object, mapper);

            //Act
            var stockHolder = await stockHolderController.Post(apiRequest);

            //Assert
            Assert.Null(stockHolder);
        }

        #endregion

        #region Helpers
        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.StockHolder>(It.IsAny<DAOs.StockHolder>())).Returns((DAOs.StockHolder inputStockHolder) =>
            {
                var dtoStockHolder = new DTOs.StockHolder
                {
                    Username = inputStockHolder.Username,
                    Email = inputStockHolder.Email,
                };

                return dtoStockHolder;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.StockHolder>>(It.IsAny<List<DAOs.StockHolder>>())).Returns((List<DAOs.StockHolder> inputStockHolders) =>
            {
                return inputStockHolders.Select(inputStockHolder => new DTOs.StockHolder()
                {
                    Username = inputStockHolder.Username,
                    Email = inputStockHolder.Email,
                })
                                        .ToList();
            });

            mockMapper.Setup(mapper => mapper.Map<DAOs.StockHolder>(It.IsAny<DTOs.StockHolder>())).Returns((DTOs.StockHolder inputStockHolder) =>
            {
                var dtoStockHolder = new DAOs.StockHolder
                {
                    Username = inputStockHolder.Username,
                    Email = inputStockHolder.Email,
                };

                return dtoStockHolder;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DAOs.StockHolder>>(It.IsAny<List<DTOs.StockHolder>>())).Returns((List<DTOs.StockHolder> inputStockHolders) =>
            {
                return inputStockHolders.Select(inputStockHolder => new DAOs.StockHolder()
                {
                    Username = inputStockHolder.Username,
                    Email = inputStockHolder.Email,
                })
                                        .ToList();
            });

            mockMapper.Setup(mapper => mapper.Map(It.IsAny<DTOs.StockHolder>(), It.IsAny<DAOs.StockHolder>()))
                      .Returns((DTOs.StockHolder dtoStockHolder, DAOs.StockHolder daoStockHolder) =>
                      {
                          daoStockHolder.Email = dtoStockHolder.Email;
                          daoStockHolder.Username = dtoStockHolder.Username;

                          return daoStockHolder;
                      });

            return mockMapper.Object;
        }

        #endregion
    }
}
