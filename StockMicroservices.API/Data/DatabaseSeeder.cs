using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StockMicroservices.API.Models;

namespace StockMicroservices.API.Data
{
    /// <summary>
    /// Waits for MongoDB, then seeds demo data on first start. Runs before the API starts
    /// accepting traffic so that a fresh compose or Kubernetes deployment is usable immediately.
    /// </summary>
    public sealed class DatabaseSeeder : IHostedService
    {
        private const int MaxAttempts = 15;

        private readonly IMongoClient _mongoClient;
        private readonly IStockDbContext _dbContext;
        private readonly DatabaseSetting _settings;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            IMongoClient mongoClient,
            IStockDbContext dbContext,
            IOptions<DatabaseSetting> settings,
            ILogger<DatabaseSeeder> logger)
        {
            _mongoClient = mongoClient;
            _dbContext = dbContext;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                try
                {
                    await _mongoClient
                        .GetDatabase(_settings.Name)
                        .RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1), cancellationToken: cancellationToken);

                    if (_settings.SeedOnStartup)
                    {
                        await SeedData.SeedAsync(_dbContext, _logger, cancellationToken);
                    }

                    return;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (attempt == MaxAttempts)
                    {
                        _logger.LogError(ex, "MongoDB was not reachable after {Attempts} attempts.", MaxAttempts);
                        throw;
                    }

                    var delay = TimeSpan.FromSeconds(Math.Min(attempt * 2, 10));
                    _logger.LogWarning(
                        "MongoDB is not ready (attempt {Attempt}/{Attempts}). Retrying in {Delay}.",
                        attempt,
                        MaxAttempts,
                        delay);

                    await Task.Delay(delay, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
