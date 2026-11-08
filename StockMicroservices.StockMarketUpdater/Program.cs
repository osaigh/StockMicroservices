using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace StockMicroservices.StockMarketUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    configuration.AddJsonFile(
                        $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        optional: true,
                        reloadOnChange: false);
                    configuration.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<StockUpdaterOptions>(context.Configuration.GetSection("StockUpdater"));

                    var hostName = context.Configuration["RabbitMq:Hostname"] ?? "localhost";
                    var port = context.Configuration.GetValue("RabbitMq:Port", 5672);
                    var username = context.Configuration["RabbitMq:Username"] ?? "user";
                    var password = context.Configuration["RabbitMq:Password"] ?? "password";

                    services.AddMassTransit(bus =>
                    {
                        bus.UsingRabbitMq((_, config) =>
                        {
                            config.Host(hostName, (ushort)port, "/", hostConfig =>
                            {
                                hostConfig.Username(username);
                                hostConfig.Password(password);
                            });
                        });
                    });

                    services.AddHostedService<StockUpdatePublisher>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
