using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataBaseContext context;

        public RoomRepository(DataBaseContext context)
        {
            this.context = context;
        }

        // =====================================================
        // GET ALL ROOMS
        // =====================================================

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await context.Rooms
                .Include(r => r.Warehouse)
                .Include(r => r.Rows)
                .ToListAsync();
        }


        // =====================================================
        // GET ROOM BY ID
        // =====================================================

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await context.Rooms
                .Include(r => r.Warehouse)
                .Include(r => r.Rows)
                .FirstOrDefaultAsync(
                    r => r.Room_Id == id);
        }


        // =====================================================
        // GET ROOMS BY WAREHOUSE
        // =====================================================

        public async Task<IEnumerable<Room>>
            GetByWarehouseIdAsync(int warehouseId)
        {
            return await context.Rooms
                .Where(r =>
                    r.Warehouse_Id == warehouseId)
                .ToListAsync();
        }


        // =====================================================
        // CHECK ROOM EXISTS
        // =====================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Rooms
                .AnyAsync(r =>
                    r.Room_Id == id);
        }


        // =====================================================
        // CHECK NAME IN SAME WAREHOUSE
        // =====================================================

        public async Task<bool>
            NameExistsInWarehouseAsync(
                string name,
                int warehouseId)
        {
            return await context.Rooms
                .AnyAsync(r =>
                    r.Room_Name == name &&
                    r.Warehouse_Id == warehouseId);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Room room)
        {
            await context.Rooms.AddAsync(room);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Room room)
        {
            context.Rooms.Update(room);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Room room)
        {
            context.Rooms.Remove(room);
        }
    }
}