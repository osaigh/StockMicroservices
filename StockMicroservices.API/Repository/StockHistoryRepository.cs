using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Data;
using DAOStockHistory = StockMicroservices.API.Models.Daos.StockHistory;

namespace StockMicroservices.API.Repository
{
    public class StockHistoryRepository : IRepository<DAOStockHistory>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHistoryRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStockHistory> AddAsync(DAOStockHistory stockHistory)
        {
            //check to see if the Stock exist
            var _stock = await _StockDbContext.Stocks.FirstOrDefaultAsync(s => s.Id == stockHistory.StockId);

            //If the stock does not exist, return null
            if (_stock == null)
            {
                return null;
            }

            //Only add this new stock history if the stock exist
            await _StockDbContext.StockHistories.AddAsync(stockHistory);
            await _StockDbContext.SaveChangesAsync();

            return stockHistory;
        }

        public async Task DeleteAsync(DAOStockHistory stockHistory)
        {
            var _stockHistory = await GetAsync(stockHistory.Id);

            if (_stockHistory != null)
            {
                _StockDbContext.StockHistories.Remove(_stockHistory);
                await _StockDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DAOStockHistory>> SearchForAsync(Expression<Func<DAOStockHistory, bool>> predicate)
        {
            return await _StockDbContext.StockHistories
                                        //.Include(s => s.Stock)
                                        //.ThenInclude(s => s.StockHistories)
                                        .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<DAOStockHistory>> GetAllAsync()
        {
            return await _StockDbContext.StockHistories
                                        //.Include(s => s.Stock)
                                        .ToListAsync();

        }

        public async Task<DAOStockHistory> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _StockDbContext.StockHistories
                                        //.Include(s => s.Stock)
                                        .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<DAOStockHistory> UpdateAsync(DAOStockHistory stockHistory)
        {
            var _stockHistory = await GetAsync(stockHistory.Id);

            if (_stockHistory == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockHistory, _stockHistory))
            {
                _Mapper.Map(stockHistory, _stockHistory);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockHistory;
        }
        #endregion
    }
}
