using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<Order?> GetByOrderNumberAsync(
            string orderNumber);

        Task<List<Order>> GetBySupplierIdAsync(
            int supplierId);

        Task<List<Order>> GetByWarehouseIdAsync(
            int warehouseId);

        Task<List<Order>> GetByStatusAsync(
            OrderStatus status);
        Task<List<Order>> GetbywarehouseId(int warehouseId);

        Task<bool> OrderNumberExistsAsync(
            string orderNumber,
            int? excludeOrderId = null);

        Task AddAsync(Order order);

        void Update(Order order);

        void Delete(Order order);

    }
}