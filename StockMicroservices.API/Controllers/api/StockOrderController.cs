using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMicroservices.API.Repository;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiRequest = StockMicroservices.API.Models.ApiRequest;
using DAOStockOrder = StockMicroservices.API.Models.Daos.StockOrder;
using DTOStockOrder = StockMicroservices.API.Models.Dtos.StockOrder;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockOrderController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockOrder> _StockOrderRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockOrderController(
            IRepository<DAOStockOrder> stockOrderRepository,
            IMapper mapper)
        {
            _StockOrderRepository = stockOrderRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockOrder>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockOrder>>> Get()
        {
            var daoStockOrders = await _StockOrderRepository.GetAllAsync();

            var dtoStockOrders = _Mapper.Map<List<DTOStockOrder>>(daoStockOrders);

            return Ok(dtoStockOrders); ;
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockOrder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockOrder>> Get(string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
            {
                return new BadRequestObjectResult("stockId is null");
            }

            var daoStockOrder = await _StockOrderRepository.GetAsync(stockId);

            if (daoStockOrder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stock with Id ", stockId));
            }

            var dtoStockOrder = _Mapper.Map<DTOStockOrder>(daoStockOrder);

            return dtoStockOrder;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockOrder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockOrder>> Post([FromBody] DTOStockOrder dtoStockOrder)
        {
            //DTOStockOrder dtoStockOrder = JsonConvert.DeserializeObject<DTOStockOrder>(apiRequest.JsonString);

            //Add
            DAOStockOrder daoStockOrder = _Mapper.Map<DAOStockOrder>(dtoStockOrder);
            daoStockOrder.StockId = dtoStockOrder.StockId;
            daoStockOrder = await _StockOrderRepository.AddAsync(daoStockOrder);
            if (daoStockOrder == null)
            {
                return new BadRequestObjectResult(string.Format("Something went wrong when placing the order."));
            }

            dtoStockOrder = _Mapper.Map<DTOStockOrder>(daoStockOrder);

            return Ok(dtoStockOrder);
        }

        #endregion
    }
}
