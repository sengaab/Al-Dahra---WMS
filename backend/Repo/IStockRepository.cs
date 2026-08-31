using whm.DTOs.Stock;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<List<StockDto>> GetAllAsync();

        Task<StockDto?> GetByIdAsync(int id);

        Task<List<StockDto>> GetByProductAsync(int productId);

        Task<List<StockDto>> GetByLocationAsync(int locationId);

        Task<List<StockDto>> GetByWarehouseAsync(int warehouseId);

        Task<List<StockDto>> GetAvailableAsync();

        Task<List<StockDto>> GetLowStockAsync();

        Task<List<StockDto>> GetOutOfStockAsync();

        Task<StockSummaryDto> GetSummaryAsync();

        Task<decimal> GetTotalQuantityAsync();

        Task<decimal> GetTotalValueAsync();

        Task<Stock?> GetEntityByIdAsync(int id);


        Task AddAsync(Stock stock);

        void Update(Stock stock);

        void Delete(Stock stock);
    }
}