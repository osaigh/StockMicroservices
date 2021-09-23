using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMicroservices.API.Models;
using StockMicroservices.API.Repository;
using DAOStockPrice = StockMicroservices.API.Models.Daos.StockPrice;
using DTOStockPrice = StockMicroservices.API.Models.Dtos.StockPrice;

namespace StockMicroservices.API.Controllers.api
{
    //[Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockPriceController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockPrice> _StockPriceRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockPriceController(
            IRepository<DAOStockPrice> stockPriceRepository,
            IMapper mapper)
        {
            _StockPriceRepository = stockPriceRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IEnumerable<DTOStockPrice>> Get()
        {
            var daoStockPrices = await _StockPriceRepository.GetAllAsync();
            var dtoStockPrices = _Mapper.Map<List<DTOStockPrice>>(daoStockPrices);

            return dtoStockPrices;
        }

        [HttpGet("{id}")]
        public async Task<DTOStockPrice> Get(int id)
        {
            var daoStockPrice = await _StockPriceRepository.GetAsync(id);

            if (daoStockPrice == null)
            {
                return null;
            }

            var dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return dtoStockPrice;
        }

        [HttpPost]
        public async Task<DTOStockPrice> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return null;
            }

            DTOStockPrice dtoStockPrice = JsonConvert.DeserializeObject<DTOStockPrice>(apiRequest.JsonString);
            if (dtoStockPrice == null || dtoStockPrice.StockId <= 0)
            {
                return null;
            }

            var daoStockPrice = _Mapper.Map<DAOStockPrice>(dtoStockPrice);
            daoStockPrice = await _StockPriceRepository.AddAsync(daoStockPrice);

            if (daoStockPrice == null)
            {
                return null;
            }

            dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return dtoStockPrice;
        }

        [HttpPut]
        public async Task<DTOStockPrice> Put([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return null;
            }

            DTOStockPrice dtoStockPrice = JsonConvert.DeserializeObject<DTOStockPrice>(apiRequest.JsonString);
            if (dtoStockPrice == null || dtoStockPrice.Id <= 0
                                               || dtoStockPrice.StockId <= 0)
            {
                return null;
            }

            var daoStockPrice = await _StockPriceRepository.GetAsync(dtoStockPrice.Id);
            if (daoStockPrice == null)
            {
                return null;
            }

            _Mapper.Map(dtoStockPrice, daoStockPrice);

            daoStockPrice = await _StockPriceRepository.UpdateAsync(daoStockPrice);
            if (daoStockPrice == null)
            {
                return null;
            }

            dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return dtoStockPrice;
        }
        #endregion
    }
}