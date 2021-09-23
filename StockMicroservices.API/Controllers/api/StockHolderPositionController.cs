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
using DAOStockHolderPosition = StockMicroservices.API.Models.Daos.StockHolderPosition;
using DTOStockHolderPosition = StockMicroservices.API.Models.Dtos.StockHolderPosition;

namespace StockMicroservices.API.Controllers.api
{
    //[Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockHolderPositionController : ControllerBase
    {
        #region Fields
        private readonly IRepository<DAOStockHolderPosition> _StockHolderPositionRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHolderPositionController(
            IRepository<DAOStockHolderPosition> stockHolderPositionRepository,
            IMapper mapper)
        {
            _StockHolderPositionRepository = stockHolderPositionRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet("{username}")]
        public async Task<IEnumerable<DTOStockHolderPosition>> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new List<DTOStockHolderPosition>();
            }

            var daoStockHolderPositions = await _StockHolderPositionRepository.SearchForAsync(s => s.StockHolderId.ToLower() == username.ToLower());

            var dtoStockHolderPositions = _Mapper.Map<List<DTOStockHolderPosition>>(daoStockHolderPositions.ToList());

            return dtoStockHolderPositions;
        }

        [HttpPost]
        public async Task<DTOStockHolderPosition> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return null;
            }

            DTOStockHolderPosition dtoStockHolderPosition = JsonConvert.DeserializeObject<DTOStockHolderPosition>(apiRequest.JsonString);
            if (dtoStockHolderPosition == null || string.IsNullOrEmpty(dtoStockHolderPosition.StockHolderId)
                                                || dtoStockHolderPosition.StockId <= 0)
            {
                return null;
            }

            var daoStockHolderPosition = _Mapper.Map<DAOStockHolderPosition>(dtoStockHolderPosition);
            daoStockHolderPosition.StockId = dtoStockHolderPosition.StockId;
            daoStockHolderPosition.StockHolderId = dtoStockHolderPosition.StockHolderId;

            daoStockHolderPosition = await _StockHolderPositionRepository.AddAsync(daoStockHolderPosition);

            if (daoStockHolderPosition == null)
            {
                return null;
            }

            dtoStockHolderPosition = _Mapper.Map<DTOStockHolderPosition>(daoStockHolderPosition);

            return dtoStockHolderPosition;
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            var daoStockHolderPosition = await _StockHolderPositionRepository.GetAsync(id);
            if (daoStockHolderPosition == null)
            {
                return;
            }

            await _StockHolderPositionRepository.DeleteAsync(daoStockHolderPosition);
        }
        #endregion
    }
}