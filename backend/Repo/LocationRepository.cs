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
            int? partitionId = null,
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


            // =========================
            // Warehouse Filter
            // =========================

            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.Bin.WarehouseId == warehouseId.Value);
            }


            // =========================
            // Partition Filter
            // =========================

            if (partitionId.HasValue)
            {
                query = query.Where(x =>
                    x.Bin.PartitionId == partitionId.Value);
            }


            // =========================
            // Bin Filter
            // =========================

            if (binId.HasValue)
            {
                query = query.Where(x =>
                    x.BinId == binId.Value);
            }


            // =========================
            // Search
            // =========================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Code.Contains(search) ||
                    x.Name.Contains(search));
            }


            // =========================
            // Type Filter
            // =========================

            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim();

                query = query.Where(x =>
                    x.Type == type);
            }


            // =========================
            // Status Filter
            // =========================

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


            // =========================
            // Pagination
            // =========================

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


                    // =========================
                    // Warehouse
                    // =========================

                    WarehouseId = x.Bin.WarehouseId,

                    WarehouseName = x.Bin.Warehouse.Name,


                    // =========================
                    // Partition
                    // =========================

                    PartitionId = x.Bin.PartitionId,

                    PartitionName = x.Bin.Partition.Name,

                    PartitionCode = x.Bin.Partition.Code,


                    // =========================
                    // Bin
                    // =========================

                    BinId = x.BinId,

                    BinName = x.Bin.Bin_Name,

                    BinCode = x.Bin.Bin_Code,


                    // =========================
                    // Location
                    // =========================

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,


                    // =========================
                    // Children
                    // =========================

                    ChildrenCount = 0
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
                .Where(x =>
                    x.LocationId == id)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,


                    // =========================
                    // Warehouse
                    // =========================

                    WarehouseId = x.Bin.WarehouseId,

                    WarehouseName = x.Bin.Warehouse.Name,


                    // =========================
                    // Partition
                    // =========================

                    PartitionId = x.Bin.PartitionId,

                    PartitionName = x.Bin.Partition.Name,

                    PartitionCode = x.Bin.Partition.Code,


                    // =========================
                    // Bin
                    // =========================

                    BinId = x.BinId,

                    BinName = x.Bin.Bin_Name,

                    BinCode = x.Bin.Bin_Code,


                    // =========================
                    // Location
                    // =========================

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,


                    ChildrenCount = 0
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Location?> GetEntityByIdAsync(int id)
        {
            return await _context.Locations
                .Include(x => x.Bin)
                .FirstOrDefaultAsync(x =>
                    x.LocationId == id);
        }


        // =====================================================
        // GET LOCATIONS BY BIN
        // =====================================================

        public async Task<IEnumerable<LocationDto>> GetByBinIdAsync(
            int binId)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.BinId == binId)
                .OrderBy(x => x.Name)
                .Select(x => new LocationDto
                {
                    LocationId = x.LocationId,


                    // =========================
                    // Warehouse
                    // =========================

                    WarehouseId = x.Bin.WarehouseId,

                    WarehouseName = x.Bin.Warehouse.Name,


                    // =========================
                    // Partition
                    // =========================

                    PartitionId = x.Bin.PartitionId,

                    PartitionName = x.Bin.Partition.Name,

                    PartitionCode = x.Bin.Partition.Code,


                    // =========================
                    // Bin
                    // =========================

                    BinId = x.BinId,

                    BinName = x.Bin.Bin_Name,

                    BinCode = x.Bin.Bin_Code,


                    // =========================
                    // Location
                    // =========================

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    ChildrenCount = 0
                })
                .ToListAsync();
        }


        // =====================================================
        // GET LOCATION ID BY BIN
        // =====================================================

        public async Task<int?> GetLocationIdByBinIdAsync(
            int binId)
        {
            return await _context.Locations
                .Where(x =>
                    x.BinId == binId)
                .Select(x =>
                    (int?)x.LocationId)
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET STRUCTURE
        // =====================================================

        public async Task<LocationStructureDto?> GetStructureAsync(
            int locationId)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.LocationId == locationId)
                .Select(x => new LocationStructureDto
                {
                    LocationId = x.LocationId,


                    // =========================
                    // Warehouse
                    // =========================

                    WarehouseId = x.Bin.WarehouseId,

                    WarehouseName = x.Bin.Warehouse.Name,


                    // =========================
                    // Partition
                    // =========================

                    PartitionId = x.Bin.PartitionId,

                    PartitionName = x.Bin.Partition.Name,

                    PartitionCode = x.Bin.Partition.Code,


                    // =========================
                    // Bin
                    // =========================

                    BinId = x.BinId,

                    BinName = x.Bin.Bin_Name,

                    BinCode = x.Bin.Bin_Code,


                    // =========================
                    // Location
                    // =========================

                    Code = x.Code,

                    Name = x.Name,

                    Type = x.Type,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count()
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET INVENTORY
        // =====================================================

        public async Task<IEnumerable<StockDto>> GetInventoryAsync(
            int locationId)
        {
            return await _context.Stocks
                .AsNoTracking()
                .Where(x =>
                    x.LocationId == locationId)
                .OrderBy(x => x.StockId)
                .Select(x => new StockDto
                {
                    // =========================
                    // Stock
                    // =========================

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


                    // =========================
                    // Product
                    // =========================

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    SKU = x.Product.SKU,

                    Barcode = x.Product.Barcode,

                    CategoryName = x.Product.Category != null
                        ? x.Product.Category.Name
                        : string.Empty,


                    // =========================
                    // Warehouse
                    // =========================

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,


                    // =========================
                    // Partition
                    // =========================

                    PartitionId = x.Location != null
                        ? x.Location.Bin.PartitionId
                        : null,

                    PartitionName = x.Location != null
                        ? x.Location.Bin.Partition.Name
                        : null,

                    PartitionCode = x.Location != null
                        ? x.Location.Bin.Partition.Code
                        : null,


                    // =========================
                    // Bin
                    // =========================

                    BinId = x.Location != null
                        ? x.Location.BinId
                        : null,

                    BinName = x.Location != null
                        ? x.Location.Bin.Bin_Name
                        : null,

                    BinCode = x.Location != null
                        ? x.Location.Bin.Bin_Code
                        : null,


                    // =========================
                    // Location
                    // =========================

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    LocationCode = x.Location != null
                        ? x.Location.Code
                        : null,


                    // =========================
                    // Supplier
                    // =========================

                    SupplierId = x.SupplierId,

                    SupplierName = x.Supplier != null
                        ? x.Supplier.Name
                        : null
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
                .Where(x =>
                    x.LocationId == locationId)
                .GroupBy(x => new
                {
                    LocationId = x.LocationId!.Value,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : string.Empty,

                    LocationType = x.Location != null
                        ? x.Location.Type
                        : string.Empty
                })
                .Select(g => new LocationOccupancyDto
                {
                    LocationId = g.Key.LocationId,

                    LocationName = g.Key.LocationName,

                    LocationType = g.Key.LocationType,

                    TotalStockItems = g.Count(),

                    TotalQuantity = g.Sum(x =>
                        x.Quantity),

                    TotalReservedQuantity = g.Sum(x =>
                        x.ReservedQuantity),

                    TotalAvailableQuantity = g.Sum(x =>
                        x.AvailableQuantity),

                    TotalValue = g.Sum(x =>
                        x.Quantity * x.UnitPrice),

                    IsOccupied = true
                })
                .FirstOrDefaultAsync();
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