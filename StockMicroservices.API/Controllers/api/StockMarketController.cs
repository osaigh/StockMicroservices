using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMicroservices.API.Services;

namespace StockMicroservices.API.Controllers.api
{
    [Authorize(Policy = "StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockMarketController : ControllerBase
    {
        private readonly IStockMarketService _stockMarketService;

        public StockMarketController(IStockMarketService stockMarketService)
        {
            _stockMarketService = stockMarketService;
        }

        /// <summary>Recalculates prices for every stock. Intended for the demo market simulator.</summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update()
        {
            await _stockMarketService.UpdateStockPrices();
            return NoContent();
        }
    }
}
