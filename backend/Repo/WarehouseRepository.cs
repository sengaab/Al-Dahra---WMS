using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Stock;
using whm.DTOs.Warehouse;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DataBaseContext _context;

        public WarehouseRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // GET /api/warehouses
        // =====================================================

        public async Task<List<WarehouseDto>> GetAllAsync(
            int? siteId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            // =================================================
            // PAGINATION VALIDATION
            // =================================================

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;


            // =================================================
            // BASE QUERY
            // =================================================

            var query = _context.Warehouses
                .AsNoTracking()
                .AsQueryable();


            // =================================================
            // FILTER BY SITE
            // =================================================

            if (siteId.HasValue)
            {
                query = query.Where(x =>
                    x.SiteId == siteId.Value);
            }


            // =================================================
            // SEARCH
            // =================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Code.Contains(search) ||
                    (x.Description != null &&
                     x.Description.Contains(search)));
            }


            // =================================================
            // STATUS
            // =================================================

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals(
                    "active",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x =>
                        x.IsActive);
                }
                else if (status.Equals(
                    "inactive",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x =>
                        !x.IsActive);
                }
            }


            // =================================================
            // PAGINATION
            // =================================================

            query = query
                .OrderBy(x => x.WarehouseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);


            // =================================================
            // DTO
            // =================================================

            return await query
                .Select(x => new WarehouseDto
                {
                    WarehouseId = x.WarehouseId,

                    SiteId = x.SiteId,

                    SiteName = x.Site.Name,

                    Code = x.Code,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    // =========================================
                    // NEW STRUCTURE
                    // =========================================

                    PartitionsCount =
                        x.Partitions.Count(),

                    BinsCount =
                        x.Bins.Count(),

                    LocationsCount =
                        x.Bins
                            .SelectMany(b => b.Locations)
                            .Count()
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/warehouses/{id}
        // =====================================================

        public async Task<WarehouseDto?> GetByIdAsync(
            int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == id)
                .Select(x => new WarehouseDto
                {
                    WarehouseId = x.WarehouseId,

                    SiteId = x.SiteId,

                    SiteName = x.Site.Name,

                    Code = x.Code,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    PartitionsCount =
                        x.Partitions.Count(),

                    BinsCount =
                        x.Bins.Count(),

                    LocationsCount =
                        x.Bins
                            .SelectMany(b => b.Locations)
                            .Count()
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Warehouse?> GetEntityByIdAsync(
            int id)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == id);
        }


        // =====================================================
        // GET INVENTORY
        // GET /api/warehouses/{id}/inventory
        // =====================================================

        public async Task<List<StockDto>> GetInventoryAsync(
            int warehouseId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId)
                .Select(x => new StockDto
                {
                    // =========================================
                    // STOCK
                    // =========================================

                    StockId = x.StockId,

                    StockCode = x.StockCode,

                    BatchNumber = x.BatchNumber,

                    ExpiryDate = x.ExpiryDate,

                    Quantity = x.Quantity,

                    ReservedQuantity =
                        x.ReservedQuantity,

                    AvailableQuantity =
                        x.AvailableQuantity,

                    UnitPrice = x.UnitPrice,

                    MinimumStock =
                        x.MinimumStock,

                    StockStatus =
                        x.stockStatus.ToString(),

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,


                    // =========================================
                    // PRODUCT
                    // =========================================

                    ProductId = x.ProductId,

                    ProductName =
                        x.Product.Name,

                    SKU =
                        x.Product.SKU,

                    Barcode =
                        x.Product.Barcode,

                    CategoryName =
                        _context.Categories
                            .Where(c =>
                                c.CategoryId ==
                                x.Product.CategoryId)
                            .Select(c => c.Name)
                            .FirstOrDefault()
                        ?? string.Empty,


                    // =========================================
                    // WAREHOUSE
                    // =========================================

                    WarehouseId =
                        x.WarehouseId,

                    WarehouseName =
                        x.Warehouse.Name,


                    // =========================================
                    // PARTITION
                    // =========================================

                    PartitionId =
                        x.Location != null
                            ? x.Location.Bin.PartitionId
                            : null,

                    PartitionName =
                        x.Location != null
                            ? x.Location.Bin.Partition.Name
                            : null,

                    PartitionCode =
                        x.Location != null
                            ? x.Location.Bin.Partition.Code
                            : null,


                    // =========================================
                    // BIN
                    // =========================================

                    BinId =
                        x.Location != null
                            ? x.Location.BinId
                            : null,

                    BinName =
                        x.Location != null
                            ? x.Location.Bin.Bin_Name
                            : null,

                    BinCode =
                        x.Location != null
                            ? x.Location.Bin.Bin_Code
                            : null,


                    // =========================================
                    // LOCATION
                    // =========================================

                    LocationId =
                        x.LocationId,

                    LocationName =
                        x.Location != null
                            ? x.Location.Name
                            : null,

                    LocationCode =
                        x.Location != null
                            ? x.Location.Code
                            : null,


                    // =========================================
                    // SUPPLIER
                    // =========================================

                    SupplierId =
                        x.SupplierId,

                    SupplierName =
                        x.Supplier != null
                            ? x.Supplier.Name
                            : null
                })
                .ToListAsync();
        }


        // =====================================================
        // GET STATS
        // GET /api/warehouses/{id}/stats
        // =====================================================

        public async Task<WarehouseStatsDto?> GetStatsAsync(
            int warehouseId)
        {
            // =================================================
            // CHECK WAREHOUSE
            // =================================================

            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == warehouseId);

            if (warehouse == null)
                return null;


            // =================================================
            // PARTITIONS
            // =================================================

            var partitions = _context.Partitions
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId);


            // =================================================
            // BINS
            // =================================================

            var bins = _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId);


            // =================================================
            // LOCATIONS
            // =================================================

            var locations = _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.Bin.WarehouseId == warehouseId);


            // =================================================
            // STOCK
            // =================================================

            var stocks = _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId);


            // =================================================
            // RETURN STATS
            // =================================================

            return new WarehouseStatsDto
            {
                WarehouseId =
                    warehouse.WarehouseId,

                WarehouseName =
                    warehouse.Name,


                // =============================================
                // STRUCTURE
                // =============================================

                TotalPartitions =
                    await partitions.CountAsync(),

                TotalBins =
                    await bins.CountAsync(),

                TotalLocations =
                    await locations.CountAsync(),

                ActiveLocations =
                    await locations.CountAsync(x =>
                        x.IsActive),

                InactiveLocations =
                    await locations.CountAsync(x =>
                        !x.IsActive),


                // =============================================
                // STOCK
                // =============================================

                TotalStockItems =
                    await stocks.CountAsync(),

                TotalQuantity =
                    await stocks.SumAsync(
                        x => (decimal?)x.Quantity)
                    ?? 0,

                TotalReservedQuantity =
                    await stocks.SumAsync(
                        x => (decimal?)x.ReservedQuantity)
                    ?? 0,

                TotalAvailableQuantity =
                    await stocks.SumAsync(
                        x => (decimal?)x.AvailableQuantity)
                    ?? 0,

                TotalValue =
                    await stocks.SumAsync(
                        x =>
                            (decimal?)
                            (x.Quantity *
                             x.UnitPrice))
                    ?? 0
            };
        }


        // =====================================================
        // GET OCCUPANCY
        // GET /api/warehouses/{id}/occupancy
        // =====================================================

        public async Task<WarehouseOccupancyDto?> GetOccupancyAsync(
            int warehouseId)
        {
            // =================================================
            // CHECK WAREHOUSE
            // =================================================

            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == warehouseId);

            if (warehouse == null)
                return null;


            // =================================================
            // TOTAL ACTIVE LOCATIONS
            // =================================================

            var totalLocations =
                await _context.Locations
                    .AsNoTracking()
                    .CountAsync(x =>
                        x.Bin.WarehouseId ==
                        warehouseId &&
                        x.IsActive);


            // =================================================
            // OCCUPIED LOCATIONS
            // =================================================

            // A Location is considered occupied
            // when it has available stock.

            var occupiedLocations =
                await _context.Stocks
                    .AsNoTracking()
                    .Where(x =>
                        x.WarehouseId ==
                        warehouseId &&

                        x.LocationId.HasValue &&

                        x.AvailableQuantity > 0)
                    .Select(x =>
                        x.LocationId)
                    .Distinct()
                    .CountAsync();


            // =================================================
            // EMPTY LOCATIONS
            // =================================================

            var emptyLocations =
                Math.Max(
                    totalLocations -
                    occupiedLocations,
                    0);


            // =================================================
            // OCCUPANCY %
            // =================================================

            decimal occupancyPercentage = 0;

            if (totalLocations > 0)
            {
                occupancyPercentage =
                    Math.Round(
                        (decimal)occupiedLocations /
                        totalLocations *
                        100,
                        2);
            }


            // =================================================
            // RESULT
            // =================================================

            return new WarehouseOccupancyDto
            {
                WarehouseId =
                    warehouse.WarehouseId,

                WarehouseName =
                    warehouse.Name,

                TotalLocations =
                    totalLocations,

                OccupiedLocations =
                    occupiedLocations,

                EmptyLocations =
                    emptyLocations,

                OccupancyPercentage =
                    occupancyPercentage
            };
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(
            Warehouse warehouse)
        {
            await _context.Warehouses
                .AddAsync(warehouse);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(
            Warehouse warehouse)
        {
            _context.Warehouses
                .Update(warehouse);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(
            Warehouse warehouse)
        {
            _context.Warehouses
                .Remove(warehouse);
        }
    }
}