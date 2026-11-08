using System;
using System.Threading.Tasks;
using StockMicroservices.API.Data;

namespace StockMicroservices.API.Services
{
    public class StockMarketService : IStockMarketService
    {
        private readonly IStockDbContext _stockDbContext;

        public StockMarketService(IStockDbContext stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task UpdateStockPrices()
        {
            var stocks = await _stockDbContext.GetStocksAsync();

            foreach (var stock in stocks)
            {
                var change = (Random.Shared.NextDouble() * 10d) - 5d;
                var newPrice = Math.Round(stock.Price + change, 2);
                stock.Price = newPrice > 0 ? newPrice : 1;

                await _stockDbContext.UpdateStockAsync(stock.Id, stock);
            }
        }
    }
}
