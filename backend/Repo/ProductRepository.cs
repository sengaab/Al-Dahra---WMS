using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext db;

        public ProductRepository(DataBaseContext db)
        {
            this.db = db;
        }

        // =====================================================
        // GET LAST SKU BY PREFIX
        // =====================================================

        public async Task<string?> GetLastSKUByPrefixAsync(string prefix)
        {
            return await db.Products
                .AsNoTracking()
                .Where(p => p.SKU.StartsWith(prefix))
                .OrderByDescending(p => p.SKU)
                .Select(p => p.SKU)
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ALL PRODUCTS
        // =====================================================

        public async Task<List<Product>> GetAllAsync()
        {
            return await db.Products
                .AsNoTracking()

                // Category
                .Include(p => p.Category)

                // SubCategory
                .Include(p => p.SubCategory)

                .OrderByDescending(p => p.ProductId)

                .ToListAsync();
        }


        // =====================================================
        // GET PRODUCT BY ID
        // =====================================================

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await db.Products
                .AsNoTracking()

                // Category
                .Include(p => p.Category)

                // SubCategory
                .Include(p => p.SubCategory)

                .FirstOrDefaultAsync(
                    p => p.ProductId == id
                );
        }


        // =====================================================
        // GET PRODUCT BY SKU
        // =====================================================

        public async Task<Product?> GetBySKUAsync(string sku)
        {
            return await db.Products
                .AsNoTracking()

                .Include(p => p.Category)

                .Include(p => p.SubCategory)

                .FirstOrDefaultAsync(
                    p => p.SKU == sku
                );
        }


        // =====================================================
        // GET PRODUCT BY BARCODE
        // =====================================================

        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return await db.Products
                .AsNoTracking()

                .Include(p => p.Category)

                .Include(p => p.SubCategory)

                .FirstOrDefaultAsync(
                    p => p.Barcode == barcode
                );
        }


        // =====================================================
        // GET PRODUCT BY QR
        // =====================================================

        public async Task<Product?> GetByQRValueAsync(string qrValue)
        {
            return await db.Products
                .AsNoTracking()

                .Include(p => p.Category)

                .Include(p => p.SubCategory)

                .FirstOrDefaultAsync(
                    p => p.QRValue == qrValue
                );
        }


        // =====================================================
        // SEARCH PRODUCTS
        // =====================================================

        public async Task<List<Product>> SearchAsync(string search)
        {
            search = search.Trim();

            return await db.Products
                .AsNoTracking()

                .Include(p => p.Category)

                .Include(p => p.SubCategory)

                .Where(p =>
                    p.ProductName.Contains(search)

                    || p.SKU.Contains(search)

                    || (
                        p.Barcode != null &&
                        p.Barcode.Contains(search)
                    )

                    || p.QRValue.Contains(search)
                )

                .OrderBy(p => p.ProductName)

                .ToListAsync();
        }


        // =====================================================
        // CHECK SKU EXISTS
        // =====================================================

        public async Task<bool> SKUExistsAsync(
            string sku,
            int? excludeProductId = null)
        {
            var query = db.Products
                .AsNoTracking()
                .Where(p => p.SKU == sku);

            if (excludeProductId.HasValue)
            {
                query = query.Where(
                    p => p.ProductId != excludeProductId.Value
                );
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // CHECK BARCODE EXISTS
        // =====================================================

        public async Task<bool> BarcodeExistsAsync(
            string barcode,
            int? excludeProductId = null)
        {
            var query = db.Products
                .AsNoTracking()
                .Where(p => p.Barcode == barcode);

            if (excludeProductId.HasValue)
            {
                query = query.Where(
                    p => p.ProductId != excludeProductId.Value
                );
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // CHECK QR EXISTS
        // =====================================================

        public async Task<bool> QRValueExistsAsync(
            string qrValue,
            int? excludeProductId = null)
        {
            var query = db.Products
                .AsNoTracking()
                .Where(p => p.QRValue == qrValue);

            if (excludeProductId.HasValue)
            {
                query = query.Where(
                    p => p.ProductId != excludeProductId.Value
                );
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // ADD PRODUCT
        // =====================================================

        public async Task AddAsync(Product product)
        {
            await db.Products.AddAsync(product);
        }


        // =====================================================
        // UPDATE PRODUCT
        // =====================================================

        public void Update(Product product)
        {
            db.Products.Update(product);
        }


        // =====================================================
        // DELETE PRODUCT
        // =====================================================

        public void Delete(Product product)
        {
            db.Products.Remove(product);
        }


        // =====================================================
        // SEARCH BY SITE AND DEPARTMENT
        // =====================================================

        public async Task<List<Product>>
            SearchBySiteAndDepartmentAsync(
                int? siteId,
                int? departmentId)
        {
            var query = db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .AsQueryable();


            // =================================================
            // FILTER BY SITE
            //
            // Product
            //   ↓
            // Stock
            //   ↓
            // Bin
            //   ↓
            // Shelf
            //   ↓
            // Row
            //   ↓
            // Room
            //   ↓
            // Warehouse
            //   ↓
            // Site
            // =================================================

            if (siteId.HasValue)
            {
                query = query.Where(product =>
                    product.Stock.Any(stock =>

                        stock.Bin != null &&

                        stock.Bin.Shelf != null &&

                        stock.Bin.Shelf.Row != null &&

                        stock.Bin.Shelf.Row.Room != null &&

                        stock.Bin.Shelf.Row.Room.Warehouse != null &&

                        stock.Bin.Shelf.Row.Room
                            .Warehouse.Site_Id
                            == siteId.Value
                    )
                );
            }


            // =================================================
            // FILTER BY DEPARTMENT
            //
            // Product
            //   ↓
            // Category
            //   ↓
            // Department
            // =================================================

            if (departmentId.HasValue)
            {
                query = query.Where(product =>
                    product.Category != null &&

                    product.Category.Department_Id
                        == departmentId.Value
                );
            }


            return await query
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }


        // =====================================================
        // GET ALL SKUs
        // =====================================================

        public async Task<List<string>> GetAllSKUsAsync()
        {
            return await db.Products
                .AsNoTracking()

                .Where(p =>
                    !string.IsNullOrWhiteSpace(p.SKU)
                )

                .Select(p => p.SKU)

                .Distinct()

                .OrderBy(sku => sku)

                .ToListAsync();
        }
    }
}