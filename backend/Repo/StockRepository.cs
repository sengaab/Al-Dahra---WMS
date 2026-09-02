using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Stock;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly DataBaseContext _context;

        public StockRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockDto>> GetAllAsync()
        {
            return await BuildQuery()
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockDto?> GetByIdAsync(int id)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.StockId == id);
        }


        // =====================================================
        // GET BY PRODUCT
        // =====================================================

        public async Task<List<StockDto>> GetByProductAsync(int productId)
        {
            return await BuildQuery()
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY LOCATION
        // =====================================================

        public async Task<List<StockDto>> GetByLocationAsync(int locationId)
        {
            return await BuildQuery()
                .Where(x => x.LocationId == locationId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY WAREHOUSE
        // =====================================================

        public async Task<List<StockDto>> GetByWarehouseAsync(int warehouseId)
        {
            return await BuildQuery()
                .Where(x => x.WarehouseId == warehouseId)
                .ToListAsync();
        }


        // =====================================================
        // AVAILABLE STOCK
        // =====================================================

        public async Task<List<StockDto>> GetAvailableAsync()
        {
            return await BuildQuery()
                .Where(x =>
                    x.AvailableQuantity > 0 &&
                    x.StockStatus == StockStatus.Available.ToString())
                .ToListAsync();
        }


        // =====================================================
        // LOW STOCK
        // =====================================================

        public async Task<List<StockDto>> GetLowStockAsync()
        {
            return await BuildQuery()
                .Where(x =>
                    x.AvailableQuantity > 0 &&
                    x.AvailableQuantity <= x.MinimumStock)
                .ToListAsync();
        }


        // =====================================================
        // OUT OF STOCK
        // =====================================================

        public async Task<List<StockDto>> GetOutOfStockAsync()
        {
            return await BuildQuery()
                .Where(x => x.AvailableQuantity <= 0)
                .ToListAsync();
        }


        // =====================================================
        // SUMMARY
        // =====================================================

        public async Task<StockSummaryDto> GetSummaryAsync()
        {
            var stocks = _context.Stocks
                .AsNoTracking();

            return new StockSummaryDto
            {
                // =========================
                // Totals
                // =========================

                TotalStockItems = await stocks.CountAsync(),

                TotalQuantity = await stocks
                    .SumAsync(x => (decimal?)x.Quantity) ?? 0,

                TotalReservedQuantity = await stocks
                    .SumAsync(x => (decimal?)x.ReservedQuantity) ?? 0,

                TotalAvailableQuantity = await stocks
                    .SumAsync(x => (decimal?)x.AvailableQuantity) ?? 0,

                TotalValue = await stocks
                    .SumAsync(x => (decimal?)(x.Quantity * x.UnitPrice)) ?? 0,


                // =========================
                // Status
                // =========================

                AvailableItems = await stocks
                    .CountAsync(x =>
                        x.stockStatus == StockStatus.Available),

                QuarantinedItems = await stocks
                    .CountAsync(x =>
                        x.stockStatus == StockStatus.Quarantined),

                DamagedItems = await stocks
                    .CountAsync(x =>
                        x.stockStatus == StockStatus.Damaged),

                ExpiredItems = await stocks
                    .CountAsync(x =>
                        x.stockStatus == StockStatus.Expired),

                BlockedItems = await stocks
                    .CountAsync(x =>
                        x.stockStatus == StockStatus.Blocked),


                // =========================
                // Stock Levels
                // =========================

                LowStockItems = await stocks
                    .CountAsync(x =>
                        x.AvailableQuantity > 0 &&
                        x.AvailableQuantity <= x.MinimumStock),

                OutOfStockItems = await stocks
                    .CountAsync(x =>
                        x.AvailableQuantity <= 0)
            };
        }


        // =====================================================
        // TOTAL QUANTITY
        // =====================================================

        public async Task<decimal> GetTotalQuantityAsync()
        {
            return await _context.Stocks
                .AsNoTracking()
                .SumAsync(x => (decimal?)x.Quantity) ?? 0;
        }


        // =====================================================
        // TOTAL VALUE
        // =====================================================

        public async Task<decimal> GetTotalValueAsync()
        {
            return await _context.Stocks
                .AsNoTracking()
                .SumAsync(x => (decimal?)(x.Quantity * x.UnitPrice)) ?? 0;
        }


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Stock?> GetEntityByIdAsync(int id)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(x => x.StockId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Stock stock)
        {
            _context.Stocks.Update(stock);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Stock stock)
        {
            _context.Stocks.Remove(stock);
        }


        // =====================================================
        // COMMON QUERY
        // =====================================================

        private IQueryable<StockDto> BuildQuery()
        {
            return _context.Stocks
                .AsNoTracking()
                .Select(x => new StockDto
                {
                    // =================================================
                    // STOCK
                    // =================================================

                    StockId = x.StockId,

                    StockCode = x.StockCode,

                    BatchNumber = x.BatchNumber,

                    ExpiryDate = x.ExpiryDate,

                    Quantity = x.Quantity,

                    ReservedQuantity = x.ReservedQuantity,

                    AvailableQuantity = x.AvailableQuantity,

                    UnitPrice = x.UnitPrice,

                    MinimumStock = x.MinimumStock,

                    StockStatus = x.stockStatus.ToString(),

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,


                    // =================================================
                    // PRODUCT
                    // =================================================

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    SKU = x.Product.SKU,

                    Barcode = x.Product.Barcode,


                    // =================================================
                    // CATEGORY
                    // =================================================

                    CategoryName = _context.Categories
                        .Where(c =>
                            c.CategoryId == x.Product.CategoryId)
                        .Select(c => c.Name)
                        .FirstOrDefault() ?? string.Empty,


                    // =================================================
                    // WAREHOUSE
                    // =================================================

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,


                    // =================================================
                    // LOCATION
                    // =================================================

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,


                    // =================================================
                    // BIN
                    // =================================================

                    BinId = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId)
                        .Select(b => (int?)b.Bin_Id)
                        .FirstOrDefault(),

                    BinName = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId)
                        .Select(b => b.Bin_Name)
                        .FirstOrDefault(),

                    BinCode = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId)
                        .Select(b => b.Bin_Code)
                        .FirstOrDefault(),


                    // =================================================
                    // SHELF
                    // =================================================

                    ShelfId = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf_Id.HasValue)
                        .Select(b => b.Shelf_Id)
                        .FirstOrDefault(),

                    ShelfName = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null)
                        .Select(b => b.Shelf!.Shelf_Name)
                        .FirstOrDefault(),

                    ShelfCode = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null)
                        .Select(b => b.Shelf!.Shelf_Code)
                        .FirstOrDefault(),


                    // =================================================
                    // RACK
                    // =================================================

                    RackId = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row_Id.HasValue)
                        .Select(b => b.Shelf!.Row_Id)
                        .FirstOrDefault(),

                    RackName = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row != null)
                        .Select(b => b.Shelf!.Row!.Rack_Name)
                        .FirstOrDefault(),

                    RackCode = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row != null)
                        .Select(b => b.Shelf!.Row!.Rack_Code)
                        .FirstOrDefault(),


                    // =================================================
                    // ROOM
                    // =================================================

                    RoomId = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row != null &&
                            b.Shelf.Row.Room_Id.HasValue)
                        .Select(b => b.Shelf!.Row!.Room_Id)
                        .FirstOrDefault(),

                    RoomName = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row != null &&
                            b.Shelf.Row.Room != null)
                        .Select(b => b.Shelf!.Row!.Room!.Room_Name)
                        .FirstOrDefault(),

                    RoomCode = _context.Bins
                        .Where(b =>
                            x.LocationId.HasValue &&
                            b.LocationId == x.LocationId &&
                            b.Shelf != null &&
                            b.Shelf.Row != null &&
                            b.Shelf.Row.Room != null)
                        .Select(b => b.Shelf!.Row!.Room!.Room_Code)
                        .FirstOrDefault(),


                    // =================================================
                    // SUPPLIER
                    // =================================================

                    SupplierId = x.SupplierId,

                    SupplierName = x.Supplier != null
                        ? x.Supplier.Name
                        : null
                });
        }
    }
}