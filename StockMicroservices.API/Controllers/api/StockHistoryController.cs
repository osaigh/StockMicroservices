using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMicroservices.API.Repository;
using DAOStockHistory = StockMicroservices.API.Models.Daos.StockHistory;
using DTOStockHistory = StockMicroservices.API.Models.Dtos.StockHistory;

namespace StockMicroservices.API.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockHistoryController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockHistory> _StockHistoryRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHistoryController(
            IRepository<DAOStockHistory> stockHistoryRepository,
            IMapper mapper)
        {
            _StockHistoryRepository = stockHistoryRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IEnumerable<DTOStockHistory>> Get()
        {
            var daoStockHistories = await _StockHistoryRepository.GetAllAsync();

            var dtoStockHistories = _Mapper.Map<List<DTOStockHistory>>(daoStockHistories);

            return dtoStockHistories;
        }

        [HttpGet("{stockId}")]
        public async Task<IEnumerable<DTOStockHistory>> Get(int stockId)
        {
            if (stockId <= 0)
            {
                return null;
            }

            var allStockHistories = await _StockHistoryRepository.GetAllAsync();

            var daoStockHistories = allStockHistories.Where(s => s.StockId == stockId).ToList();

            StockHistoryComparer stockHistoryComparer = new StockHistoryComparer();
            daoStockHistories.Sort(stockHistoryComparer);
            var dtoStockHistories = _Mapper.Map<List<DTOStockHistory>>(daoStockHistories);

            return dtoStockHistories;
        }

        #endregion
    }

    public class StockHistoryComparer : IComparer<DAOStockHistory>
    {
        public int Compare(DAOStockHistory x, DAOStockHistory y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}