using whm.DTOs.Supplier;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        // =====================================================
        // SUPPLIER
        // =====================================================

        Task<List<SupplierDto>> GetAllAsync();

        Task<SupplierDto?> GetByIdAsync(int id);

        Task<Supplier?> GetEntityByIdAsync(int id);

        Task<SupplierDto?> GetByCodeAsync(string code);


        // =====================================================
        // SUPPLIER PRODUCTS
        // =====================================================

        Task<List<SupplierProductDto>> GetProductsAsync(
            int supplierId);

        Task<SupplierProduct?> GetSupplierProductAsync(
            int supplierId,
            int productId);

        Task AddSupplierProductAsync(
            SupplierProduct supplierProduct);

        void UpdateSupplierProduct(
            SupplierProduct supplierProduct);

        void DeleteSupplierProduct(
            SupplierProduct supplierProduct);


        // =====================================================
        // CREATE / UPDATE / DELETE SUPPLIER
        // =====================================================

        Task AddAsync(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(Supplier supplier);
    }
}