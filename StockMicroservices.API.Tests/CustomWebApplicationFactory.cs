using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mongo2Go;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockMicroservices.API.Data;
using StockMicroservices.API.Tests.Persistence;

namespace StockMicroservices.API.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
            //builder.ConfigureServices(services =>
            //{
            //    var descriptor = services.SingleOrDefault(
            //        d => d.ServiceType ==
            //             typeof(IStockDbContext));

            //    services.Remove(descriptor);

            //    //Use the test MongoDbContext
            //    MongoDbTestContext MongoDbTestContext = new MongoDbTestContext("stocks");
            //    services.AddSingleton<IStockDbContext>(MongoDbTestContext);

            //    var sp = services.BuildServiceProvider();

            //    using (var scope = sp.CreateScope())
            //    {
            //        var scopedServices = scope.ServiceProvider;
            //        var db = scopedServices.GetRequiredService<IStockDbContext>();
            //        var logger = scopedServices
            //            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    

            //        try
            //        {
            //            Utilities.InitializeDbForTests(db);
            //        }
            //        catch (Exception ex)
            //        {
            //            logger.LogError(ex, "An error occurred seeding the " +
            //                                "database with test messages. Error: {Message}", ex.Message);
            //        }
            //    }
            //});
        }

    }
}
