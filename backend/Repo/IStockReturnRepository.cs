using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockReturnRepository
    {
        Task<List<StockReturn>> GetAllAsync();

        Task<StockReturn?> GetByIdAsync(int id);

        Task<StockReturn?> GetByIdForUpdateAsync(int id);

        Task AddAsync(StockReturn stockReturn);

        void Update(StockReturn stockReturn);

        Task<bool> ExistsAsync(int id);
    }
}