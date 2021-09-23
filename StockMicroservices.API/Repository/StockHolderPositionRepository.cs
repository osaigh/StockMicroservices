using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Data;
using DAOStockHolderPosition = StockMicroservices.API.Models.Daos.StockHolderPosition;

namespace StockMicroservices.API.Repository
{
    public class StockHolderPositionRepository : IRepository<DAOStockHolderPosition>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHolderPositionRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DAOStockHolderPosition> AddAsync(DAOStockHolderPosition stockHolderPosition)
        {
            if (stockHolderPosition.StockId <= 0
                || string.IsNullOrEmpty(stockHolderPosition.StockHolderId)
                || stockHolderPosition.Shares <= 0)
            {
                string message = stockHolderPosition.StockId <= 0 ? "StockId is invalid" : (stockHolderPosition.Shares <= 0 ? "Shares is invalid" : "StockHolderId is invalid");
                throw new ArgumentException(message);
            }

            //Get the stock if it exist
            var stock = await _StockDbContext.Stocks
                                             .FirstOrDefaultAsync(s => s.Id == stockHolderPosition.StockId);
            if (stock == null)
            {
                return null;
            }

            //Get the Stock Holder
            var stockHolder = await _StockDbContext.StockHolders
                                                   .FirstOrDefaultAsync(s => s.Username.ToLower() == stockHolderPosition.StockHolderId.ToLower());
            if (stockHolder == null)
            {
                return null;
            }

            //check to see if this Stock position already exist
            var _stockHolderPosition = await _StockDbContext.StockHolderPositions
                                                            .FirstOrDefaultAsync(s => s.StockId == stockHolderPosition.StockId && s.StockHolderId.ToLower() == stockHolderPosition.StockHolderId.ToLower());
            if (_stockHolderPosition != null)
            {
                throw new Exception("Position already exist");
            }

            stockHolderPosition.CostBasis = stock.Price * stockHolderPosition.Shares;
            stock.Volume += stockHolderPosition.Shares;
            await _StockDbContext.StockHolderPositions.AddAsync(stockHolderPosition);

            await _StockDbContext.SaveChangesAsync();

            return stockHolderPosition;
        }

        public async Task DeleteAsync(DAOStockHolderPosition stockHolderPosition)
        {
            if (stockHolderPosition.StockId <= 0
                || string.IsNullOrEmpty(stockHolderPosition.StockHolderId)
                || stockHolderPosition.Shares <= 0)
            {
                string message = stockHolderPosition.StockId <= 0 ? "StockId is invalid" : (stockHolderPosition.Shares <= 0 ? "Shares is invalid" : "StockHolderId is invalid");
                throw new ArgumentException(message);
            }

            //check to see if this Stock position already exist
            var _stockHolderPosition = await _StockDbContext.StockHolderPositions
                                                            .FirstOrDefaultAsync(s => s.StockId == stockHolderPosition.StockId && s.StockHolderId.ToLower() == stockHolderPosition.StockHolderId.ToLower());
            if (_stockHolderPosition != null)
            {
                _StockDbContext.StockHolderPositions.Remove(_stockHolderPosition);
            }

            await _StockDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DAOStockHolderPosition>> SearchForAsync(Expression<Func<DAOStockHolderPosition, bool>> predicate)
        {
            return await _StockDbContext.StockHolderPositions
                                        .Include(s => s.StockHolder)
                                        .Include(s => s.Stock)
                                        .ThenInclude(s => s.StockHistories)
                                        .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<DAOStockHolderPosition>> GetAllAsync()
        {
            return await _StockDbContext.StockHolderPositions
                                        .Include(s => s.StockHolder)
                                        //.Include(s => s.Stock)
                                        //.ThenInclude(s => s.StockHistories)
                                        .ToListAsync();

        }

        public async Task<DAOStockHolderPosition> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _StockDbContext.StockHolderPositions
                                        .Include(s => s.StockHolder)
                                        //.Include(s => s.Stock)
                                        //.ThenInclude(s => s.StockHistories)
                                        .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<DAOStockHolderPosition> UpdateAsync(DAOStockHolderPosition stockHolderPosition)
        {
            var _stockHolderPosition = await GetAsync(stockHolderPosition.Id);


            if (_stockHolderPosition == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockHolderPosition, _stockHolderPosition))
            {
                _Mapper.Map(stockHolderPosition, _stockHolderPosition);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockHolderPosition;
        }
        #endregion
    }
}
