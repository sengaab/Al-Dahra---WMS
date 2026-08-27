using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockCountRepository
    {
        Task<List<StockCount>> GetAllAsync();

        Task<StockCount?> GetByIdAsync(int id);

        Task<StockCount?> GetByIdForUpdateAsync(int id);

        Task AddAsync(StockCount stockCount);

        void Update(StockCount stockCount);

        Task<bool> ExistsAsync(int id);
    }
}