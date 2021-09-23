using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Data;
using DAOStockPrice = StockMicroservices.API.Models.Daos.StockPrice;

namespace StockMicroservices.API.Repository
{
    public class StockPriceRepository : IRepository<DAOStockPrice>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockPriceRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStockPrice> AddAsync(DAOStockPrice stockPrice)
        {
            var stock = await _StockDbContext.Stocks.FirstOrDefaultAsync(s => s.Id == stockPrice.StockId);

            if (stock == null)
            {
                return null;
            }

            await _StockDbContext.StockPrices.AddAsync(stockPrice);
            await _StockDbContext.SaveChangesAsync();

            return stockPrice;

        }

        public async Task DeleteAsync(DAOStockPrice stockPrice)
        {
            var _stockPrice = await GetAsync(stockPrice.Id);
            if (_stockPrice != null)
            {
                _StockDbContext.StockPrices.Remove(_stockPrice);
                await _StockDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DAOStockPrice>> SearchForAsync(Expression<Func<DAOStockPrice, bool>> predicate)
        {
            return await _StockDbContext.StockPrices
                                        .Include(s => s.Stock)
                                        .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<DAOStockPrice>> GetAllAsync()
        {
            return await _StockDbContext.StockPrices
                                        .Include(s => s.Stock)
                                        .ToListAsync();
        }

        public async Task<DAOStockPrice> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _StockDbContext.StockPrices
                                         .Include(s => s.Stock)
                                         .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<DAOStockPrice> UpdateAsync(DAOStockPrice stockPrice)
        {
            var _stockPrice = await GetAsync(stockPrice.Id);

            if (_stockPrice == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockPrice, _stockPrice))
            {
                _Mapper.Map(stockPrice, _stockPrice);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockPrice;
        }

        #endregion
    }
}
