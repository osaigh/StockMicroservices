using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StockMicroservices.API.Models;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Data
{
    public class StockDbContext : IStockDbContext
    {
        #region Properties
        private readonly IMongoCollection<Stock> _stocksCollection;
        private readonly IMongoCollection<StockHolder> _stockHoldersCollection;
        #endregion

        #region Constructor
        public StockDbContext(IOptions<DatabaseSetting> databaseSettings)
        {
            string connectionString = string.Format("mongodb://{0}:{1}@{2}:27017/{3}", databaseSettings.Value.DbUser, databaseSettings.Value.DbPassword, databaseSettings.Value.Hostname, databaseSettings.Value.Name);

            var mongoClient = new MongoClient(connectionString);

            var stockDatabase = mongoClient.GetDatabase(databaseSettings.Value.Name);

            this._stocksCollection = stockDatabase.GetCollection<Stock>("Stock");
            this._stockHoldersCollection = stockDatabase.GetCollection<StockHolder>("StockHolder");
        }
        #endregion

        #region Methods (CRUD)
        public async Task<List<Stock>> GetStocksAsync() =>
            await _stocksCollection.Find(_ => true).ToListAsync();

        public async Task<Stock?> GetStockByIdAsync(string id) =>
            await _stocksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateStockAsync(Stock stock) =>
            await _stocksCollection.InsertOneAsync(stock);

        public async Task UpdateStockAsync(string id, Stock stock) =>
            await _stocksCollection.ReplaceOneAsync(x => x.Id == id, stock);

        public async Task RemoveStockAsync(string id) =>
            await _stocksCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<StockHolder>> GetStockHoldersAsync() =>
            await _stockHoldersCollection.Find(_ => true).ToListAsync();

        public async Task<StockHolder?> GetStockHolderByIdAsync(string id) =>
            await _stockHoldersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<StockHolder?> GetStockHolderByUsernameAsync(string username) =>
            await _stockHoldersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

        public async Task CreateStockHolderAsync(StockHolder stockHolder) =>
            await _stockHoldersCollection.InsertOneAsync(stockHolder);

        public async Task UpdateStockHolderAsync(string id, StockHolder stockHolder) =>
            await _stockHoldersCollection.ReplaceOneAsync(x => x.Id == id, stockHolder);

        public async Task RemoveStockHolderAsync(string id) =>
            await _stockHoldersCollection.DeleteOneAsync(x => x.Id == id);
        #endregion
    }
}
