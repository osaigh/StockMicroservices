using StockMicroservices.API.Models.Daos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockMicroservices.API.Data
{
    public interface IStockDbContext
    {
        Task<List<Stock>> GetStocksAsync();

        Task<Stock?> GetStockByIdAsync(string id);

        Task CreateStockAsync(Stock stock);

        Task UpdateStockAsync(string id, Stock stock);

        Task RemoveStockAsync(string id);

        Task<List<StockHolder>> GetStockHoldersAsync();

        Task<StockHolder?> GetStockHolderByIdAsync(string id);

        Task<StockHolder?> GetStockHolderByUsernameAsync(string username);

        Task CreateStockHolderAsync(StockHolder stockHolder);

        Task UpdateStockHolderAsync(string id, StockHolder stockHolder);

        Task RemoveStockHolderAsync(string id);
    }
}
