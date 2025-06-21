using AutoMapper;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models.Daos;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockMicroservices.API.Repository
{
    public class StockPositionRepository : IRepository<StockPosition>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockPositionRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<StockPosition> AddAsync(StockPosition stockPosition)
        {
            if (stockPosition.Shares <= 0)
            {
                string message = stockPosition.Shares <= 0 ? "Shares is invalid" : "invalid request";
                throw new ArgumentException(message);
            }

            //Get the stock if it exist
            var stock = await _StockDbContext.Stocks
                                             .FirstOrDefaultAsync(s => s.Id == stockPosition.StockId);
            if (stock == null)
            {
                return null;
            }


            //check to see if this Stock position already exist
            var _stockPosition = await _StockDbContext.StockPositions
                                                            .FirstOrDefaultAsync(s => s.StockId == stockPosition.StockId);
            if (_stockPosition != null)
            {
                throw new Exception("Position already exist");
            }

            stockPosition.CostBasis = stock.Price * stockPosition.Shares;
            await _StockDbContext.StockPositions.AddAsync(stockPosition);

            await _StockDbContext.SaveChangesAsync();

            return stockPosition;
        }

        public async Task DeleteAsync(StockPosition stockPosition)
        {
            if (stockPosition.Shares <= 0)
            {
                string message = "StockId is invalid";
                throw new ArgumentException(message);
            }

            //check to see if this Stock position already exist
            var _stockPosition = await _StockDbContext.StockPositions
                                                            .FirstOrDefaultAsync(s => s.Id == stockPosition.Id);
            if (_stockPosition != null)
            {
                _StockDbContext.StockPositions.Remove(_stockPosition);
            }

            await _StockDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockPosition>> SearchForAsync(Expression<Func<StockPosition, bool>> predicate)
        {
            var stockPositions = await _StockDbContext.StockPositions.Include(x => x.Stock)
                                        .Where(predicate).ToListAsync();

            foreach (var stockPosition in stockPositions)
            {
                stockPosition.CurrentPrice = stockPosition.Stock.Price;
            }

            return stockPositions;
        }

        public async Task<IEnumerable<StockPosition>> GetAllAsync()
        {
            var stockPositions = await _StockDbContext.StockPositions.Include(sp => sp.Stock)
                                        .ToListAsync();

            foreach (var stockPosition in stockPositions)
            {
                stockPosition.CurrentPrice = stockPosition.Stock.Price;
            }

            return stockPositions;

        }

        public async Task<StockPosition> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            var stockPosition = await _StockDbContext.StockPositions.Include(sp => sp.Stock)
                                        .FirstOrDefaultAsync(s => s.Id == key);

            stockPosition.CurrentPrice = stockPosition.Stock.Price;
            return stockPosition;
        }

        public async Task<StockPosition> UpdateAsync(StockPosition stockPosition)
        {
            var _stockPosition = await GetAsync(stockPosition.Id);


            if (_stockPosition == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockPosition, _stockPosition))
            {
                _Mapper.Map(stockPosition, _stockPosition);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockPosition;
        }
        #endregion
    }
}
