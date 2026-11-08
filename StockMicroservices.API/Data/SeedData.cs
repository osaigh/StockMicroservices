using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Data
{
    public static class SeedData
    {
        public static IReadOnlyList<Stock> GetStocks()
        {
            return new List<Stock>
            {
                CreateStock("Microsoft", 89, 2000,
                    (2020, 11, 30, 60), (2020, 12, 31, 62), (2021, 1, 31, 69),
                    (2021, 2, 27, 72), (2021, 3, 31, 80), (2021, 4, 30, 89)),
                CreateStock("Slim Stack", 23, 700,
                    (2020, 11, 30, 20), (2020, 12, 31, 26), (2021, 1, 31, 33),
                    (2021, 2, 27, 29), (2021, 3, 31, 19), (2021, 4, 30, 23)),
                CreateStock("Apple", 120, 2400,
                    (2020, 11, 30, 80), (2020, 12, 31, 85), (2021, 1, 31, 91),
                    (2021, 2, 27, 100), (2021, 3, 31, 110), (2021, 4, 30, 150)),
                CreateStock("Google", 104, 2100,
                    (2020, 11, 30, 70), (2020, 12, 31, 76), (2021, 1, 31, 82),
                    (2021, 2, 27, 93), (2021, 3, 31, 100), (2021, 4, 30, 104)),
                CreateStock("Red Space", 19, 560,
                    (2020, 11, 30, 9), (2020, 12, 31, 13), (2021, 1, 31, 15),
                    (2021, 2, 27, 18), (2021, 3, 31, 23), (2021, 4, 30, 19)),
                CreateStock("Yahoo", 12, 300,
                    (2020, 11, 30, 79), (2020, 12, 31, 76), (2021, 1, 31, 56),
                    (2021, 2, 27, 38), (2021, 3, 31, 26), (2021, 4, 30, 12)),
                CreateStock("Alliance", 15, 690,
                    (2020, 11, 30, 14), (2020, 12, 31, 18), (2021, 1, 31, 20),
                    (2021, 2, 27, 22), (2021, 3, 31, 18), (2021, 4, 30, 15)),
            };
        }

        public static async Task SeedAsync(IStockDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
        {
            var existing = await dbContext.GetStocksAsync();
            if (existing.Count > 0)
            {
                logger.LogInformation("Database already contains {Count} stocks; seeding skipped.", existing.Count);
                return;
            }

            var stocks = GetStocks();
            foreach (var stock in stocks)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await dbContext.CreateStockAsync(stock);
            }

            logger.LogInformation("Seeded {Count} stocks.", stocks.Count);
        }

        private static Stock CreateStock(string name, double price, int volume, params (int Year, int Month, int Day, double Price)[] history)
        {
            var stock = new Stock
            {
                Name = name,
                Price = price,
                Volume = volume,
            };

            foreach (var point in history)
            {
                stock.StockHistories.Add(new StockHistory
                {
                    StockId = stock.Id,
                    Date = new DateTimeOffset(point.Year, point.Month, point.Day, 0, 0, 0, TimeSpan.Zero),
                    Price = point.Price,
                });
            }

            return stock;
        }
    }
}
