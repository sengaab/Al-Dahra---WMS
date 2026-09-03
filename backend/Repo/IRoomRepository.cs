using whm.DTOs.Room;
using whm.Models;

namespace whm.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<RoomDto>> GetAllAsync(
            int? warehouseId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<RoomDto?> GetByIdAsync(int id);

        Task<Room?> GetEntityByIdAsync(int id);

        Task<IEnumerable<RoomDto>> GetByWarehouseIdAsync(
            int warehouseId);

        Task<IEnumerable<RoomDto>> GetByLocationIdAsync(
            int locationId);

        Task AddAsync(Room room);

        void Update(Room room);

        void Delete(Room room);
    }
}