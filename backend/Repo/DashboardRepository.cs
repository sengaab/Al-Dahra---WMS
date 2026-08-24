using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataBaseContext _context;

        public DashboardRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // PRODUCTS
        // =====================================================

        public IQueryable<Product> GetProductsQuery()
        {
            return _context.Products
                .AsNoTracking();
        }


        // =====================================================
        // STOCK
        // =====================================================

        public IQueryable<Stock> GetStocksQuery()
        {
            return _context.Stocks
                .AsNoTracking()
                .Where(s => s.IsActive);
        }


        // =====================================================
        // PRODUCT ITEMS
        // =====================================================

        public IQueryable<ProductItem> GetProductItemsQuery()
        {
            return _context.ProductItems
                .AsNoTracking()
                .Where(i => i.IsActive);
        }


        // =====================================================
        // WAREHOUSES
        // =====================================================

        public IQueryable<Warehouse> GetWarehousesQuery()
        {
            return _context.Warehouses
                .AsNoTracking()
                .Where(w => w.IsActive);
        }


        // =====================================================
        // OPERATIONS
        // =====================================================

        public IQueryable<Operations> GetTransactionsQuery()
        {
            return _context.Operations
                .AsNoTracking();
        }


        // =====================================================
        // ORDERS
        // =====================================================

        public IQueryable<Order> GetOrdersQuery()
        {
            return _context.Orders
                .AsNoTracking();
        }
    }
}