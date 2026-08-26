using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockRequestRepository
    {
        // =====================================================
        // STOCK REQUESTS
        // =====================================================

        Task<List<StockRequest>> GetAllAsync();

        Task<StockRequest?> GetByIdAsync(int id);

        Task<StockRequest?> GetByIdWithItemsAsync(int id);

        Task<bool> RequestNumberExistsAsync(
            string requestNumber,
            int? excludeRequestId = null);

        Task AddAsync(StockRequest request);

        void Update(StockRequest request);

        void Delete(StockRequest request);


        // =====================================================
        // ITEMS
        // =====================================================

        Task<List<StockRequestItem>> GetItemsAsync(int requestId);

        Task<StockRequestItem?> GetItemByIdAsync(
            int requestId,
            int itemId);

        Task<bool> ProductExistsInRequestAsync(
            int requestId,
            int productId,
            int? excludeItemId = null);

        Task AddItemAsync(StockRequestItem item);

        void UpdateItem(StockRequestItem item);

        void DeleteItem(StockRequestItem item);
    }
}