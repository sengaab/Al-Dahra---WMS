using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetAllAsync();

        Task<Supplier?> GetByIdAsync(int id);

        Task<Supplier?> GetByCodeAsync(string supplierCode);

        Task<List<Supplier>> SearchAsync(string search);

        Task<bool> SupplierCodeExistsAsync(
            string supplierCode,
            int? excludeSupplierId = null);

        Task<bool> SupplierNameExistsAsync(
            string supplierName,
            int? excludeSupplierId = null);

        Task AddAsync(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(Supplier supplier);
    }
}