using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMicroservices.API.Models.Daos;
using Microsoft.EntityFrameworkCore;

namespace StockMicroservices.API.Data
{
    public class SeedData
    {
        public static void InitializeDB(StockDbContext stockDbContext)
        {
            stockDbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            //Stocks
            if (!stockDbContext.Stocks.Any())
            {
          
                var microsoft = new Stock()
                {
                    Name = "Microsoft",
                    Price = 89,
                    Volume = 2000
                };

                var slimStack = new Stock()
                {
                    Name = "Slim Stack",
                    Price = 23,
                    Volume = 700
                };

                var apple = new Stock()
                {
                    Name = "Apple",
                    Price = 120,
                    Volume = 2400
                };

                var google = new Stock()
                {
                    Name = "Google",
                    Price = 104,
                    Volume = 2100
                };

                var redSpace = new Stock()
                {
                    Name = "Red Space",
                    Price = 19,
                    Volume = 560
                };

                var yahoo = new Stock()
                {
                    Name = "Yahoo",
                    Price = 12,
                    Volume = 300
                };

                var alliance = new Stock()
                {
                    Name = "Alliance",
                    Price = 15,
                    Volume = 690
                };

                stockDbContext.Stocks.Add(microsoft);
                stockDbContext.Stocks.Add(slimStack);
                stockDbContext.Stocks.Add(apple);
                stockDbContext.Stocks.Add(google);
                stockDbContext.Stocks.Add(redSpace);
                stockDbContext.Stocks.Add(yahoo);
                stockDbContext.Stocks.Add(alliance);

                stockDbContext.SaveChanges();

                //Stock History
                //microsoft 
                StockHistory microsoftHistory1 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 60,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory microsoftHistory2 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 62,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory microsoftHistory3 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 69,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory microsoftHistory4 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 72,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory microsoftHistory5 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 80,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory microsoftHistory6 = new StockHistory()
                {
                    StockId = microsoft.Id,
                    Price = 89,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(microsoftHistory1);
                stockDbContext.StockHistories.Add(microsoftHistory2);
                stockDbContext.StockHistories.Add(microsoftHistory3);
                stockDbContext.StockHistories.Add(microsoftHistory4);
                stockDbContext.StockHistories.Add(microsoftHistory5);
                stockDbContext.StockHistories.Add(microsoftHistory6);

                //slimStack 
                StockHistory slimStackHistory1 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 20,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory slimStackHistory2 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 26,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory slimStackHistory3 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 33,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory slimStackHistory4 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 29,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory slimStackHistory5 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 19,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory slimStackHistory6 = new StockHistory()
                {
                    StockId = slimStack.Id,
                    Price = 23,
                    Date = new DateTimeOffset(2024, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(slimStackHistory1);
                stockDbContext.StockHistories.Add(slimStackHistory2);
                stockDbContext.StockHistories.Add(slimStackHistory3);
                stockDbContext.StockHistories.Add(slimStackHistory4);
                stockDbContext.StockHistories.Add(slimStackHistory5);
                stockDbContext.StockHistories.Add(slimStackHistory6);

                //apple 
                StockHistory appleHistory1 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 80,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory appleHistory2 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 85,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory appleHistory3 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 91,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory appleHistory4 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 100,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory appleHistory5 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 110,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory appleHistory6 = new StockHistory()
                {
                    StockId = apple.Id,
                    Price = 120,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(appleHistory1);
                stockDbContext.StockHistories.Add(appleHistory2);
                stockDbContext.StockHistories.Add(appleHistory3);
                stockDbContext.StockHistories.Add(appleHistory4);
                stockDbContext.StockHistories.Add(appleHistory5);
                stockDbContext.StockHistories.Add(appleHistory6);

                //google 
                StockHistory googleHistory1 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 70,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory googleHistory2 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 76,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory googleHistory3 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 82,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory googleHistory4 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 93,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory googleHistory5 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 100,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory googleHistory6 = new StockHistory()
                {
                    StockId = google.Id,
                    Price = 104,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(googleHistory1);
                stockDbContext.StockHistories.Add(googleHistory2);
                stockDbContext.StockHistories.Add(googleHistory3);
                stockDbContext.StockHistories.Add(googleHistory4);
                stockDbContext.StockHistories.Add(googleHistory5);
                stockDbContext.StockHistories.Add(googleHistory6);

                //redSpace 
                StockHistory redSpaceHistory1 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 9,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory redSpaceHistory2 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 13,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory redSpaceHistory3 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 15,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory redSpaceHistory4 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 18,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory redSpaceHistory5 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 23,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory redSpaceHistory6 = new StockHistory()
                {
                    StockId = redSpace.Id,
                    Price = 19,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(redSpaceHistory1);
                stockDbContext.StockHistories.Add(redSpaceHistory2);
                stockDbContext.StockHistories.Add(redSpaceHistory3);
                stockDbContext.StockHistories.Add(redSpaceHistory4);
                stockDbContext.StockHistories.Add(redSpaceHistory5);
                stockDbContext.StockHistories.Add(redSpaceHistory6);

                //yahoo 
                StockHistory yahooHistory1 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 79,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory yahooHistory2 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 76,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory yahooHistory3 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 56,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory yahooHistory4 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 38,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory yahooHistory5 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 26,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory yahooHistory6 = new StockHistory()
                {
                    StockId = yahoo.Id,
                    Price = 12,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(yahooHistory1);
                stockDbContext.StockHistories.Add(yahooHistory2);
                stockDbContext.StockHistories.Add(yahooHistory3);
                stockDbContext.StockHistories.Add(yahooHistory4);
                stockDbContext.StockHistories.Add(yahooHistory5);
                stockDbContext.StockHistories.Add(yahooHistory6);

                //alliance 
                StockHistory allianceHistory1 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 14,
                    Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory allianceHistory2 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 18,
                    Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory allianceHistory3 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 20,
                    Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
                };
                StockHistory allianceHistory4 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 22,
                    Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory allianceHistory5 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 18,
                    Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
                };

                StockHistory allianceHistory6 = new StockHistory()
                {
                    StockId = alliance.Id,
                    Price = 15,
                    Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
                };

                stockDbContext.StockHistories.Add(allianceHistory1);
                stockDbContext.StockHistories.Add(allianceHistory2);
                stockDbContext.StockHistories.Add(allianceHistory3);
                stockDbContext.StockHistories.Add(allianceHistory4);
                stockDbContext.StockHistories.Add(allianceHistory5);
                stockDbContext.StockHistories.Add(allianceHistory6);

                stockDbContext.SaveChanges();
            }
        }
    }
}
