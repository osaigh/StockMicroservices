using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Data
{
    public class StockDbContext : IStockDbContext
    {
        public const string CollectionName = "Stock";

        private readonly IMongoCollection<Stock> _stocksCollection;

        public StockDbContext(IMongoClient mongoClient, IOptions<DatabaseSetting> databaseSettings)
        {
            var settings = databaseSettings.Value;
            var database = mongoClient.GetDatabase(settings.Name);
            _stocksCollection = database.GetCollection<Stock>(CollectionName);
        }

        public async Task<List<Stock>> GetStocksAsync() =>
            await _stocksCollection.Find(_ => true).ToListAsync();

        public async Task<Stock?> GetStockByIdAsync(string id) =>
            await _stocksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Stock?> GetStockByNameAsync(string name) =>
            await _stocksCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

        public async Task CreateStockAsync(Stock stock) =>
            await _stocksCollection.InsertOneAsync(stock);

        public async Task UpdateStockAsync(string id, Stock stock) =>
            await _stocksCollection.ReplaceOneAsync(x => x.Id == id, stock);

        public async Task RemoveStockAsync(string id) =>
            await _stocksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
