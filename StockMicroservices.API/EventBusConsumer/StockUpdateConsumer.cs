using System;
using System.Threading.Tasks;
using MassTransit;
using StockMicroservices.API.Data;
using StockMicroservices.EventBus.Common.Events;

namespace StockMicroservices.API.EventBusConsumer
{
    public class StockUpdateConsumer : IConsumer<StockUpdated>
    {
        private readonly IStockDbContext _stockDbContext;

        public StockUpdateConsumer(IStockDbContext stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task Consume(ConsumeContext<StockUpdated> context)
        {
            var stock = await _stockDbContext.GetStockByNameAsync(context.Message.Name);
            if (stock is null)
            {
                return;
            }

            var newPrice = Math.Round(stock.Price + context.Message.Change, 2);
            stock.Price = newPrice > 0 ? newPrice : 1;

            await _stockDbContext.UpdateStockAsync(stock.Id, stock);
        }
    }
}
