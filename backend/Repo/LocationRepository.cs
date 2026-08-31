using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Location;
using whm.DTOs.Stock;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataBaseContext _context;

        public LocationRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // GET /api/locations
        // =====================================================

        public async Task<List<LocationDto>> GetAllAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .OrderBy(x => x.LocationId)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    ParentLocationId = x.ParentLocationId,

                    ParentLocationName =
                        x.ParentLocation != null
                            ? x.ParentLocation.Name
                            : null,

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount =
                        x.ChildLocations.Count()
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/locations/{id}
        // =====================================================

        public async Task<LocationDto?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x => x.LocationId == id)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    ParentLocationId = x.ParentLocationId,

                    ParentLocationName =
                        x.ParentLocation != null
                            ? x.ParentLocation.Name
                            : null,

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount =
                        x.ChildLocations.Count()
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Location?> GetEntityByIdAsync(int id)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(x =>
                    x.LocationId == id);
        }


        // =====================================================
        // GET CHILDREN
        // GET /api/locations/{id}/children
        // =====================================================

        public async Task<List<LocationDto>> GetChildrenAsync(
            int locationId)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.ParentLocationId == locationId)
                .OrderBy(x => x.LocationId)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    ParentLocationId = x.ParentLocationId,

                    ParentLocationName =
                        x.ParentLocation != null
                            ? x.ParentLocation.Name
                            : null,

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount =
                        x.ChildLocations.Count()
                })
                .ToListAsync();
        }


        // =====================================================
        // GET INVENTORY
        // GET /api/locations/{id}/inventory
        // =====================================================

        public async Task<List<StockDto>> GetInventoryAsync(
            int locationId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.LocationId == locationId)
                .Select(x => new StockDto
                {
                    StockId = x.StockId,

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    CategoryName =
                        _context.Categories
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

                    LocationName =
                        x.Location != null
                            ? x.Location.Name
                            : null,

                    StockCode = x.StockCode,

                    BatchNumber = x.BatchNumber,

                    Barcode = x.Product.Barcode,

                    ExpiryDate = x.ExpiryDate,

                    Quantity = x.Quantity,

                    ReservedQuantity =
                        x.ReservedQuantity,

                    AvailableQuantity =
                        x.AvailableQuantity,

                    UnitPrice = x.UnitPrice,

                    MinimumStock =
                        x.Product.MinimumStock,

                    StockStatus =
                        x.stockStatus.ToString(),

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();
        }


        // =====================================================
        // OCCUPANCY
        // GET /api/locations/{id}/occupancy
        // =====================================================

        public async Task<LocationOccupancyDto?> GetOccupancyAsync(
            int locationId)
        {
            var location =
                await _context.Locations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.LocationId == locationId);

            if (location == null)
                return null;


            var stocks = _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.LocationId == locationId);


            var totalStockItems =
                await stocks.CountAsync();


            var totalQuantity =
                await stocks
                    .SumAsync(x =>
                        (decimal?)x.Quantity) ?? 0;


            var totalReservedQuantity =
                await stocks
                    .SumAsync(x =>
                        (decimal?)x.ReservedQuantity) ?? 0;


            var totalAvailableQuantity =
                await stocks
                    .SumAsync(x =>
                        (decimal?)x.AvailableQuantity) ?? 0;


            var totalValue =
                await stocks
                    .SumAsync(x =>
                        (decimal?)
                        (x.Quantity * x.UnitPrice)) ?? 0;


            return new LocationOccupancyDto
            {
                LocationId =
                    location.LocationId,

                LocationName =
                    location.Name,

                LocationType =
                    location.Type,

                TotalStockItems =
                    totalStockItems,

                TotalQuantity =
                    totalQuantity,

                TotalReservedQuantity =
                    totalReservedQuantity,

                TotalAvailableQuantity =
                    totalAvailableQuantity,

                TotalValue =
                    totalValue,

                IsOccupied =
                    totalAvailableQuantity > 0
            };
        }


        // =====================================================
        // TREE
        // GET /api/locations/tree
        // =====================================================

        public async Task<List<WarehouseTreeDto>> GetTreeAsync()
        {
            // =================================================
            // GET WAREHOUSES
            // =================================================

            var warehouses =
                await _context.Warehouses
                    .AsNoTracking()
                    .OrderBy(x => x.WarehouseId)
                    .Select(x => new WarehouseTreeDto
                    {
                        WarehouseId =
                            x.WarehouseId,

                        Code =
                            x.Code,

                        Name =
                            x.Name,

                        IsActive =
                            x.IsActive
                    })
                    .ToListAsync();


            // =================================================
            // GET LOCATIONS
            // =================================================

            var locations =
                await _context.Locations
                    .AsNoTracking()
                    .OrderBy(x => x.LocationId)
                    .Select(x => new
                    {
                        x.LocationId,

                        x.WarehouseId,

                        x.ParentLocationId,

                        x.Code,

                        x.Name,

                        x.Type,

                        x.IsActive
                    })
                    .ToListAsync();


            // =================================================
            // BUILD TREE
            // =================================================

            foreach (var warehouse in warehouses)
            {
                var warehouseLocations =
                    locations
                        .Where(x =>
                            x.WarehouseId ==
                            warehouse.WarehouseId)
                        .ToList();


                var locationDictionary =
                    warehouseLocations
                        .ToDictionary(
                            x => x.LocationId,
                            x => new LocationTreeDto
                            {
                                LocationId =
                                    x.LocationId,

                                WarehouseId =
                                    x.WarehouseId,

                                Code =
                                    x.Code,

                                Name =
                                    x.Name,

                                Type =
                                    x.Type,

                                IsActive =
                                    x.IsActive
                            });


                foreach (var location in warehouseLocations)
                {
                    var current =
                        locationDictionary[
                            location.LocationId];


                    // Parent exists
                    if (location.ParentLocationId.HasValue &&
                        locationDictionary.ContainsKey(
                            location.ParentLocationId.Value))
                    {
                        locationDictionary[
                            location.ParentLocationId.Value]
                            .Children
                            .Add(current);
                    }
                    else
                    {
                        // Root location
                        warehouse.Locations.Add(current);
                    }
                }
            }


            return warehouses;
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Location location)
        {
            await _context.Locations
                .AddAsync(location);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Location location)
        {
            _context.Locations
                .Update(location);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Location location)
        {
            _context.Locations
                .Remove(location);
        }
    }
}