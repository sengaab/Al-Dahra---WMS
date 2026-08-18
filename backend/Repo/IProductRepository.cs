using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetBySKUAsync(string sku);

        Task<Product?> GetByBarcodeAsync(string barcode);

        Task<Product?> GetByQRValueAsync(string qrValue);

        Task<List<Product>> SearchAsync(string search);

        Task<bool> SKUExistsAsync(
            string sku,
            int? excludeProductId = null);

        Task<bool> BarcodeExistsAsync(
            string barcode,
            int? excludeProductId = null);
        Task<string?> GetLastSKUByPrefixAsync(string prefix);

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);
    }
}