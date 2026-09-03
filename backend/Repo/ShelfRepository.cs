using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Shelf;
using whm.Models;

namespace whm.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly DataBaseContext _context;

        public ShelfRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<ShelfDto>> GetAllAsync(
            int? rackId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Shelves
                .AsNoTracking()
                .AsQueryable();

            // =================================================
            // FILTER BY RACK
            // =================================================

            if (rackId.HasValue)
            {
                query = query.Where(x =>
                    x.Rack_Id == rackId.Value);
            }

            // =================================================
            // FILTER BY LOCATION
            //
            // Location contains ShelfId
            // =================================================

            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId.Value &&
                        l.ShelfId == x.Shelf_Id));
            }

            // =================================================
            // SEARCH
            // =================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    (x.Shelf_Code != null &&
                     x.Shelf_Code.Contains(search))
                    ||
                    x.Shelf_Name.Contains(search));
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

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            // =================================================
            // RESULT
            // =================================================

            return await query
                .OrderBy(x => x.Shelf_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Rack_Id,

                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<ShelfDto?> GetByIdAsync(int id)
        {
            return await _context.Shelves
                .AsNoTracking()
                .Where(x => x.Shelf_Id == id)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Rack_Id,

                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .FirstOrDefaultAsync();
        }

        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Shelf?> GetEntityByIdAsync(int id)
        {
            return await _context.Shelves
                .FirstOrDefaultAsync(x =>
                    x.Shelf_Id == id);
        }

        // =====================================================
        // GET BY RACK
        // =====================================================

        public async Task<IEnumerable<ShelfDto>> GetByRackIdAsync(
            int rackId)
        {
            return await _context.Shelves
                .AsNoTracking()
                .Where(x =>
                    x.Rack_Id == rackId)
                .OrderBy(x => x.Shelf_Name)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Rack_Id,

                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.ShelfId == x.Shelf_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET BY LOCATION
        // =====================================================

        public async Task<IEnumerable<ShelfDto>> GetByLocationIdAsync(
            int locationId)
        {
            return await _context.Shelves
                .AsNoTracking()
                .Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId &&
                        l.ShelfId == x.Shelf_Id))
                .OrderBy(x => x.Shelf_Name)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Rack_Id,

                    RackName = x.Rack != null
                        ? x.Rack.Rack_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l =>
                            l.LocationId == locationId &&
                            l.ShelfId == x.Shelf_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l =>
                            l.LocationId == locationId &&
                            l.ShelfId == x.Shelf_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .ToListAsync();
        }

        // =====================================================
        // GET ENTITY BY LOCATION
        // =====================================================

        public async Task<Shelf?> GetEntityByLocationIdAsync(
            int locationId)
        {
            return await _context.Shelves
                .FirstOrDefaultAsync(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId &&
                        l.ShelfId == x.Shelf_Id));
        }

        // =====================================================
        // GET ENTITY BY BIN
        // =====================================================

        public async Task<Shelf?> GetEntityByBinIdAsync(
            int binId)
        {
            return await _context.Shelves
                .FirstOrDefaultAsync(x =>
                    _context.Locations.Any(l =>
                        l.BinId == binId &&
                        l.ShelfId == x.Shelf_Id));
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Shelf shelf)
        {
            await _context.Shelves.AddAsync(shelf);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Shelf shelf)
        {
            _context.Shelves.Update(shelf);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Shelf shelf)
        {
            _context.Shelves.Remove(shelf);
        }
    }
}