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

        public async Task<IEnumerable<BinDto>> GetAllAsync(
            int? shelfId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Bins
                .AsNoTracking()
                .AsQueryable();

            // Filter by Shelf
            if (shelfId.HasValue)
            {
                query = query.Where(x =>
                    x.Shelf_Id == shelfId.Value);
            }

            // Filter by Location
            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId.Value &&
                        l.BinId == x.Bin_Id));
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    (x.Bin_Code != null &&
                     x.Bin_Code.Contains(search))
                    ||
                    x.Bin_Name.Contains(search));
            }

            // Status
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

            // Pagination
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            return await query
                .OrderBy(x => x.Bin_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        public async Task<BinDto?> GetByIdAsync(int id)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x => x.Bin_Id == id)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Bin?> GetEntityByIdAsync(int id)
        {
            return await _context.Bins
                .FirstOrDefaultAsync(x =>
                    x.Bin_Id == id);
        }

        public async Task<IEnumerable<BinDto>> GetByShelfIdAsync(
            int shelfId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    x.Shelf_Id == shelfId)
                .OrderBy(x => x.Bin_Name)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l => l.BinId == x.Bin_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BinDto>> GetByLocationIdAsync(
            int locationId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId &&
                        l.BinId == x.Bin_Id))
                .OrderBy(x => x.Bin_Name)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = _context.Locations
                        .Where(l =>
                            l.LocationId == locationId &&
                            l.BinId == x.Bin_Id)
                        .Select(l => (int?)l.LocationId)
                        .FirstOrDefault(),

                    LocationName = _context.Locations
                        .Where(l =>
                            l.LocationId == locationId &&
                            l.BinId == x.Bin_Id)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .ToListAsync();
        }

        public async Task AddAsync(Bin bin)
        {
            await _context.Bins.AddAsync(bin);
        }

        public void Update(Bin bin)
        {
            _context.Bins.Update(bin);
        }

        public void Delete(Bin bin)
        {
            _context.Bins.Remove(bin);
        }
    }
}