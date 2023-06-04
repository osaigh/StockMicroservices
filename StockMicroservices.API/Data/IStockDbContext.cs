using StockMicroservices.API.Models.Daos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockMicroservices.API.Data
{
    public interface IStockDbContext
    {
        Task<List<Stock>> GetStocksAsync();

        Task<Stock?> GetStockByIdAsync(string id);

        Task<Stock?> GetStockByNameAsync(string name);

        Task CreateStockAsync(Stock stock);

        Task UpdateStockAsync(string id, Stock stock);

        Task RemoveStockAsync(string id);

    }
}
