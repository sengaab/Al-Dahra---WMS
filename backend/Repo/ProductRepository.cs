using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Product;
using whm.DTOs.Stock;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _context;

        public ProductRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL PRODUCTS
        // =====================================================

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await BuildQuery()
                .ToListAsync();
        }


        // =====================================================
        // GET PRODUCT BY ID
        // =====================================================

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.ProductId == id);
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Product?> GetEntityByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == id);
        }


        // =====================================================
        // SEARCH
        // GET /api/products/search?q=
        // =====================================================

        public async Task<List<ProductDto>> SearchAsync(string search)
        {
            search = search.Trim();

            return await BuildQuery()
                .Where(x =>
                    x.Name.Contains(search) ||
                    x.SKU.Contains(search) ||
                    (x.Barcode != null &&
                     x.Barcode.Contains(search)))
                .ToListAsync();
        }


        // =====================================================
        // GET BY BARCODE
        // GET /api/products/barcode/{barcode}
        // =====================================================

        public async Task<ProductDto?> GetByBarcodeAsync(string barcode)
        {
            barcode = barcode.Trim();

            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.Barcode == barcode);
        }


        // =====================================================
        // GET BY SKU
        // GET /api/products/sku/{sku}
        // =====================================================

        public async Task<ProductDto?> GetBySkuAsync(string sku)
        {
            sku = sku.Trim();

            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.SKU == sku);
        }


        // =====================================================
        // COMMON PRODUCT QUERY
        // =====================================================

        private IQueryable<ProductDto> BuildQuery()
        {
            return _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,

                    SKU = p.SKU,

                    Barcode = p.Barcode,

                    QRValue = p.QRValue,

                    Name = p.Name,

                    CategoryId = p.CategoryId,

                    CategoryName = p.Category != null
                        ? p.Category.Name
                        : null,

                    UnitId = p.UnitId,

                    UnitName = p.Unit != null
                        ? p.Unit.Name
                        : null,

                    UnitPrice = p.UnitPrice,

                    MinimumStock = p.MinimumStock,

                    Description = p.Description,

                    ProductStatus = p.ProductStatus.ToString(),

                    IsActive = p.IsActive,

                    CreatedAt = p.CreatedAt,

                    UpdatedAt = p.UpdatedAt,

                    Suppliers = p.SupplierProducts
                        .Select(sp => new ProductSupplierDto
                        {
                            SupplierId = sp.SupplierId,

                            SupplierName = sp.Supplier.Name
                        })
                        .ToList()
                });
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }


        // =====================================================
        // INVENTORY
        // =====================================================

        public async Task<List<StockDto>> GetInventoryAsync(int productId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new StockDto
                {
                    StockId = x.StockId,

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    CategoryName = x.Product.Category != null
                        ? x.Product.Category.Name
                        : string.Empty,

                    SKU = x.Product.SKU,

                    Barcode = x.Product.Barcode,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    StockCode = x.StockCode,

                    BatchNumber = x.BatchNumber,

                    ExpiryDate = x.ExpiryDate,

                    Quantity = x.Quantity,

                    ReservedQuantity = x.ReservedQuantity,

                    AvailableQuantity = x.AvailableQuantity,

                    UnitPrice = x.UnitPrice,

                    MinimumStock = x.Product.MinimumStock,

                    StockStatus = x.stockStatus.ToString(),

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();
        }


        // =====================================================
        // STOCK
        // =====================================================

        public async Task<List<StockDto>> GetStockAsync(int productId)
        {
            return await GetInventoryAsync(productId);
        }


        // =====================================================
        // LOCATIONS
        // =====================================================

        public async Task<List<object>> GetLocationsAsync(int productId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.ProductId == productId &&
                    x.LocationId != null)
                .Select(x => new
                {
                    LocationId = x.LocationId,

                    LocationName = x.Location!.Name,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    Quantity = x.Quantity,

                    AvailableQuantity = x.AvailableQuantity
                })
                .Cast<object>()
                .ToListAsync();
        }


        // =====================================================
        // TRANSACTIONS
        // =====================================================

        public async Task<List<object>> GetTransactionsAsync(int productId)
        {
            return await _context.StockTransactions
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new
                {
                    x.StockId,

                    x.ProductId,

                    x.Quantity,

                    x.CreatedAt
                })
                .Cast<object>()
                .ToListAsync();
        }


        // =====================================================
        // SUPPLIERS
        // =====================================================

        public async Task<List<ProductSupplierDto>> GetSuppliersAsync(
            int productId)
        {
            return await _context.SupplierProducts
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new ProductSupplierDto
                {
                    SupplierId = x.SupplierId,

                    SupplierName = x.Supplier.Name
                })
                .ToListAsync();
        }


        // =====================================================
        // PURCHASE HISTORY
        // =====================================================

        public async Task<List<object>> GetPurchaseHistoryAsync(
            int productId)
        {
            return await _context.PurchaseOrderItems
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new
                {
                    x.PurchaseOrderItemId,

                    x.ProductId,

                    x.OrderedQuantity,

                    x.UnitPrice
                })
                .Cast<object>()
                .ToListAsync();
        }


        // =====================================================
        // STOCK SUMMARY
        // =====================================================

        public async Task<object> GetStockSummaryAsync(int productId)
        {
            var stocks = _context.Stocks
                .AsNoTracking()
                .Where(x => x.ProductId == productId);

            return new
            {
                ProductId = productId,

                TotalStockItems = await stocks.CountAsync(),

                TotalQuantity = await stocks
                    .SumAsync(x => (decimal?)x.Quantity) ?? 0,

                TotalReservedQuantity = await stocks
                    .SumAsync(x => (decimal?)x.ReservedQuantity) ?? 0,

                TotalAvailableQuantity = await stocks
                    .SumAsync(x => (decimal?)x.AvailableQuantity) ?? 0,

                TotalValue = await stocks
                    .SumAsync(x =>
                        (decimal?)(x.Quantity * x.UnitPrice)) ?? 0
            };
        }


        // =====================================================
        // STOCK BY WAREHOUSE
        // =====================================================

        public async Task<List<object>> GetStockByWarehouseAsync(
            int productId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .GroupBy(x => new
                {
                    x.WarehouseId,

                    x.Warehouse.Name
                })
                .Select(g => new
                {
                    WarehouseId = g.Key.WarehouseId,

                    WarehouseName = g.Key.Name,

                    Quantity = g.Sum(x => x.Quantity),

                    ReservedQuantity =
                        g.Sum(x => x.ReservedQuantity),

                    AvailableQuantity =
                        g.Sum(x => x.AvailableQuantity)
                })
                .Cast<object>()
                .ToListAsync();
        }


        // =====================================================
        // STOCK BY LOCATION
        // =====================================================

        public async Task<List<object>> GetStockByLocationAsync(
            int productId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.ProductId == productId &&
                    x.LocationId != null)
                .GroupBy(x => new
                {
                    x.LocationId,

                    x.Location!.Name
                })
                .Select(g => new
                {
                    LocationId = g.Key.LocationId,

                    LocationName = g.Key.Name,

                    Quantity = g.Sum(x => x.Quantity),

                    ReservedQuantity =
                        g.Sum(x => x.ReservedQuantity),

                    AvailableQuantity =
                        g.Sum(x => x.AvailableQuantity)
                })
                .Cast<object>()
                .ToListAsync();
        }
    }
}