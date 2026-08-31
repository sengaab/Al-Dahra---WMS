using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Category;
using whm.DTOs.Product;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataBaseContext _context;

        public CategoryRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // GET /api/categories
        // =====================================================

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await BuildQuery()
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/categories/{id}
        // =====================================================

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.CategoryId == id);
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Category?> GetEntityByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == id);
        }


        // =====================================================
        // GET PRODUCTS
        // GET /api/categories/{id}/products
        // =====================================================

        public async Task<List<ProductDto>> GetProductsAsync(
            int categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .Select(x => new ProductDto
                {
                    ProductId = x.ProductId,

                    SKU = x.SKU,

                    Barcode = x.Barcode,

                    QRValue = x.QRValue,

                    Name = x.Name,

                    CategoryId = x.CategoryId,

                    CategoryName = x.Category != null
                        ? x.Category.Name
                        : null,

                    UnitId = x.UnitId,

                    UnitName = x.Unit != null
                        ? x.Unit.Name
                        : null,

                    UnitPrice = x.UnitPrice,

                    MinimumStock = x.MinimumStock,

                    Description = x.Description,

                    ProductStatus = x.ProductStatus.ToString(),

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    Suppliers = x.SupplierProducts
                        .Select(sp => new ProductSupplierDto
                        {
                            SupplierId = sp.SupplierId,

                            SupplierName = sp.Supplier.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }


        // =====================================================
        // COMMON QUERY
        // =====================================================

        private IQueryable<CategoryDto> BuildQuery()
        {
            return _context.Categories
                .AsNoTracking()
                .Select(x => new CategoryDto
                {
                    CategoryId = x.CategoryId,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt
                });
        }
    }
}