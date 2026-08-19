using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IShelfRepository
    {
        Task<IEnumerable<Shelf>> GetAllAsync();

        Task<Shelf?> GetByIdAsync(int id);

        Task<IEnumerable<Shelf>> GetByRowIdAsync(int rowId);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsInRowAsync(
            string name,
            int rowId);

        Task AddAsync(Shelf shelf);

        void Update(Shelf shelf);

        void Delete(Shelf shelf);
    }
}