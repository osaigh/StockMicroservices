using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace StockMicroservices.StockMarketUpdater
{
    public class Program
    {
        private static int _retryCount;
        private static string _hostname;
        private static string _username;
        private static string _password;

        static void Main(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ENVIRONMENT")?.ToLower();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json", false);
            if (!string.IsNullOrEmpty(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", false, true);
            }
            IConfiguration config = builder.AddEnvironmentVariables()
                                           .Build();

            _retryCount = int.Parse(config["RabbitMq:RetryCount"]);
            _hostname = config["RabbitMq:Hostname"];
            _username = config["RabbitMq:Username"];
            _password = config["RabbitMq:Password"];

            Console.WriteLine("Hello World!");

            Program program = new Program();
            program.RunStart();
        }


        private void RunStart()
        {
            Thread thread = new Thread(UpdateMarket);
            thread.Start();
        }

        private void UpdateMarket()
        { 
            var factory = new ConnectionFactory(){HostName = _hostname, UserName = _username, Password = _password};
            var policy = RetryPolicy.Handle<SocketException>()
                                    .Or<BrokerUnreachableException>()
                                    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                                                                                                                                                                 {
                                                                                                                                                                     Console.WriteLine("RabbitMQ Client could not connect: Retrying");
                                                                                                                                                                 });
            IConnection connection = null;
            policy.Execute(() =>
                           {
                               connection = factory.CreateConnection();
                           });
            if (connection == null)
            {
                return;
            }
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "stock_exchange", type: ExchangeType.Direct);
                while (true)
                {
                    Thread.Sleep(3000);

                    //send update
                    var message = "update_stocks";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "stock_exchange",
                        routingKey: "stock_updates",
                        basicProperties: null,
                        body: body
                    );

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

        }
    }
}
