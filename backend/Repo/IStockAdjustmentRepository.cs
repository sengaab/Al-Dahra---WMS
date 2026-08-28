using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockAdjustmentRepository
    {
        Task<List<StockAdjustment>> GetAllAsync();

        Task<StockAdjustment?> GetByIdAsync(int id);

        Task<StockAdjustment?> GetByIdForUpdateAsync(int id);

        Task AddAsync(StockAdjustment adjustment);

        void Update(StockAdjustment adjustment);

        Task<bool> ExistsAsync(int id);
    }
}