using MongoDB.Bson;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models.Daos;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockMicroservices.API.Tests
{
    public static class Utilities
    {
        #region Code
        public static void InitializeDbForTests(IStockDbContext db)
        {
            //Stocks
            List<Stock> stocks = new List<Stock>();
            var stock1 = new Stock()
            {
                Id = (new ObjectId("e656bb6f9a358bcc3ae63c61")).ToString(),
                Name = "Microsoft",
                Price = 85.0,
                Volume = 1000,
                StockHistories = new List<StockHistory>()
            };

            var stock2 = new Stock()
            {
                Id = (new ObjectId("057340ee83ddedcbeef5b1ab")).ToString(),
                Name = "Google",
                Price = 135.0,
                Volume = 1300,
                StockHistories = new List<StockHistory>()
            };
            stocks.Add(stock1);
            stocks.Add(stock2);


            //Stock Histories
            List<StockHistory> stockHistories = new List<StockHistory>();
            var stockHistory1 = new StockHistory()
            {
                StockId = stock1.Id,
                Date = new DateTimeOffset(new DateTime(2018, 12, 02)),
                Price = 45
            };
            stock1.StockHistories.Add(stockHistory1);

            var stockHistory2 = new StockHistory()
            {
                StockId = stock2.Id,
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 65
            };
            stock2.StockHistories.Add(stockHistory2);

            var stockHistory3 = new StockHistory()
            {
                StockId = stock1.Id,
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 70
            };
            stock1.StockHistories.Add(stockHistory3);

            var stockHistory4 = new StockHistory()
            {
                StockId = stock2.Id,
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 100
            };
            stock2.StockHistories.Add(stockHistory4);

            var stockHistory5 = new StockHistory()
            {
                StockId = stock2.Id,
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 120
            };
            stock2.StockHistories.Add(stockHistory5);

            db.CreateStockAsync(stock1);
            db.CreateStockAsync(stock2);

        }

        public static List<Stock> GetTestStocks()
        {
            List<Stock> stocks = new List<Stock>();
            stocks.Add(new Stock()
            {
                Id = (new ObjectId("e656bb6f9a358bcc3ae63c61")).ToString(),
                Name = "Microsoft",
                Volume = 1000,
                Price = 85
            });
            stocks.Add(new Stock()
            {
                Id = (new ObjectId("057340ee83ddedcbeef5b1ab")).ToString(),
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
                StockId = "e656bb6f9a358bcc3ae63c61",
                Date = new DateTimeOffset(new DateTime(2018, 12, 02)),
                Price = 45
            });
            stockHistories.Add(new StockHistory()
            {
                StockId = "e656bb6f9a358bcc3ae63c61",
                Date = new DateTimeOffset(new DateTime(2019, 12, 02)),
                Price = 25
            });
            stockHistories.Add(new StockHistory()
            {
                StockId = "e656bb6f9a358bcc3ae63c61",
                Date = new DateTimeOffset(new DateTime(2020, 12, 02)),
                Price = 37
            });


            return stockHistories;
        }

        #endregion
    }
}
