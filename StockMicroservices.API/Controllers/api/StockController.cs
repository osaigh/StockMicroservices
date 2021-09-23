using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMicroservices.API.Repository;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;
using DTOStock = StockMicroservices.API.Models.Dtos.Stock;

namespace StockMicroservices.API.Controllers.api
{
    //[Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStock> _StockRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockController(
            IRepository<DAOStock> stockRepository,
            IMapper mapper)
        {
            _StockRepository = stockRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IEnumerable<DTOStock>> Get()
        {
            var daoStocks = await _StockRepository.GetAllAsync();

            var dtoStocks = _Mapper.Map<List<DTOStock>>(daoStocks);

            return dtoStocks;
        }

        [HttpGet("{id}")]
        public async Task<DTOStock> Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            var daoStock = await _StockRepository.GetAsync(id);

            if (daoStock == null)
            {
                return null;
            }

            var dtoStock = _Mapper.Map<DTOStock>(daoStock);

            return dtoStock;
        }

        #endregion
    }
}