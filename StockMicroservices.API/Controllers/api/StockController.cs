using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Utils;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;
using DTOStock = StockMicroservices.API.Models.Dtos.Stock;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region Fields
        private readonly ILogger<StockController> _logger;
        private readonly IRepository<DAOStock> _StockRepository;
        private readonly IMapper _Mapper;
        private readonly ActivitySource activitySource;
        #endregion

        #region Constructor
        public StockController(
            ILogger<StockController> logger,
            Instrumentation instrumentation,
            IRepository<DAOStock> stockRepository,
            IMapper mapper)
        {
            _StockRepository = stockRepository;
            _Mapper = mapper;
            _logger = logger;
            activitySource = instrumentation.ActivitySource;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStock>>> Get()
        {
            _logger.LogInformation("Handling request for GET Stocks");
            using var activity = Activity.Current.Source.StartActivity("Get Stocks from Db");
            var daoStocks = await _StockRepository.GetAllAsync();

            var dtoStocks = _Mapper.Map<List<DTOStock>>(daoStocks);
            return Ok(dtoStocks);
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStock), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStock>> Get(int stockId)
        {
            _logger.LogInformation($"Handling request for GET Stock with id: {stockId}");
            using var activity = Activity.Current.Source.StartActivity("Get Stock from Db");
            var daoStock = await _StockRepository.GetAsync(stockId);

            if (daoStock == null)
            {
                _logger.LogInformation($"No Stock with id: {stockId}");
                return null;
            }

            var dtoStock = _Mapper.Map<DTOStock>(daoStock);

            return Ok(dtoStock);
        }

        #endregion
    }
}