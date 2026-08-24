using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        // =====================================================
        // PRODUCTS
        // =====================================================

        IQueryable<Product> GetProductsQuery();


        // =====================================================
        // STOCK
        // =====================================================

        IQueryable<Stock> GetStocksQuery();


        // =====================================================
        // PRODUCT ITEMS
        // =====================================================

        IQueryable<ProductItem> GetProductItemsQuery();


        // =====================================================
        // WAREHOUSES
        // =====================================================

        IQueryable<Warehouse> GetWarehousesQuery();


        // =====================================================
        // OPERATIONS
        // =====================================================

        IQueryable<Operations> GetTransactionsQuery();


        // =====================================================
        // ORDERS
        // =====================================================

        IQueryable<Order> GetOrdersQuery();
    }
}