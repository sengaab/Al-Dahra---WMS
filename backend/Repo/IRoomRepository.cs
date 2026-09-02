using whm.DTOs.Room;
using whm.Models;

namespace whm.Repositories
{
    public interface IRoomRepository
    {
        // GET ALL
        Task<IEnumerable<RoomDto>> GetAllAsync(
            int? warehouseId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        // GET BY ID
        Task<RoomDto?> GetByIdAsync(int id);

        // GET ENTITY
        Task<Room?> GetEntityByIdAsync(int id);

        // GET BY WAREHOUSE
        Task<IEnumerable<RoomDto>> GetByWarehouseIdAsync(
            int warehouseId);

        // GET BY LOCATION
        Task<IEnumerable<RoomDto>> GetByLocationIdAsync(
            int locationId);

        // ADD
        Task AddAsync(Room room);

        // UPDATE
        void Update(Room room);

        // DELETE
        void Delete(Room room);
    }
}