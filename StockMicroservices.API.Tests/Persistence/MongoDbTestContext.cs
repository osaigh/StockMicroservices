using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models;
using DAO = StockMicroservices.API.Models.Daos;
using DTO = StockMicroservices.API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMicroservices.API.Tests.Persistence
{
    public class MongoDbTestContext : IStockDbContext
    {
        public MongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public string ConnectionString { get; }

        private readonly MongoDbRunner _mongoRunner;

        public IMongoCollection<DAO.Stock> StockCollection { get; }

        public MongoDbTestContext(string databaseName)
        {
            // initializes the instance
            _mongoRunner = MongoDbRunner.Start();

            // store the connection string with the chosen port number
            ConnectionString = _mongoRunner.ConnectionString;

            // create a client and database for use outside the class
            Client = new MongoClient(ConnectionString);

            Database = Client.GetDatabase(databaseName);

            // initialize the Stock collection
            StockCollection = Database.GetCollection<DAO.Stock>("Stock");
        }

        #region Methods (CRUD)
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

        #endregion

        public void SeedData(List<DAO.Stock> stocks)
        {
            //Add sample stocks
            foreach (var stock in stocks)
            {
                StockCollection.InsertOne(stock);
            }
        }
    }
}
