using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Partition;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class PartitionRepository : IPartitionRepository
    {
        private readonly DataBaseContext _context;

        public PartitionRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<PartitionDto>> GetAllAsync(
            int? warehouseId = null,
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

            var query = _context.Partitions
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
            // Search
            // =================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Code.Contains(search) ||
                    x.Name.Contains(search));
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
                .OrderBy(x => x.PartitionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PartitionDto
                {
                    PartitionId = x.PartitionId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    Code = x.Code,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    BinsCount = x.Bins.Count()
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<PartitionDto?> GetByIdAsync(int id)
        {
            return await _context.Partitions
                .AsNoTracking()
                .Where(x => x.PartitionId == id)
                .Select(x => new PartitionDto
                {
                    PartitionId = x.PartitionId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    Code = x.Code,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    BinsCount = x.Bins.Count()
                })
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET BY WAREHOUSE
        // =====================================================

        public async Task<IEnumerable<PartitionDto>> GetByWarehouseIdAsync(
            int warehouseId)
        {
            return await _context.Partitions
                .AsNoTracking()
                .Where(x => x.WarehouseId == warehouseId)
                .OrderBy(x => x.PartitionId)
                .Select(x => new PartitionDto
                {
                    PartitionId = x.PartitionId,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    Code = x.Code,

                    Name = x.Name,

                    Description = x.Description,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt,

                    BinsCount = x.Bins.Count()
                })
                .ToListAsync();
        }

        // =====================================================
        // SUMMARY
        // =====================================================

        public async Task<IEnumerable<PartitionSummaryDto>> GetSummaryAsync(
            int? warehouseId = null)
        {
            var query = _context.Partitions
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }

            return await query
                .OrderBy(x => x.PartitionId)
                .Select(x => new PartitionSummaryDto
                {
                    PartitionId = x.PartitionId,

                    Code = x.Code,

                    Name = x.Name,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    BinsCount = x.Bins.Count(),

                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Partition?> GetEntityByIdAsync(int id)
        {
            return await _context.Partitions
                .Include(x => x.Warehouse)
                .FirstOrDefaultAsync(x =>
                    x.PartitionId == id);
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Partition partition)
        {
            await _context.Partitions.AddAsync(partition);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Partition partition)
        {
            _context.Partitions.Update(partition);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Partition partition)
        {
            _context.Partitions.Remove(partition);
        }
    }
}