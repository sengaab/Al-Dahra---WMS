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
                .Include(x => x.Row)
                .Include(x => x.Location)
                .Include(x => x.Bins)
                .AsQueryable();


            // =================================================
            // FILTER BY RACK
            // =================================================

            if (rackId.HasValue)
            {
                query = query.Where(x =>
                    x.Row_Id == rackId.Value);
            }


            // =================================================
            // FILTER BY LOCATION
            // =================================================

            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    x.LocationId == locationId.Value);
            }


            // =================================================
            // SEARCH
            // =================================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Shelf_Code != null &&
                    x.Shelf_Code.Contains(search)
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
            // RESULT
            // =================================================

            return await query
                .OrderBy(x => x.Shelf_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Row_Id,

                    RackName = x.Row != null
                        ? x.Row.Rack_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

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

                    RackId = x.Row_Id,

                    RackName = x.Row != null
                        ? x.Row.Rack_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

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
                .Where(x => x.Row_Id == rackId)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Row_Id,

                    RackName = x.Row != null
                        ? x.Row.Rack_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .OrderBy(x => x.Name)
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
                .Where(x => x.LocationId == locationId)
                .Select(x => new ShelfDto
                {
                    ShelfId = x.Shelf_Id,

                    RackId = x.Row_Id,

                    RackName = x.Row != null
                        ? x.Row.Rack_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Shelf_Code ?? string.Empty,

                    Name = x.Shelf_Name,

                    IsActive = x.IsActive,

                    BinCount = x.Bins.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
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