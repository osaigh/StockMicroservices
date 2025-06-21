using MassTransit;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.API.Repository;
using StockMicroservices.EventBus.Common.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.EventBusConsumer
{
    public class StockUpdateConsumer : IConsumer<StockUpdated>
    {
        #region Constructor
        private readonly IRepository<Stock> _stockRepository;
        #endregion

        #region Constructor
        public StockUpdateConsumer(IRepository<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
         
        }
        #endregion

        #region IConsumer
        public async Task Consume(ConsumeContext<StockUpdated> context)
        {
            var stocks = await _stockRepository.SearchForAsync(s => string.Equals(s.Name, context.Message.Name, StringComparison.InvariantCultureIgnoreCase));

            var stock = stocks.FirstOrDefault();
            if (stock != null)
            {
                double newPrice = stock.Price + context.Message.Change;
                stock.Price = newPrice > 0 ? newPrice : 1;
                await _stockRepository.UpdateAsync(stock);
            }
        }
        #endregion
    }
}
