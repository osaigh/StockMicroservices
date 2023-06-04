using MassTransit;
using StockMicroservices.API.Data;
using StockMicroservices.EventBus.Common.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StockMicroservices.API.EventBusConsumer
{
    public class StockUpdateConsumer : IConsumer<StockUpdated>
    {
        #region Constructor
        private readonly IStockDbContext _StockDbContext;
        #endregion

        #region Constructor
        public StockUpdateConsumer(IStockDbContext stockDbContext)
        {
            _StockDbContext = stockDbContext;
         
        }
        #endregion

        #region IConsumer
        public async Task Consume(ConsumeContext<StockUpdated> context)
        {
            var stock = await _StockDbContext.GetStockByNameAsync(context.Message.Name);

            if (stock != null)
            {
                double newPrice = stock.Price + context.Message.Change;
                stock.Price = newPrice > 0 ? newPrice : 1;
                await _StockDbContext.UpdateStockAsync(stock.Id, stock);
            }
        }
        #endregion
    }
}
