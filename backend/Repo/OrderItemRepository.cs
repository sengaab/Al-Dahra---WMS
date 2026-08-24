using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DataBaseContext db;

        public OrderItemRepository(DataBaseContext db)
        {
            this.db = db;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<OrderItem>> GetAllAsync()
        {
            return await db.OrderItems
                .AsNoTracking()

                .Include(i => i.Order)

                .Include(i => i.Product)

                .OrderByDescending(
                    i => i.OrderItemId)

                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<OrderItem?> GetByIdAsync(
            int id)
        {
            return await db.OrderItems
                .AsNoTracking()

                .Include(i => i.Order)

                .Include(i => i.Product)

                .FirstOrDefaultAsync(
                    i => i.OrderItemId == id);
        }


        // =====================================================
        // GET BY ORDER
        // =====================================================

        public async Task<List<OrderItem>>
            GetByOrderIdAsync(int orderId)
        {
            return await db.OrderItems
                .AsNoTracking()

                .Include(i => i.Product)

                .Where(i =>
                    i.OrderId == orderId)

                .OrderBy(i => i.OrderItemId)

                .ToListAsync();
        }


        // =====================================================
        // GET BY PRODUCT
        // =====================================================

        public async Task<List<OrderItem>>
            GetByProductIdAsync(int productId)
        {
            return await db.OrderItems
                .AsNoTracking()

                .Include(i => i.Order)

                .Include(i => i.Product)

                .Where(i =>
                    i.ProductId == productId)

                .OrderByDescending(
                    i => i.OrderItemId)

                .ToListAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(
            OrderItem orderItem)
        {
            await db.OrderItems.AddAsync(orderItem);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(OrderItem orderItem)
        {
            db.OrderItems.Update(orderItem);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(OrderItem orderItem)
        {
            db.OrderItems.Remove(orderItem);
        }
    }
}