using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Configuration;

namespace StockMicroservices.API.Services
{
    public class StockUpdateListener: IStockUpdateListener,IDisposable
    {
        #region Fields

        private IConnection connection;
        private IModel channel;
        private IServiceProvider _Services;
        private EventingBasicConsumer _consumer;
        private bool _isListening;
        private int _retryCount = 5;
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;

        #endregion

        #region Constructor

        public StockUpdateListener(IServiceProvider services,IConfiguration configuration)
        {
            _Services = services;
            _retryCount = int.Parse(configuration["RabbitMq:RetryCount"]);
            _hostname = configuration["RabbitMq:Hostname"];
            _username = configuration["RabbitMq:Username"];
            _password = configuration["RabbitMq:Password"];
        }
        #endregion

        #region Methods
        public void StartListener()
        {
            if (_isListening)
            {
                return;
            }
            _isListening = true;
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            var policy = RetryPolicy.Handle<SocketException>()
                                    .Or<BrokerUnreachableException>()
                                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                                                                                                                                {
                                                                                                                                    Debug.WriteLine("RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                                                                                                                                });
            IConnection connection = null;
            policy.Execute(() =>
                           {
                               connection = factory
                                   .CreateConnection();
                           });
            if (connection == null)
            {
                return;
            }
            connection = factory.CreateConnection();
            connection.ConnectionShutdown += Connection_ConnectionShutdown;
            connection.CallbackException += Connection_CallbackException;
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "stock_exchange", type: ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "stock_exchange",
                              routingKey: "stock_updates");

            _consumer = new EventingBasicConsumer(channel);
            _consumer.Received += (model, ea) =>
                                  {
                                      var body = ea.Body.ToArray();
                                      var message = Encoding.UTF8.GetString(body);
                                      Debug.WriteLine(" [x] Received {0}", message);

                                      if (message.ToLower() == "update_stocks")
                                      {
                                          Task.Run(async () =>
                                                   {
                                                       await UpdateStocks();
                                                   });
                                      }
                                  };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: _consumer);


        }

        private async Task UpdateStocks()
        {
            using (var scope = _Services.CreateScope())
            {
                IStockMarketService _StockMarketService = scope.ServiceProvider.GetRequiredService<IStockMarketService>();
                await _StockMarketService.UpdateStockPrices();
            }
        }

        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {

            if (channel != null)
            {
                if (channel.IsOpen)
                {
                    channel.Close();
                }
                channel.Dispose();
            }

            if (connection != null)
            {
                if (connection.IsOpen)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }
    }
}
