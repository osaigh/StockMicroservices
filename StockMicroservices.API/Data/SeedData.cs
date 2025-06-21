using System;
using System.Collections.Generic;
using System.Linq;
using StockMicroservices.API.Models.Daos;


namespace StockMicroservices.API.Data
{
    public class SeedData
    {
        public static void InitializeDB(StockDbContext stockDbContext)
        {
            AddStocks(stockDbContext);
        }

        private static void AddStocks(StockDbContext stockDbContext)
        {
            //Stocks
            if (!stockDbContext.Stocks.Any())
            {
                var microsoft = new Stock()
                {
                    Name = "Microsoft",
                    Price = 89,
                };

                var slimStack = new Stock()
                {
                    Name = "Slim Stack",
                    Price = 23,
                };

                var apple = new Stock()
                {
                    Name = "Apple",
                    Price = 120,
                };

                var google = new Stock()
                {
                    Name = "Google",
                    Price = 104,
                };

                var redSpace = new Stock()
                {
                    Name = "Red Space",
                    Price = 19,
                };

                var yahoo = new Stock()
                {
                    Name = "Yahoo",
                    Price = 12,
                };

                var alliance = new Stock()
                {
                    Name = "Alliance",
                    Price = 15,
                };

                Dictionary<DateTimeOffset, double> stockHistoryValues = new Dictionary<DateTimeOffset, double>();

                stockDbContext.Stocks.Add(microsoft);
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 60);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 62);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 69);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 72);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 80);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 89);
                AddStockHistory(stockDbContext, microsoft, stockHistoryValues);
                AddStockPosition(stockDbContext, microsoft, 200, 17800, 1);

                stockDbContext.Stocks.Add(slimStack);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 20);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 26);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 33);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 29);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 19);
                stockHistoryValues.Add(new DateTimeOffset(2024, 4, 30, 0, 0, 0, TimeSpan.Zero), 23);
                AddStockHistory(stockDbContext, slimStack, stockHistoryValues);
                AddStockPosition(stockDbContext, slimStack, 10, 230, 2);

                stockDbContext.Stocks.Add(apple);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 80);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 85);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 91);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 100);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 110);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 120);
                AddStockHistory(stockDbContext, apple, stockHistoryValues);
                AddStockPosition(stockDbContext, apple, 100, 12000, 3);

                stockDbContext.Stocks.Add(google);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 70);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 82);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 93);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 100);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 104);
                AddStockHistory(stockDbContext, google, stockHistoryValues);
                AddStockPosition(stockDbContext, google, 200, 20000, 4);

                stockDbContext.Stocks.Add(redSpace);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 9);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 13);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 15);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 23);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 19);
                AddStockHistory(stockDbContext, redSpace, stockHistoryValues);
                AddStockPosition(stockDbContext, redSpace, 40, 72, 5);

                stockDbContext.Stocks.Add(yahoo);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 79);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 56);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 38);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 26);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 12);
                AddStockHistory(stockDbContext, yahoo, stockHistoryValues);
                AddStockPosition(stockDbContext, yahoo, 30, 3600, 6);

                stockDbContext.Stocks.Add(alliance);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 14);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 20);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 22);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 15);
                AddStockHistory(stockDbContext, alliance, stockHistoryValues);
                AddStockPosition(stockDbContext, alliance, 500, 10000, 7);

                stockDbContext.SaveChanges();
            }

        }

        private static void AddStockHistory(StockDbContext stockDbContext, Stock stock, Dictionary<DateTimeOffset, double> stockHistoryValues)
        {
            foreach (var stockHistoryValue in stockHistoryValues)
            {
                StockHistory stockHistory = new StockHistory()
                {
                    StockId = stock.Id,
                    Price = stockHistoryValue.Value,
                    Date = stockHistoryValue.Key
                };

                stockDbContext.StockHistories.Add(stockHistory);
            }
            stockDbContext.SaveChanges();
        }

        private static void AddStockPosition(StockDbContext stockDbContext, Stock stock, int shares, double costBasis, int stockPositionId)
        {
            StockPosition stockPosition = new StockPosition()
            {
                Id = stockPositionId,
                StockId = stock.Id,
                Shares = shares,
                CostBasis = costBasis,

            };

            stockDbContext.StockPositions.Add(stockPosition);
            stockDbContext.SaveChanges();
        }
    }
}
