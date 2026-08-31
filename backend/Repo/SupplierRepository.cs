using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Supplier;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DataBaseContext _context;

        public SupplierRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // GET /api/suppliers
        // =====================================================

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            return await BuildQuery()
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/suppliers/{id}
        // =====================================================

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.SupplierId == id);
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Supplier?> GetEntityByIdAsync(int id)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x => x.SupplierId == id);
        }


        // =====================================================
        // GET BY CODE
        // =====================================================

        public async Task<SupplierDto?> GetByCodeAsync(
            string code)
        {
            code = code.Trim();

            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.Code == code);
        }


        // =====================================================
        // GET SUPPLIER PRODUCTS
        // GET /api/suppliers/{id}/products
        // =====================================================

        public async Task<List<SupplierProductDto>> GetProductsAsync(
            int supplierId)
        {
            return await _context.SupplierProducts
                .AsNoTracking()
                .Where(x => x.SupplierId == supplierId)
                .Select(x => new SupplierProductDto
                {
                    SupplierProductId =
                        x.SupplierProductId,

                    SupplierId =
                        x.SupplierId,

                    ProductId =
                        x.ProductId,

                    ProductName =
                        x.Product.Name,

                    SKU =
                        x.Product.SKU,

                    SupplierSKU =
                        x.SupplierSKU,

                    UnitPrice =
                        x.UnitPrice,

                    LeadTimeDays =
                        x.LeadTimeDays,

                    IsPreferred =
                        x.IsPreferred
                })
                .ToListAsync();
        }


        // =====================================================
        // GET SUPPLIER PRODUCT ENTITY
        // =====================================================

        public async Task<SupplierProduct?> GetSupplierProductAsync(
            int supplierId,
            int productId)
        {
            return await _context.SupplierProducts
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == supplierId &&
                    x.ProductId == productId);
        }


        // =====================================================
        // ADD SUPPLIER PRODUCT
        // =====================================================

        public async Task AddSupplierProductAsync(
            SupplierProduct supplierProduct)
        {
            await _context.SupplierProducts
                .AddAsync(supplierProduct);
        }


        // =====================================================
        // UPDATE SUPPLIER PRODUCT
        // =====================================================

        public void UpdateSupplierProduct(
            SupplierProduct supplierProduct)
        {
            _context.SupplierProducts
                .Update(supplierProduct);
        }


        // =====================================================
        // DELETE SUPPLIER PRODUCT
        // =====================================================

        public void DeleteSupplierProduct(
            SupplierProduct supplierProduct)
        {
            _context.SupplierProducts
                .Remove(supplierProduct);
        }


        // =====================================================
        // ADD SUPPLIER
        // =====================================================

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers
                .AddAsync(supplier);
        }


        // =====================================================
        // UPDATE SUPPLIER
        // =====================================================

        public void Update(Supplier supplier)
        {
            _context.Suppliers
                .Update(supplier);
        }


        // =====================================================
        // DELETE SUPPLIER
        // =====================================================

        public void Delete(Supplier supplier)
        {
            _context.Suppliers
                .Remove(supplier);
        }


        // =====================================================
        // COMMON QUERY
        // =====================================================

        private IQueryable<SupplierDto> BuildQuery()
        {
            return _context.Suppliers
                .AsNoTracking()
                .Select(x => new SupplierDto
                {
                    SupplierId =
                        x.SupplierId,

                    Code =
                        x.Code,

                    Name =
                        x.Name,

                    ContactName =
                        x.ContactName,

                    Email =
                        x.Email,

                    Phone =
                        x.Phone,

                    Address =
                        x.Address,

                    IsActive =
                        x.IsActive,

                    SupplierStatus =
                        x.SupplierStatus.ToString(),

                    CreatedAt =
                        x.CreatedAt,

                    UpdatedAt =
                        x.UpdatedAt,

                    ProductsCount =
                        x.SupplierProducts.Count()
                });
        }
    }
}