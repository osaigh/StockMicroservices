using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMicroservices.API.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAOStockHistory = StockMicroservices.API.Models.Daos.StockHistory;
using DTOStockHistory = StockMicroservices.API.Models.Dtos.StockHistory;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
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
        [ProducesResponseType(typeof(IEnumerable<DTOStockHistory>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockHistory>>> Get()
        {
            var daoStockHistories = await _StockHistoryRepository.GetAllAsync();

            var dtoStockHistories = _Mapper.Map<List<DTOStockHistory>>(daoStockHistories);

            return Ok(dtoStockHistories);
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<DTOStockHistory>), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<DTOStockHistory>>> Get(int stockId)
        {
            var allStockHistories = await _StockHistoryRepository.GetAllAsync();

            var daoStockHistories = allStockHistories.Where(s => s.StockId == stockId).ToList();

            StockHistoryComparer stockHistoryComparer = new StockHistoryComparer();
            daoStockHistories.Sort(stockHistoryComparer);
            var dtoStockHistories = _Mapper.Map<List<DTOStockHistory>>(daoStockHistories);

            return Ok(dtoStockHistories);
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
