using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Location;
using whm.DTOs.Stock;
using whm.Models;

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
        // =====================================================

        public async Task<IEnumerable<LocationDto>> GetAllAsync(
            int? warehouseId = null,
            int? parentLocationId = null,
            int? roomId = null,
            int? rackId = null,
            int? shelfId = null,
            int? binId = null,
            string? search = null,
            string? type = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Locations
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }

            if (parentLocationId.HasValue)
            {
                query = query.Where(x =>
                    x.ParentLocationId == parentLocationId.Value);
            }

            if (roomId.HasValue)
            {
                query = query.Where(x =>
                    x.RoomId == roomId.Value);
            }

            if (rackId.HasValue)
            {
                query = query.Where(x =>
                    x.RackId == rackId.Value);
            }

            if (shelfId.HasValue)
            {
                query = query.Where(x =>
                    x.ShelfId == shelfId.Value);
            }

            if (binId.HasValue)
            {
                query = query.Where(x =>
                    x.BinId == binId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Code.Contains(search) ||
                    x.Name.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim();

                query = query.Where(x =>
                    x.Type == type);
            }

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

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            return await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    ParentLocationId = x.ParentLocationId,
                    ParentLocationName = x.ParentLocation != null
                        ? x.ParentLocation.Name
                        : null,

                    RoomId = x.RoomId,
                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    RackId = x.RackId,
                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    ShelfId = x.ShelfId,
                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    BinId = x.BinId,
                    BinName = x.Bin != null
                        ? x.Bin.Bin_Name
                        : null,

                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount = x.ChildLocations.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
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
                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    ParentLocationId = x.ParentLocationId,
                    ParentLocationName = x.ParentLocation != null
                        ? x.ParentLocation.Name
                        : null,

                    RoomId = x.RoomId,
                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    RackId = x.RackId,
                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    ShelfId = x.ShelfId,
                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    BinId = x.BinId,
                    BinName = x.Bin != null
                        ? x.Bin.Bin_Name
                        : null,

                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount = x.ChildLocations.Count
                })
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Location?> GetEntityByIdAsync(int id)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(x => x.LocationId == id);
        }

        // =====================================================
        // GET CHILDREN
        // =====================================================

        public async Task<IEnumerable<LocationDto>> GetChildrenAsync(
            int parentLocationId)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.ParentLocationId == parentLocationId)
                .OrderBy(x => x.Name)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,

                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    ParentLocationId = x.ParentLocationId,
                    ParentLocationName = x.ParentLocation != null
                        ? x.ParentLocation.Name
                        : null,

                    RoomId = x.RoomId,
                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    RackId = x.RackId,
                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    ShelfId = x.ShelfId,
                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    BinId = x.BinId,
                    BinName = x.Bin != null
                        ? x.Bin.Bin_Name
                        : null,

                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount = x.ChildLocations.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET LOCATION ID BY BIN ID
        // =====================================================

        public async Task<int> GetLocationIdByBinIdAsync(
            int binId)
        {
            return await _context.Locations
                .Where(x => x.BinId == binId)
                .Select(x => x.LocationId)
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET STRUCTURE
        // =====================================================

        public async Task<LocationStructureDto?> GetStructureAsync(
            int locationId)
        {
            var location = await _context.Locations
                .AsNoTracking()
                .Where(x => x.LocationId == locationId)
                .Select(x => new
                {
                    x.LocationId,
                    x.WarehouseId,
                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    x.ParentLocationId,

                    ParentLocationName = x.ParentLocation != null
                        ? x.ParentLocation.Name
                        : null,

                    x.RoomId,
                    x.RackId,
                    x.ShelfId,
                    x.BinId,

                    x.Code,
                    x.Name,
                    x.Type,
                    x.IsActive
                })
                .FirstOrDefaultAsync();

            if (location == null)
                return null;

            var result = new LocationStructureDto
            {
                LocationId = location.LocationId,
                WarehouseId = location.WarehouseId,
                WarehouseName = location.WarehouseName,

                ParentLocationId = location.ParentLocationId,
                ParentLocationName = location.ParentLocationName,

                RoomId = location.RoomId,
                RackId = location.RackId,
                ShelfId = location.ShelfId,
                BinId = location.BinId,

                Code = location.Code,
                Name = location.Name,
                Type = location.Type,
                IsActive = location.IsActive
            };

            if (!location.WarehouseId.HasValue)
                return result;

            var warehouseId = location.WarehouseId.Value;

            // -------------------------------------------------
            // ROOMS
            // -------------------------------------------------

            result.Rooms = await _context.Rooms
                .AsNoTracking()
                .Where(x =>
                    x.Warehouse_Id == warehouseId)
                .OrderBy(x => x.Room_Name)
                .Select(x => new LocationRoomDto
                {
                    RoomId = x.Room_Id,
                    Code = x.Room_Code ?? string.Empty,
                    Name = x.Room_Name,
                    IsActive = x.IsActive,
                    RackCount = x.Racks.Count
                })
                .ToListAsync();

            // -------------------------------------------------
            // RACKS
            // -------------------------------------------------

            result.Racks = await _context.Racks
                .AsNoTracking()
                .Where(x =>
                    x.Room != null &&
                    x.Room.Warehouse_Id == warehouseId)
                .OrderBy(x => x.Rack_Name)
                .Select(x => new LocationRackDto
                {
                    RackId = x.Rack_Id,
                    RoomId = x.Room_Id,

                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    Code = x.Rack_Code ?? string.Empty,
                    Name = x.Rack_Name,
                    IsActive = x.IsActive,
                    ShelfCount = x.Shelves.Count
                })
                .ToListAsync();

            // -------------------------------------------------
            // SHELVES
            // -------------------------------------------------

            result.Shelves = await _context.Shelves
                .AsNoTracking()
                .Where(x =>
                    x.Rack != null &&
                    x.Rack.Room != null &&
                    x.Rack.Room.Warehouse_Id == warehouseId)
                .OrderBy(x => x.Shelf_Name)
                .Select(x => new LocationShelfDto
                {
                    ShelfId = x.Shelf_Id,
                    RackId = x.Rack_Id,

                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    Code = x.Shelf_Code ?? string.Empty,
                    Name = x.Shelf_Name,
                    IsActive = x.IsActive,
                    BinCount = x.Bins.Count
                })
                .ToListAsync();

            // -------------------------------------------------
            // BINS
            // -------------------------------------------------

            result.Bins = await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.Shelf != null &&
                    x.Shelf.Rack != null &&
                    x.Shelf.Rack.Room != null &&
                    x.Shelf.Rack.Room.Warehouse_Id == warehouseId)
                .OrderBy(x => x.Bin_Name)
                .Select(x => new LocationBinDto
                {
                    BinId = x.Bin_Id,
                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    Code = x.Bin_Code ?? string.Empty,
                    Name = x.Bin_Name,
                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();

            return result;
        }

        // =====================================================
        // GET INVENTORY
        // =====================================================

        public async Task<IEnumerable<StockDto>> GetInventoryAsync(
            int locationId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x => x.LocationId == locationId)
                .OrderBy(x => x.StockId)
                .Select(x => new StockDto
                {
                    StockId = x.StockId,

                    // Product
                    ProductId = x.ProductId,

                    ProductName = x.Product != null
                        ? x.Product.Name
                        : string.Empty,

                    CategoryName =
                        x.Product != null &&
                        x.Product.Category != null
                            ? x.Product.Category.Name
                            : string.Empty,

                    SKU = x.Product != null
                        ? x.Product.SKU
                        : string.Empty,

                    Barcode = x.Product != null
                        ? x.Product.Barcode
                        : null,

                    // Warehouse
                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : string.Empty,

                    // Location
                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    // Room
                    RoomId = x.Location != null
                        ? x.Location.RoomId
                        : null,

                    RoomName =
                        x.Location != null &&
                        x.Location.Room != null
                            ? x.Location.Room.Room_Name
                            : null,

                    RoomCode =
                        x.Location != null &&
                        x.Location.Room != null
                            ? x.Location.Room.Room_Code
                            : null,

                    // Rack
                    RackId = x.Location != null
                        ? x.Location.RackId
                        : null,

                    RackName =
                        x.Location != null &&
                        x.Location.Rack != null
                            ? x.Location.Rack.Rack_Name
                            : null,

                    RackCode =
                        x.Location != null &&
                        x.Location.Rack != null
                            ? x.Location.Rack.Rack_Code
                            : null,

                    // Shelf
                    ShelfId = x.Location != null
                        ? x.Location.ShelfId
                        : null,

                    ShelfName =
                        x.Location != null &&
                        x.Location.Shelf != null
                            ? x.Location.Shelf.Shelf_Name
                            : null,

                    ShelfCode =
                        x.Location != null &&
                        x.Location.Shelf != null
                            ? x.Location.Shelf.Shelf_Code
                            : null,

                    // Bin
                    BinId = x.Location != null
                        ? x.Location.BinId
                        : null,

                    BinName =
                        x.Location != null &&
                        x.Location.Bin != null
                            ? x.Location.Bin.Bin_Name
                            : null,

                    BinCode =
                        x.Location != null &&
                        x.Location.Bin != null
                            ? x.Location.Bin.Bin_Code
                            : null,

                    // Supplier
                    SupplierId = x.SupplierId,

                    SupplierName = x.Supplier != null
                        ? x.Supplier.Name
                        : null,

                    // Stock data
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
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();
        }

        // =====================================================
        // GET OCCUPANCY
        // =====================================================

        public async Task<LocationOccupancyDto?> GetOccupancyAsync(
            int locationId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x => x.LocationId == locationId)
                .GroupBy(x => new
                {
                    x.LocationId,
                    LocationName = x.Location != null
                        ? x.Location.Name
                        : string.Empty,

                    LocationType = x.Location != null
                        ? x.Location.Type
                        : string.Empty
                })
                .Select(g => new LocationOccupancyDto
                {
                    LocationId = g.Key.LocationId!.Value,

                    LocationName = g.Key.LocationName,

                    LocationType = g.Key.LocationType,

                    TotalStockItems = g.Count(),

                    TotalQuantity = g.Sum(x => x.Quantity),

                    TotalReservedQuantity =
                        g.Sum(x => x.ReservedQuantity),

                    TotalAvailableQuantity =
                        g.Sum(x => x.AvailableQuantity),

                    TotalValue =
                        g.Sum(x =>
                            x.Quantity * x.UnitPrice),

                    IsOccupied = g.Any()
                })
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET LOCATION TREE
        // =====================================================

        public async Task<IEnumerable<LocationTreeDto>> GetTreeAsync()
        {
            var locations = await _context.Locations
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new LocationTreeDto
                {
                    LocationId = x.LocationId,
                    WarehouseId = x.WarehouseId,

                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive,

                    Children = new List<LocationTreeDto>()
                })
                .ToListAsync();

            var lookup = locations
                .ToDictionary(x => x.LocationId);

            var childrenMap = await _context.Locations
                .AsNoTracking()
                .Select(x => new
                {
                    x.LocationId,
                    x.ParentLocationId
                })
                .ToListAsync();

            foreach (var item in childrenMap)
            {
                if (!item.ParentLocationId.HasValue)
                    continue;

                if (!lookup.TryGetValue(
                    item.ParentLocationId.Value,
                    out var parent))
                {
                    continue;
                }

                if (lookup.TryGetValue(
                    item.LocationId,
                    out var child))
                {
                    parent.Children.Add(child);
                }
            }

            var rootLocations = locations
                .Where(x =>
                    !childrenMap.Any(c =>
                        c.LocationId == x.LocationId &&
                        c.ParentLocationId.HasValue))
                .OrderBy(x => x.Name)
                .ToList();

            return rootLocations;
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Location location)
        {
            _context.Locations.Update(location);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Location location)
        {
            _context.Locations.Remove(location);
        }
    }
}