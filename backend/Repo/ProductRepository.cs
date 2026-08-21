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
        public async Task<string?> GetLastSKUByPrefixAsync(string prefix)
        {
            return await db.Products
                .Where(p => p.SKU.StartsWith(prefix))
                .OrderByDescending(p => p.SKU)
                .Select(p => p.SKU)
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<Product>> GetAllAsync()
        {
            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .FirstOrDefaultAsync(
                    p => p.ProductId == id);
        }


        // =====================================================
        // GET BY SKU
        // =====================================================

        public async Task<Product?> GetBySKUAsync(
            string sku)
        {
            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .FirstOrDefaultAsync(
                    p => p.SKU == sku);
        }


        // =====================================================
        // GET BY BARCODE
        // =====================================================

        public async Task<Product?> GetByBarcodeAsync(
            string barcode)
        {
            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .FirstOrDefaultAsync(
                    p => p.Barcode == barcode);
        }


        // =====================================================
        // GET BY QR
        // =====================================================

        public async Task<Product?> GetByQRValueAsync(
            string qrValue)
        {
            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .FirstOrDefaultAsync(
                    p => p.QRValue == qrValue);
        }


        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<List<Product>> SearchAsync(
            string search)
        {
            search = search.Trim();

            return await db.Products
                .Include(p => p.Category)
                .Include(p => p.Units)
                .Where(p =>
                    p.ProductName.Contains(search) ||
                    p.SKU.Contains(search) ||
                    (p.Barcode != null &&
                     p.Barcode.Contains(search)))
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }


        // =====================================================
        // CHECK SKU
        // =====================================================

        public async Task<bool> SKUExistsAsync(
            string sku,
            int? excludeProductId = null)
        {
            var query = db.Products
                .Where(p => p.SKU == sku);

            if (excludeProductId.HasValue)
            {
                query = query.Where(
                    p => p.ProductId != excludeProductId.Value);
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // CHECK BARCODE
        // =====================================================

        public async Task<bool> BarcodeExistsAsync(
            string barcode,
            int? excludeProductId = null)
        {
            var query = db.Products
                .Where(p => p.Barcode == barcode);

            if (excludeProductId.HasValue)
            {
                query = query.Where(
                    p => p.ProductId != excludeProductId.Value);
            }

            return await query.AnyAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Product product)
        {
            await db.Products.AddAsync(product);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Product product)
        {
            db.Products.Update(product);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Product product)
        {
            db.Products.Remove(product);
        }
        public async Task<List<Product>>
    SearchBySiteAndDepartmentAsync(
        int? siteId,
        int? departmentId)
        {
            var query = db.Products
                .AsNoTracking()
                .AsQueryable();


            // =====================================================
            // FILTER BY SITE
            // Product
            // → Stock
            // → Bin
            // → Shelf
            // → Row
            // → Room
            // → Warehouse
            // → Site
            // =====================================================

            if (siteId.HasValue)
            {
                query = query.Where(product =>
                    product.Stock.Any(stock =>
                        stock.Bin != null &&
                        stock.Bin.Shelf != null &&
                        stock.Bin.Shelf.Row != null &&
                        stock.Bin.Shelf.Row.Room != null &&
                        stock.Bin.Shelf.Row.Room.Warehouse != null &&
                        stock.Bin.Shelf.Row.Room.Warehouse.Site_Id
                            == siteId.Value
                    )
                );
            }


            // =====================================================
            // FILTER BY DEPARTMENT
            //
            // Product → Category → Department
            // =====================================================

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
                .Where(p => !string.IsNullOrWhiteSpace(p.SKU))
                .Select(p => p.SKU)
                .Distinct()
                .OrderBy(sku => sku)
                .ToListAsync();
        }
    }
}