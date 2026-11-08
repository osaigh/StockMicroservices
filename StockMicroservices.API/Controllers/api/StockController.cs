using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StockMicroservices.API.Models;
using StockMicroservices.API.Repository;
using DAOStock = StockMicroservices.API.Models.Daos.Stock;
using DTOStock = StockMicroservices.API.Models.Dtos.Stock;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize(Policy = "StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        private readonly IRepository<DAOStock> _stockRepository;

        public StockController(IRepository<DAOStock> stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStock>>> Get()
        {
            var daoStocks = await _stockRepository.GetAllAsync();
            return Ok(daoStocks.ToDtoList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTOStock), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DTOStock>> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("id must be a valid 24 character MongoDB ObjectId.");
            }

            var daoStock = await _stockRepository.GetAsync(id);
            if (daoStock is null)
            {
                return NotFound($"No stock found with id '{id}'.");
            }

            return Ok(daoStock.ToDto());
        }
    }
}
