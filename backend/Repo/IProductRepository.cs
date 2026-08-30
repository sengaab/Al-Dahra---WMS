using whm.DTOs.Product;
using whm.DTOs.Stock;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IProductRepository
    {
        // =====================================================
        // PRODUCTS
        // =====================================================

        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task<Product?> GetEntityByIdAsync(int id);


        // =====================================================
        // SEARCH
        // =====================================================

        Task<List<ProductDto>> SearchAsync(string search);

        Task<ProductDto?> GetByBarcodeAsync(string barcode);

        Task<ProductDto?> GetBySkuAsync(string sku);


        // =====================================================
        // PRODUCT INVENTORY
        // =====================================================

        Task<List<StockDto>> GetInventoryAsync(int productId);

        Task<List<StockDto>> GetStockAsync(int productId);


        // =====================================================
        // PRODUCT LOCATIONS
        // =====================================================

        Task<List<object>> GetLocationsAsync(int productId);


        // =====================================================
        // PRODUCT TRANSACTIONS
        // =====================================================

        Task<List<object>> GetTransactionsAsync(int productId);


        // =====================================================
        // PRODUCT SUPPLIERS
        // =====================================================

        Task<List<ProductSupplierDto>> GetSuppliersAsync(int productId);


        // =====================================================
        // PURCHASE HISTORY
        // =====================================================

        Task<List<object>> GetPurchaseHistoryAsync(int productId);


        // =====================================================
        // PRODUCT STOCK STATISTICS
        // =====================================================

        Task<object> GetStockSummaryAsync(int productId);

        Task<List<object>> GetStockByWarehouseAsync(int productId);

        Task<List<object>> GetStockByLocationAsync(int productId);


        // =====================================================
        // CRUD
        // =====================================================

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);
    }
}