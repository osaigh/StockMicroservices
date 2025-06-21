using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Data;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;

namespace StockMicroservices.API.Repository
{
    public class StockRepository : IRepository<DAOStock>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStock> AddAsync(DAOStock stock)
        {
            await _StockDbContext.Stocks.AddAsync(stock);
            await _StockDbContext.SaveChangesAsync();

            return stock;
        }

        public async Task DeleteAsync(DAOStock stock)
        {
            var _stock = await GetAsync(stock.Id);
            if (_stock != null)
            {
                _StockDbContext.Stocks.Remove(_stock);
                await _StockDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DAOStock>> SearchForAsync(Expression<Func<DAOStock, bool>> predicate)
        {
            return await _StockDbContext.Stocks
                                        //.Include(s => s.StockHistories)
                                        .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<DAOStock>> GetAllAsync()
        {
            return await _StockDbContext.Stocks
                                        //.Include(s => s.StockHistories)
                                        .ToListAsync();
        }

        public async Task<DAOStock> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _StockDbContext.Stocks
                                         //.Include(s => s.StockHistories)
                                         .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<DAOStock> UpdateAsync(DAOStock stock)
        {
            var _stock = await GetAsync(stock.Id);

            if (_stock == null)
            {
                return null;
            }

            if (!ReferenceEquals(stock, _stock))
            {
                _Mapper.Map(stock, _stock);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stock;

        }

        #endregion
    }
}
