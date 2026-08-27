using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockTransferRepository
    {
        Task<List<StockTransfer>> GetAllAsync();

        Task<StockTransfer?> GetByIdAsync(int id);

        Task<StockTransfer?> GetByIdForUpdateAsync(int id);

        Task AddAsync(StockTransfer transfer);

        void Update(StockTransfer transfer);

        Task<bool> ExistsAsync(int id);
    }
}