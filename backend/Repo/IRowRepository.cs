using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IRowRepository
    {
        Task<IEnumerable<Row>> GetAllAsync();

        Task<Row?> GetByIdAsync(int id);

        Task<IEnumerable<Row>> GetByRoomIdAsync(int roomId);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsInRoomAsync(
            string name,
            int roomId);

        Task AddAsync(Row row);

        void Update(Row row);

        void Delete(Row row);
    }
}