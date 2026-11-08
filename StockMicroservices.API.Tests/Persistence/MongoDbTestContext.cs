using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mongo2Go;
using MongoDB.Driver;
using StockMicroservices.API.Data;
using DAO = StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Tests.Persistence
{
    public sealed class MongoDbTestContext : IStockDbContext, IDisposable
    {
        private readonly MongoDbRunner _mongoRunner;

        public MongoDbTestContext(string databaseName)
        {
            _mongoRunner = MongoDbRunner.Start();
            ConnectionString = _mongoRunner.ConnectionString;

            var client = new MongoClient(ConnectionString);
            Database = client.GetDatabase(databaseName);
            StockCollection = Database.GetCollection<DAO.Stock>("Stock");
        }

        public IMongoDatabase Database { get; }

        public string ConnectionString { get; }

        public IMongoCollection<DAO.Stock> StockCollection { get; }

        public async Task<List<DAO.Stock>> GetStocksAsync() =>
            await StockCollection.Find(_ => true).ToListAsync();

        public async Task<DAO.Stock?> GetStockByIdAsync(string id) =>
            await StockCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<DAO.Stock?> GetStockByNameAsync(string name) =>
            await StockCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

        public async Task CreateStockAsync(DAO.Stock stock) =>
            await StockCollection.InsertOneAsync(stock);

        public async Task UpdateStockAsync(string id, DAO.Stock stock) =>
            await StockCollection.ReplaceOneAsync(x => x.Id == id, stock);

        public async Task RemoveStockAsync(string id) =>
            await StockCollection.DeleteOneAsync(x => x.Id == id);

        public void Dispose()
        {
            _mongoRunner.Dispose();
        }
    }
}
