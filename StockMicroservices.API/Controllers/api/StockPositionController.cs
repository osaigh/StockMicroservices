using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAOStockPosition = StockMicroservices.API.Models.Daos.StockPosition;
using DTOStockPosition = StockMicroservices.API.Models.Dtos.StockPosition;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockPositionController : ControllerBase
    {
        #region Fields
        private readonly IRepository<DAOStockPosition> _StockPositionRepository;
        private readonly IMapper _Mapper;
        private readonly ILogger<StockPositionController> _logger;
        private readonly ActivitySource activitySource;
        #endregion

        #region Constructor
        public StockPositionController(
            ILogger<StockPositionController> logger,
            Instrumentation instrumentation,
            IRepository<DAOStockPosition> stockPositionRepository,
            IMapper mapper)
        {
            _StockPositionRepository = stockPositionRepository;
            _Mapper = mapper;
            _logger = logger;
            activitySource = instrumentation.ActivitySource;
        }
        #endregion

        #region Methods
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<DTOStockPosition>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockPosition>>> Get()
        {
            _logger.LogInformation("Handling request for GET StockPosition");
            var dtoStockPositions = new List<DTOStockPosition>();
            using (var activity = activitySource.StartActivity("Getting StockPositions from Db"))
            {
                var daoStockPositions = await _StockPositionRepository.GetAllAsync();

                dtoStockPositions = _Mapper.Map<List<DTOStockPosition>>(daoStockPositions.ToList());
            }

            return Ok(dtoStockPositions);
        }


        #endregion
    }
}
