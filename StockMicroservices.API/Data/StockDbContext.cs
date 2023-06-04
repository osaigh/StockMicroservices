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
        #endregion

        #region Constructor
        public StockDbContext(IOptions<DatabaseSetting> databaseSettings)
        {
            string connectionString = string.Format("mongodb://{0}:{1}@{2}:27017/{3}", databaseSettings.Value.DbUser, databaseSettings.Value.DbPassword, databaseSettings.Value.Hostname, databaseSettings.Value.Name);

            var mongoClient = new MongoClient(connectionString);

            var stockDatabase = mongoClient.GetDatabase(databaseSettings.Value.Name);

            this._stocksCollection = stockDatabase.GetCollection<Stock>("Stock");
        }
        #endregion

        #region Methods (CRUD)
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

        #endregion
    }
}
