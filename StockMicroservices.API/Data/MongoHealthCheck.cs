using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StockMicroservices.API.Models;

namespace StockMicroservices.API.Data
{
    public sealed class MongoHealthCheck : IHealthCheck
    {
        private readonly IMongoClient _mongoClient;
        private readonly DatabaseSetting _settings;

        public MongoHealthCheck(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _mongoClient = mongoClient;
            _settings = settings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _mongoClient
                    .GetDatabase(_settings.Name)
                    .RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1), cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy("MongoDB is reachable.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("MongoDB is not reachable.", ex);
            }
        }
    }
}
