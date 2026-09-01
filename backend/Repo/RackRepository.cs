using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Rack;
using whm.Models;

namespace whm.Repositories
{
    public class RackRepository : IRackRepository
    {
        private readonly DataBaseContext _context;

        public RackRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<RackDto>> GetAllAsync(
            int? roomId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Racks
                .AsNoTracking()
                .Include(x => x.Room)
                .Include(x => x.Location)
                .Include(x => x.Shelves)
                .AsQueryable();


            // =================================================
            // FILTER BY ROOM
            // =================================================

            if (roomId.HasValue)
            {
                query = query.Where(x =>
                    x.Room_Id == roomId.Value);
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
                    x.Rack_Code != null &&
                    x.Rack_Code.Contains(search)
                    ||
                    x.Rack_Name.Contains(search));
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
                .OrderBy(x => x.Rack_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RackDto
                {
                    RackId = x.Rack_Id,

                    RoomId = x.Room_Id,

                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Rack_Code ?? string.Empty,

                    Name = x.Rack_Name,

                    IsActive = x.IsActive,

                    ShelfCount = x.Shelves.Count
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<RackDto?> GetByIdAsync(int id)
        {
            return await _context.Racks
                .AsNoTracking()
                .Where(x => x.Rack_Id == id)
                .Select(x => new RackDto
                {
                    RackId = x.Rack_Id,

                    RoomId = x.Room_Id,

                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Rack_Code ?? string.Empty,

                    Name = x.Rack_Name,

                    IsActive = x.IsActive,

                    ShelfCount = x.Shelves.Count
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Rack?> GetEntityByIdAsync(int id)
        {
            return await _context.Racks
                .FirstOrDefaultAsync(x => x.Rack_Id == id);
        }


        // =====================================================
        // GET BY ROOM
        // =====================================================

        public async Task<IEnumerable<RackDto>> GetByRoomIdAsync(
            int roomId)
        {
            return await _context.Racks
                .AsNoTracking()
                .Where(x => x.Room_Id == roomId)
                .Select(x => new RackDto
                {
                    RackId = x.Rack_Id,

                    RoomId = x.Room_Id,

                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Rack_Code ?? string.Empty,

                    Name = x.Rack_Name,

                    IsActive = x.IsActive,

                    ShelfCount = x.Shelves.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }


        // =====================================================
        // GET BY LOCATION
        // =====================================================

        public async Task<IEnumerable<RackDto>> GetByLocationIdAsync(
            int locationId)
        {
            return await _context.Racks
                .AsNoTracking()
                .Where(x => x.LocationId == locationId)
                .Select(x => new RackDto
                {
                    RackId = x.Rack_Id,

                    RoomId = x.Room_Id,

                    RoomName = x.Room != null
                        ? x.Room.Room_Name
                        : null,

                    LocationId = x.LocationId,

                    LocationName = x.Location != null
                        ? x.Location.Name
                        : null,

                    Code = x.Rack_Code ?? string.Empty,

                    Name = x.Rack_Name,

                    IsActive = x.IsActive,

                    ShelfCount = x.Shelves.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Rack rack)
        {
            await _context.Racks.AddAsync(rack);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Rack rack)
        {
            _context.Racks.Update(rack);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Rack rack)
        {
            _context.Racks.Remove(rack);
        }
    }
}