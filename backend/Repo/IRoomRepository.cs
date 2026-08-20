using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();

        Task<Room?> GetByIdAsync(int id);

        Task<IEnumerable<Room>> GetByWarehouseIdAsync(
            int warehouseId);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsInWarehouseAsync(
            string name,
            int warehouseId);

        Task AddAsync(Room room);

        void Update(Room room);

        void Delete(Room room);
    }
}