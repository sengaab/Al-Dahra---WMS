using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DataBaseContext db;

        public SupplierRepository(DataBaseContext db)
        {
            this.db = db;
        }


        // =====================================================
        // GET ALL SUPPLIERS
        // =====================================================

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await db.Suppliers
                .AsNoTracking()
                .Include(s => s.Orders)
                .OrderByDescending(s => s.SupplierId)
                .ToListAsync();
        }


        // =====================================================
        // GET SUPPLIER BY ID
        // =====================================================

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await db.Suppliers
                .AsNoTracking()
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(
                    s => s.SupplierId == id
                );
        }


        // =====================================================
        // GET SUPPLIER BY CODE
        // =====================================================

        public async Task<Supplier?> GetByCodeAsync(
            string supplierCode)
        {
            supplierCode = supplierCode.Trim();

            return await db.Suppliers
                .AsNoTracking()
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(
                    s => s.SupplierCode == supplierCode
                );
        }


        // =====================================================
        // SEARCH SUPPLIERS
        // =====================================================

        public async Task<List<Supplier>> SearchAsync(
            string search)
        {
            search = search.Trim();

            return await db.Suppliers
                .AsNoTracking()
                .Where(s =>
                    s.SupplierCode.Contains(search) ||
                    s.SupplierName.Contains(search) ||
                    (s.ContactPerson != null &&
                     s.ContactPerson.Contains(search)) ||
                    (s.Phone != null &&
                     s.Phone.Contains(search)) ||
                    (s.Email != null &&
                     s.Email.Contains(search))
                )
                .OrderBy(s => s.SupplierName)
                .ToListAsync();
        }


        // =====================================================
        // CHECK SUPPLIER CODE
        // =====================================================

        public async Task<bool> SupplierCodeExistsAsync(
            string supplierCode,
            int? excludeSupplierId = null)
        {
            supplierCode = supplierCode.Trim();

            var query = db.Suppliers
                .AsNoTracking()
                .Where(s =>
                    s.SupplierCode == supplierCode);

            if (excludeSupplierId.HasValue)
            {
                query = query.Where(
                    s => s.SupplierId !=
                         excludeSupplierId.Value
                );
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // CHECK SUPPLIER NAME
        // =====================================================

        public async Task<bool> SupplierNameExistsAsync(
            string supplierName,
            int? excludeSupplierId = null)
        {
            supplierName = supplierName.Trim();

            var query = db.Suppliers
                .AsNoTracking()
                .Where(s =>
                    s.SupplierName == supplierName);

            if (excludeSupplierId.HasValue)
            {
                query = query.Where(
                    s => s.SupplierId !=
                         excludeSupplierId.Value
                );
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Supplier supplier)
        {
            await db.Suppliers.AddAsync(supplier);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Supplier supplier)
        {
            db.Suppliers.Update(supplier);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Supplier supplier)
        {
            db.Suppliers.Remove(supplier);
        }
    }
}