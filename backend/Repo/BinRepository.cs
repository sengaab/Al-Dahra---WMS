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
            int? shelfId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Bins
                .AsNoTracking()
                .Include(x => x.Shelf)
                .Include(x => x.Location)
                .Include(x => x.Stocks)
                .AsQueryable();


            // =====================================================
            // FILTER BY SHELF
            // =====================================================

            if (shelfId.HasValue)
            {
                query = query.Where(x =>
                    x.Shelf_Id == shelfId.Value);
            }


            // =====================================================
            // FILTER BY LOCATION
            // =====================================================

            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    x.LocationId == locationId.Value);
            }


            // =====================================================
            // SEARCH
            // =====================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Bin_Code != null &&
                    x.Bin_Code.Contains(search)
                    ||
                    x.Bin_Name.Contains(search));
            }


            // =====================================================
            // STATUS
            // =====================================================

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


            // =====================================================
            // RESULT
            // =====================================================

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

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

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

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Bin?> GetEntityByIdAsync(int id)
        {
            return await _context.Bins
                .FirstOrDefaultAsync(x => x.Bin_Id == id);
        }


        // =====================================================
        // GET BY SHELF
        // =====================================================

        public async Task<IEnumerable<BinDto>> GetByShelfIdAsync(
            int shelfId)
        {
            return await _context.Bins
                .AsNoTracking()
                .Where(x => x.Shelf_Id == shelfId)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .OrderBy(x => x.Name)
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
                .Where(x => x.LocationId == locationId)
                .Select(x => new BinDto
                {
                    BinId = x.Bin_Id,

                    ShelfId = x.Shelf_Id,

                    ShelfName = x.Shelf != null
                        ? x.Shelf.Shelf_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Bin_Code ?? string.Empty,

                    Name = x.Bin_Name,

                    IsActive = x.IsActive,

                    StockCount = x.Stocks.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
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