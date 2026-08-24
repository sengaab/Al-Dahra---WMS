using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetAllAsync();

        Task<OrderItem?> GetByIdAsync(int id);

        Task<List<OrderItem>> GetByOrderIdAsync(
            int orderId);

        Task<List<OrderItem>> GetByProductIdAsync(
            int productId);

        Task AddAsync(OrderItem orderItem);

        void Update(OrderItem orderItem);

        void Delete(OrderItem orderItem);
    }
}