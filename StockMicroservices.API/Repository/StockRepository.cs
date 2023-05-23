using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using StockMicroservices.API.Data;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;

namespace StockMicroservices.API.Repository
{
    public class StockRepository : IRepository<DAOStock>
    {
        #region fields
        private readonly IStockDbContext _StockDbContext;
        #endregion

        #region Constructor
        public StockRepository(IStockDbContext stockDbContext)
        {
            _StockDbContext = stockDbContext;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStock> AddAsync(DAOStock stock)
        {
            await _StockDbContext.CreateStockAsync(stock);

            return stock;
        }

        public async Task DeleteAsync(DAOStock stock)
        {
            await _StockDbContext.RemoveStockAsync(stock.Id);
        }

        public async Task<IEnumerable<DAOStock>> SearchForAsync(Expression<Func<DAOStock, bool>> predicate)
        {
            var stocks = await _StockDbContext.GetStocksAsync();
            return stocks.Where<DAOStock>(predicate.Compile());

        }

        public async Task<IEnumerable<DAOStock>> GetAllAsync()
        {
            return await _StockDbContext.GetStocksAsync();
        }

        public async Task<DAOStock> GetAsync(object id)
        {
            return await _StockDbContext.GetStockByIdAsync(id.ToString());
        }

        public async Task<DAOStock> UpdateAsync(DAOStock stock)
        {
            await _StockDbContext.UpdateStockAsync(stock.Id,stock);

            return stock;

        }

        #endregion
    }
}
