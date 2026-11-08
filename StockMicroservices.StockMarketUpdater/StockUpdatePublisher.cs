using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockMicroservices.EventBus.Common.Events;

namespace StockMicroservices.StockMarketUpdater
{
    public sealed class StockUpdatePublisher : BackgroundService
    {
        private readonly MassTransit.IBus _bus;
        private readonly StockUpdaterOptions _options;
        private readonly ILogger<StockUpdatePublisher> _logger;

        public StockUpdatePublisher(
            MassTransit.IBus bus,
            IOptions<StockUpdaterOptions> options,
            ILogger<StockUpdatePublisher> logger)
        {
            _bus = bus;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_options.StockNames.Length == 0)
            {
                _logger.LogWarning("No stock names are configured; the market updater will not publish anything.");
                return;
            }

            var interval = TimeSpan.FromMilliseconds(Math.Max(_options.IntervalMilliseconds, 250));

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var stockName = _options.StockNames[Random.Shared.Next(_options.StockNames.Length)];
                    var change = Math.Round((Random.Shared.NextDouble() * 10d) - 5d, 2);

                    await _bus.Publish(new StockUpdated
                    {
                        Name = stockName,
                        Change = change,
                        CreationDate = DateTime.UtcNow,
                    }, stoppingToken);

                    _logger.LogInformation("Published price change {Change:+0.00;-0.00;0.00} for {Stock}.", change, stockName);

                    await Task.Delay(interval, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown.
            }
        }
    }
}
