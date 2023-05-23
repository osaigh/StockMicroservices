using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Data;
using DAOStockHolder = StockMicroservices.API.Models.Daos.StockHolder;

namespace StockMicroservices.API.Repository
{
    public class StockHolderRepository : IRepository<DAOStockHolder>
    {
        #region fields
        private readonly IStockDbContext _StockDbContext;
        #endregion

        #region Constructor
        public StockHolderRepository(IStockDbContext stockDbContext)
        {
            _StockDbContext = stockDbContext;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStockHolder> AddAsync(DAOStockHolder stockHolder)
        {
            await _StockDbContext.CreateStockHolderAsync(stockHolder);

            return stockHolder;
        }

        public async Task DeleteAsync(DAOStockHolder stockHolder)
        {
            await _StockDbContext.RemoveStockHolderAsync(stockHolder.Id);
        }

        public async Task<IEnumerable<DAOStockHolder>> SearchForAsync(Expression<Func<DAOStockHolder, bool>> predicate)
        {
            var stockHolders = await _StockDbContext.GetStockHoldersAsync();
            return stockHolders.Where<DAOStockHolder>(predicate.Compile());
        }

        public async Task<IEnumerable<DAOStockHolder>> GetAllAsync()
        {
            return await _StockDbContext.GetStockHoldersAsync();
        }

        public async Task<DAOStockHolder> GetAsync(object id)
        {
            return await _StockDbContext.GetStockHolderByUsernameAsync(id.ToString());
        }

        public async Task<DAOStockHolder> UpdateAsync(DAOStockHolder stockHolder)
        {
            await _StockDbContext.UpdateStockHolderAsync(stockHolder.Id, stockHolder);

            return stockHolder;
        }

        #endregion
    }
}
