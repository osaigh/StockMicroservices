using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using StockMicroservices.API.Data;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;

namespace StockMicroservices.API.Repository
{
    public class StockRepository : IRepository<DAOStock>
    {
        private readonly IStockDbContext _stockDbContext;

        public StockRepository(IStockDbContext stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task<DAOStock> AddAsync(DAOStock stock)
        {
            await _stockDbContext.CreateStockAsync(stock);
            return stock;
        }

        public async Task DeleteAsync(DAOStock stock)
        {
            await _stockDbContext.RemoveStockAsync(stock.Id);
        }

        public async Task<IEnumerable<DAOStock>> SearchForAsync(Expression<Func<DAOStock, bool>> predicate)
        {
            var stocks = await _stockDbContext.GetStocksAsync();
            return stocks.Where(predicate.Compile());
        }

        public async Task<IEnumerable<DAOStock>> GetAllAsync()
        {
            return await _stockDbContext.GetStocksAsync();
        }

        public async Task<DAOStock?> GetAsync(object id)
        {
            return await _stockDbContext.GetStockByIdAsync(id.ToString()!);
        }

        public async Task<DAOStock> UpdateAsync(DAOStock stock)
        {
            await _stockDbContext.UpdateStockAsync(stock.Id, stock);
            return stock;
        }
    }
}
