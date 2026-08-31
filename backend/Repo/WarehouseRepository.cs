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
                    query = query.Where(x => x.IsActive);
                }
                else if (status.Equals(
                    "inactive",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => !x.IsActive);
                }
            }


            // =================================================
            // PAGINATION
            // =================================================

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

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

                    LocationCount = x.Locations.Count()
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/warehouses/{id}
        // =====================================================

        public async Task<WarehouseDto?> GetByIdAsync(int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(x => x.WarehouseId == id)
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

                    LocationCount = x.Locations.Count()
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Warehouse?> GetEntityByIdAsync(int id)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == id);
        }


        // =====================================================
        // GET LOCATIONS
        // GET /api/warehouses/{id}/locations
        // =====================================================

        public async Task<List<WarehouseLocationDto>> GetLocationsAsync(
            int warehouseId)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId)
                .OrderBy(x => x.LocationId)
                .Select(x => new WarehouseLocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,

                    ParentLocationId = x.ParentLocationId,

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive
                })
                .ToListAsync();
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
                    StockId = x.StockId,

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    CategoryName = _context.Categories
                        .Where(c =>
                            c.CategoryId ==
                            x.Product.CategoryId)
                        .Select(c => c.Name)
                        .FirstOrDefault()
                        ?? string.Empty,

                    SKU = x.Product.SKU,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    StockCode = x.StockCode,

                    BatchNumber = x.BatchNumber,

                    Barcode = x.Product.Barcode,

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
        // GET STATS
        // GET /api/warehouses/{id}/stats
        // =====================================================

        public async Task<WarehouseStatsDto?> GetStatsAsync(
            int warehouseId)
        {
            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == warehouseId);

            if (warehouse == null)
                return null;


            var locations = _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId);


            var stocks = _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId);


            return new WarehouseStatsDto
            {
                WarehouseId = warehouse.WarehouseId,

                WarehouseName = warehouse.Name,

                TotalLocations =
                    await locations.CountAsync(),

                ActiveLocations =
                    await locations.CountAsync(x =>
                        x.IsActive),

                InactiveLocations =
                    await locations.CountAsync(x =>
                        !x.IsActive),

                TotalStockItems =
                    await stocks.CountAsync(),

                TotalQuantity =
                    await stocks
                        .SumAsync(x =>
                            (decimal?)x.Quantity) ?? 0,

                TotalReservedQuantity =
                    await stocks
                        .SumAsync(x =>
                            (decimal?)x.ReservedQuantity) ?? 0,

                TotalAvailableQuantity =
                    await stocks
                        .SumAsync(x =>
                            (decimal?)x.AvailableQuantity) ?? 0,

                TotalValue =
                    await stocks
                        .SumAsync(x =>
                            (decimal?)(x.Quantity *
                                       x.UnitPrice)) ?? 0
            };
        }


        // =====================================================
        // GET OCCUPANCY
        // GET /api/warehouses/{id}/occupancy
        // =====================================================

        public async Task<WarehouseOccupancyDto?> GetOccupancyAsync(
            int warehouseId)
        {
            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == warehouseId);

            if (warehouse == null)
                return null;


            var totalLocations =
                await _context.Locations
                    .AsNoTracking()
                    .CountAsync(x =>
                        x.WarehouseId == warehouseId &&
                        x.IsActive);


            // Location is considered occupied
            // when there is stock assigned to it.

            var occupiedLocations =
                await _context.Stocks
                    .AsNoTracking()
                    .Where(x =>
                        x.WarehouseId == warehouseId &&
                        x.LocationId.HasValue &&
                        x.AvailableQuantity > 0)
                    .Select(x => x.LocationId)
                    .Distinct()
                    .CountAsync();


            var emptyLocations =
                Math.Max(
                    totalLocations - occupiedLocations,
                    0);


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

        public async Task AddAsync(Warehouse warehouse)
        {
            await _context.Warehouses
                .AddAsync(warehouse);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Warehouse warehouse)
        {
            _context.Warehouses
                .Update(warehouse);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Warehouse warehouse)
        {
            _context.Warehouses
                .Remove(warehouse);
        }
    }
}