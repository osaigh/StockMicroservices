using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockMicroservices.API.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAOStockPosition = StockMicroservices.API.Models.Daos.StockPosition;
using DTOStockPosition = StockMicroservices.API.Models.Dtos.StockPosition;

namespace StockMicroservices.API.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockPositionController : ControllerBase
    {
        #region Fields
        private readonly IRepository<DAOStockPosition> _StockPositionRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockPositionController(
            IRepository<DAOStockPosition> stockPositionRepository,
            IMapper mapper)
        {
            _StockPositionRepository = stockPositionRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<DTOStockPosition>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockPosition>>> Get()
        {
            var daoStockPositions = await _StockPositionRepository.GetAllAsync();

            var dtoStockPositions = _Mapper.Map<List<DTOStockPosition>>(daoStockPositions.ToList());

            return Ok(dtoStockPositions);
        }


        #endregion
    }
}
