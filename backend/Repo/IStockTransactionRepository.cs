using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockTransactionRepository
    {
        Task<List<StockTransaction>> GetAllAsync(
            int? productId = null,
            int? stockId = null,
            int? warehouseId = null,
            int? locationId = null,
            string? transactionType = null,
            DateTimeOffset? fromDate = null,
            DateTimeOffset? toDate = null,
            Guid? userId = null);

        Task<StockTransaction?> GetByIdAsync(long id);
    }
}