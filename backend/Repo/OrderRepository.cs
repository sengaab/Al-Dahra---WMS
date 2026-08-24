using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataBaseContext db;

        public OrderRepository(DataBaseContext db)
        {
            this.db = db;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<Order>> GetAllAsync()
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.CreatedByUser)

                .Include(o => o.ApprovedByUser)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .OrderByDescending(o => o.OrderId)

                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.CreatedByUser)

                .Include(o => o.ApprovedByUser)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .FirstOrDefaultAsync(
                    o => o.OrderId == id);
        }


        // =====================================================
        // GET BY ORDER NUMBER
        // =====================================================

        public async Task<Order?> GetByOrderNumberAsync(
            string orderNumber)
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .FirstOrDefaultAsync(
                    o => o.OrderNumber == orderNumber);
        }


        // =====================================================
        // GET BY SUPPLIER
        // =====================================================

        public async Task<List<Order>> GetBySupplierIdAsync(
            int supplierId)
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .Where(o =>
                    o.SupplierId == supplierId)

                .OrderByDescending(o => o.OrderId)

                .ToListAsync();
        }


        // =====================================================
        // GET BY WAREHOUSE
        // =====================================================

        public async Task<List<Order>> GetByWarehouseIdAsync(
            int warehouseId)
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .Where(o =>
                    o.WarehouseId == warehouseId)

                .OrderByDescending(o => o.OrderId)

                .ToListAsync();
        }


        // =====================================================
        // GET BY STATUS
        // =====================================================

        public async Task<List<Order>> GetByStatusAsync(
            OrderStatus status)
        {
            return await db.Orders
                .AsNoTracking()

                .Include(o => o.Supplier)

                .Include(o => o.Warehouse)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)

                .Where(o =>
                    o.Status == status)

                .OrderByDescending(o => o.OrderId)

                .ToListAsync();
        }
        public async Task<List<Order>> GetbywarehouseId(int warehouseId)
        {
            return await db.Orders
                .AsNoTracking()
                .Include(o => o.Supplier)
                .Include(o => o.Warehouse)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.WarehouseId == warehouseId)
                .OrderByDescending(o => o.OrderId)
                .ToListAsync();
        }

        // =====================================================
        // CHECK ORDER NUMBER
        // =====================================================

        public async Task<bool> OrderNumberExistsAsync(
            string orderNumber,
            int? excludeOrderId = null)
        {
            var query = db.Orders
                .AsNoTracking()
                .Where(o =>
                    o.OrderNumber == orderNumber);

            if (excludeOrderId.HasValue)
            {
                query = query.Where(o =>
                    o.OrderId !=
                    excludeOrderId.Value);
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Order order)
        {
            await db.Orders.AddAsync(order);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Order order)
        {
            db.Orders.Update(order);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Order order)
        {
            db.Orders.Remove(order);
        }
    }
}