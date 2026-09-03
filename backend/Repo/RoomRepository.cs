using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Room;
using whm.Models;

namespace whm.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataBaseContext _context;

        public RoomRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<RoomDto>> GetAllAsync(
            int? warehouseId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Rooms
                .AsNoTracking()
                .AsQueryable();

            // -------------------------------------------------
            // FILTER BY WAREHOUSE
            // -------------------------------------------------

            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.Warehouse_Id == warehouseId.Value);
            }

            // -------------------------------------------------
            // FILTER BY LOCATION
            // -------------------------------------------------

            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId.Value &&
                        l.RoomId == x.Room_Id));
            }

            // -------------------------------------------------
            // SEARCH
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Room_Name.Contains(search) ||
                    (x.Room_Code != null &&
                     x.Room_Code.Contains(search)));
            }

            // -------------------------------------------------
            // STATUS
            // -------------------------------------------------

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

            // -------------------------------------------------
            // PAGINATION
            // -------------------------------------------------

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            // -------------------------------------------------
            // RESULT
            // -------------------------------------------------

            return await query
                .OrderBy(x => x.Room_Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RoomDto
                {
                    RoomId = x.Room_Id,

                    WarehouseId = x.Warehouse_Id,

                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    Code = x.Room_Code ?? string.Empty,

                    Name = x.Room_Name,

                    Description = x.Room_Description,

                    IsActive = x.IsActive,

                    RackCount = x.Racks.Count
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .AsNoTracking()
                .Where(x => x.Room_Id == id)
                .Select(x => new RoomDto
                {
                    RoomId = x.Room_Id,

                    WarehouseId = x.Warehouse_Id,

                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    Code = x.Room_Code ?? string.Empty,

                    Name = x.Room_Name,

                    Description = x.Room_Description,

                    IsActive = x.IsActive,

                    RackCount = x.Racks.Count
                })
                .FirstOrDefaultAsync();
        }


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        public async Task<Room?> GetEntityByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(x =>
                    x.Room_Id == id);
        }


        // =====================================================
        // GET BY WAREHOUSE
        // =====================================================

        public async Task<IEnumerable<RoomDto>> GetByWarehouseIdAsync(
            int warehouseId)
        {
            return await _context.Rooms
                .AsNoTracking()
                .Where(x =>
                    x.Warehouse_Id == warehouseId)
                .OrderBy(x => x.Room_Name)
                .Select(x => new RoomDto
                {
                    RoomId = x.Room_Id,

                    WarehouseId = x.Warehouse_Id,

                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    Code = x.Room_Code ?? string.Empty,

                    Name = x.Room_Name,

                    Description = x.Room_Description,

                    IsActive = x.IsActive,

                    RackCount = x.Racks.Count
                })
                .ToListAsync();
        }


        // =====================================================
        // GET BY LOCATION
        // =====================================================

        public async Task<IEnumerable<RoomDto>> GetByLocationIdAsync(
            int locationId)
        {
            return await _context.Rooms
                .AsNoTracking()
                .Where(x =>
                    _context.Locations.Any(l =>
                        l.LocationId == locationId &&
                        l.RoomId == x.Room_Id))
                .OrderBy(x => x.Room_Name)
                .Select(x => new RoomDto
                {
                    RoomId = x.Room_Id,

                    WarehouseId = x.Warehouse_Id,

                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,

                    Code = x.Room_Code ?? string.Empty,

                    Name = x.Room_Name,

                    Description = x.Room_Description,

                    IsActive = x.IsActive,

                    RackCount = x.Racks.Count
                })
                .ToListAsync();
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Room room)
        {
            _context.Rooms.Update(room);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
        }
    }
}