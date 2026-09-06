using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Bin;
using whm.Models;

namespace whm.Repositories
{
    public class BinRepository : IBinRepository
    {
        private readonly DataBaseContext _context;

        public BinRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<BinDto>> GetAllAsync(
            int? warehouseId = null,
            int? partitionId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            var query = _context.Bins
                .AsNoTracking()
                .AsQueryable();

            // =================================================
            // Filter by Warehouse
            // =================================================

            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }

            // =================================================
            // Filter by Partition
            // =================================================

            if (partitionId.HasValue)
            {
                query = query.Where(x =>
                    x.PartitionId == partitionId.Value);
            }

            // =================================================
            // Search
            // =================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    (x.Bin_Code != null &&
                     x.Bin_Code.Contains(search))
                    ||
                    x.Bin_Name.Contains(search));
            }

            // =================================================
            // Status
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
            // Pagination
            // =================================================

            return await query
                .OrderBy(x => x.Bin_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    PartitionId = x.PartitionId,

                    PartitionName = x.Partition.Name,

                    PartitionCode = x.Partition.Code,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    Description = x.Bin_Description,

                    IsActive = x.IsActive,

                    LocationsCount = x.Locations.Count,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<BinDto?> GetByIdAsync(int id)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x => x.Bin_Id == id)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    PartitionId = x.PartitionId,

                    PartitionName = x.Partition.Name,

                    PartitionCode = x.Partition.Code,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    Description = x.Bin_Description,

                    IsActive = x.IsActive,

                    LocationsCount = x.Locations.Count,

                    StockCount = x.Stocks.Count
                })
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Bin?> GetEntityByIdAsync(int id)
        {
            return await _context.Bins
                .Include(x => x.Warehouse)
                .Include(x => x.Partition)
                .FirstOrDefaultAsync(x =>
                    x.Bin_Id == id);
        }

        // =====================================================
        // GET BY WAREHOUSE
        // =====================================================

        public async Task<IEnumerable<BinDto>> GetByWarehouseIdAsync(
            int warehouseId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.WarehouseId == warehouseId)
                .OrderBy(x => x.Bin_Name)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    PartitionId = x.PartitionId,

                    PartitionName = x.Partition.Name,

                    PartitionCode = x.Partition.Code,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    Description = x.Bin_Description,

                    IsActive = x.IsActive,

                    LocationsCount = x.Locations.Count,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY PARTITION
        // =====================================================

        public async Task<IEnumerable<BinDto>> GetByPartitionIdAsync(
            int partitionId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.PartitionId == partitionId)
                .OrderBy(x => x.Bin_Name)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    PartitionId = x.PartitionId,

                    PartitionName = x.Partition.Name,

                    PartitionCode = x.Partition.Code,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    Description = x.Bin_Description,

                    IsActive = x.IsActive,

                    LocationsCount = x.Locations.Count,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY LOCATION
        // =====================================================

        public async Task<IEnumerable<BinDto>> GetByLocationIdAsync(
            int locationId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.Locations.Any(l =>
                        l.LocationId == locationId))
                .OrderBy(x => x.Bin_Name)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    PartitionId = x.PartitionId,

                    PartitionName = x.Partition.Name,

                    PartitionCode = x.Partition.Code,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    Description = x.Bin_Description,

                    IsActive = x.IsActive,

                    LocationsCount = x.Locations.Count,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // EXISTS BY PARTITION
        // =====================================================

        public async Task<bool> ExistsByPartitionIdAsync(
            int partitionId)
        {
            return await _context.Bins
                .AsNoTracking()
                .AnyAsync(x =>
                    x.PartitionId == partitionId);
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Bin bin)
        {
            await _context.Bins.AddAsync(bin);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Bin bin)
        {
            _context.Bins.Update(bin);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Bin bin)
        {
            _context.Bins.Remove(bin);
        }
    }
}