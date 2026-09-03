using whm.DTOs.PurchaseOrder;
using whm.Models;

namespace whm.Repositories
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<PurchaseOrderDto>> GetAllAsync(
            string? search = null,
            string? status = null,
            int? supplierId = null,
            int? siteId = null,
            int page = 1,
            int pageSize = 20);

        Task<PurchaseOrderDto?> GetByIdAsync(int id);

        Task<PurchaseOrder?> GetEntityByIdAsync(int id);

        Task<bool> PONumberExistsAsync(
            string poNumber,
            int? excludeId = null);

        Task<IEnumerable<PurchaseOrderItemDto>> GetItemsAsync(
            int purchaseOrderId);

        Task<PurchaseOrderItem?> GetItemEntityByIdAsync(
            int purchaseOrderItemId);

        Task<PurchaseOrderItemDto?> GetItemByIdAsync(
            int purchaseOrderItemId);

        Task<IEnumerable<PurchaseOrderReceiptDto>> GetReceiptsAsync(
            int purchaseOrderId);

        Task<IEnumerable<PurchaseOrderHistoryDto>> GetHistoryAsync(
            int purchaseOrderId);

        Task AddAsync(PurchaseOrder purchaseOrder);

        void Update(PurchaseOrder purchaseOrder);

        void Delete(PurchaseOrder purchaseOrder);

        Task AddItemAsync(PurchaseOrderItem item);

        void UpdateItem(PurchaseOrderItem item);

        void DeleteItem(PurchaseOrderItem item);
    }
}