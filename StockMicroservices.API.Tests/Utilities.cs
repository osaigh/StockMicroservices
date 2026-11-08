using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Tests
{
    public static class Utilities
    {
        public static async Task InitializeDbForTestsAsync(IStockDbContext db)
        {
            var microsoft = new Stock
            {
                Id = new ObjectId("e656bb6f9a358bcc3ae63c61").ToString(),
                Name = "Microsoft",
                Price = 85.0,
                Volume = 1000,
                StockHistories = new List<StockHistory>
                {
                    new StockHistory
                    {
                        StockId = "e656bb6f9a358bcc3ae63c61",
                        Date = new DateTimeOffset(new DateTime(2018, 12, 2)),
                        Price = 45,
                    },
                    new StockHistory
                    {
                        StockId = "e656bb6f9a358bcc3ae63c61",
                        Date = new DateTimeOffset(new DateTime(2020, 12, 2)),
                        Price = 70,
                    },
                },
            };

            var google = new Stock
            {
                Id = new ObjectId("057340ee83ddedcbeef5b1ab").ToString(),
                Name = "Google",
                Price = 135.0,
                Volume = 1300,
                StockHistories = new List<StockHistory>
                {
                    new StockHistory
                    {
                        StockId = "057340ee83ddedcbeef5b1ab",
                        Date = new DateTimeOffset(new DateTime(2019, 12, 2)),
                        Price = 100,
                    },
                    new StockHistory
                    {
                        StockId = "057340ee83ddedcbeef5b1ab",
                        Date = new DateTimeOffset(new DateTime(2020, 12, 2)),
                        Price = 120,
                    },
                },
            };

            await db.CreateStockAsync(microsoft);
            await db.CreateStockAsync(google);
        }

        public static List<Stock> GetTestStocks()
        {
            return new List<Stock>
            {
                new Stock
                {
                    Id = new ObjectId("e656bb6f9a358bcc3ae63c61").ToString(),
                    Name = "Microsoft",
                    Volume = 1000,
                    Price = 85,
                },
                new Stock
                {
                    Id = new ObjectId("057340ee83ddedcbeef5b1ab").ToString(),
                    Name = "Google",
                    Volume = 1300,
                    Price = 105,
                },
            };
        }

        public static List<StockHistory> GetTestStockHistories()
        {
            return new List<StockHistory>
            {
                new StockHistory
                {
                    StockId = "e656bb6f9a358bcc3ae63c61",
                    Date = new DateTimeOffset(new DateTime(2018, 12, 2)),
                    Price = 45,
                },
                new StockHistory
                {
                    StockId = "e656bb6f9a358bcc3ae63c61",
                    Date = new DateTimeOffset(new DateTime(2019, 12, 2)),
                    Price = 25,
                },
                new StockHistory
                {
                    StockId = "e656bb6f9a358bcc3ae63c61",
                    Date = new DateTimeOffset(new DateTime(2020, 12, 2)),
                    Price = 37,
                },
            };
        }
    }
}
