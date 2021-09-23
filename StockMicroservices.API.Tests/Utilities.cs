using StockMicroservices.API.Data;
using StockMicroservices.API.Models.Daos;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockMicroservices.API.Tests
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(StockDbContext db)
        {
            //Stocks
            List<Stock> stocks = new List<Stock>();
            var stock1 = new Stock()
            {
                Id = 1,
                Name = "Microsoft",
                Price = 85.0,
                Volume = 1000,

            };

            var stock2 = new Stock()
            {
                Id = 2,
                Name = "Google",
                Price = 135.0,
                Volume = 1300,

            };
            stocks.Add(stock1);
            stocks.Add(stock2);


            //Stock Histories
            List<StockHistory> stockHistories = new List<StockHistory>();
            var stockHistory1 = new StockHistory()
            {
                Id = 1,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2018, 12, 02)),
                Price = 45
            };

            var stockHistory2 = new StockHistory()
            {
                Id = 2,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 65
            };

            var stockHistory3 = new StockHistory()
            {
                Id = 3,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 70
            };

            var stockHistory4 = new StockHistory()
            {
                Id = 4,
                StockId = 2,
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 100
            };

            var stockHistory5 = new StockHistory()
            {
                Id = 5,
                StockId = 2,
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 120
            };

            stockHistories.Add(stockHistory1);
            stockHistories.Add(stockHistory2);
            stockHistories.Add(stockHistory3);
            stockHistories.Add(stockHistory4);
            stockHistories.Add(stockHistory5);

            //Stockholders
            List<StockHolder> stockHolders = new List<StockHolder>();

            var jimmy = new StockHolder()
            {
                Username = "Jimmy",
                Email = "jimmy@tlink.com",
            };
            stockHolders.Add(jimmy);

            var priel = new StockHolder()
            {
                Username = "Priel",
                Email = "priel@tyebank.com",
            };
            stockHolders.Add(priel);

            var steve = new StockHolder()
            {
                Username = "Steve",
                Email = "steve@yahoo.com",
            };

            stockHolders.Add(steve);


            //StockholderPositions
            List<StockHolderPosition> stockHolderPositions = new List<StockHolderPosition>();
            stockHolderPositions.Add(new StockHolderPosition()
            {
                Id = 1,
                StockHolderId = jimmy.Username,
                StockHolder = jimmy,
                CostBasis = 280.99,
                Shares = 10,
                StockId = 1,
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                Id = 2,
                StockHolderId = jimmy.Username,
                StockHolder = jimmy,
                CostBasis = 1900.22,
                Shares = 100,
                StockId = 1,
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                Id = 3,
                StockHolderId = jimmy.Username,
                StockHolder = jimmy,
                CostBasis = 1900.22,
                Shares = 100,
                StockId = 1,
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                Id = 4,
                StockHolderId = priel.Username,
                StockHolder = priel,
                CostBasis = 234,
                Shares = 25,
                StockId = 1,
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                Id = 5,
                StockHolderId = steve.Username,
                StockHolder = steve,
                CostBasis = 523.43,
                Shares = 50,
                StockId = 1,
            });


            db.Stocks.AddRange(stocks);
            db.StockHistories.AddRange(stockHistories);
            db.StockHolders.AddRange(stockHolders);
            db.StockHolderPositions.AddRange(stockHolderPositions);


            db.SaveChanges();

        }

        public static List<Stock> GetTestStocks()
        {
            List<Stock> stocks = new List<Stock>();
            stocks.Add(new Stock()
            {
                Id = 1,
                Name = "Microsoft",
                Volume = 1000,
                Price = 85
            });
            stocks.Add(new Stock()
            {
                Id = 2,
                Name = "Google",
                Volume = 1300,
                Price = 105
            });


            return stocks;
        }

        public static List<StockHistory> GetTestStockHistories()
        {
            List<StockHistory> stockHistories = new List<StockHistory>();
            stockHistories.Add(new StockHistory()
            {
                Id = 1,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2018, 12, 02)),
                Price = 45
            });
            stockHistories.Add(new StockHistory()
            {
                Id = 2,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 25
            });
            stockHistories.Add(new StockHistory()
            {
                Id = 3,
                StockId = 1,
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 37
            });


            return stockHistories;
        }

        public static List<StockHolder> GetTestStockHolders()
        {
            List<StockHolder> stockHolders = new List<StockHolder>();
            stockHolders.Add(new StockHolder()
            {
                Username = "Jimmy",
                Email = "jimmy@tlink.com",
            });
            stockHolders.Add(new StockHolder()
            {
                Username = "Priel",
                Email = "priel@tyebank.com",
            });
            stockHolders.Add(new StockHolder()
            {
                Username = "Steve",
                Email = "steve@yahoo.com",
            });


            return stockHolders;
        }

        public static List<StockHolderPosition> GetTestStockHolderPositions()
        {
            List<StockHolderPosition> stockHolderPositions = new List<StockHolderPosition>();
            stockHolderPositions.Add(new StockHolderPosition()
            {
                StockHolderId = "Jimmy",
                CostBasis = 280.99,
                Shares = 10,
                StockId = 1
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                StockHolderId = "Jimmy",
                CostBasis = 1900.22,
                Shares = 100,
                StockId = 1
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                StockHolderId = "Jimmy",
                CostBasis = 1900.22,
                Shares = 100,
                StockId = 1
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                StockHolderId = "Priel",
                CostBasis = 6990.13,
                Shares = 25,
                StockId = 1
            });
            stockHolderPositions.Add(new StockHolderPosition()
            {
                StockHolderId = "Steve",
                CostBasis = 523.43,
                Shares = 50,
                StockId = 1
            });

            return stockHolderPositions;
        }
        #endregion
    }
}
