using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IBinRepository
    {
        Task<IEnumerable<Bin>> GetAllAsync();

        Task<Bin?> GetByIdAsync(int id);

        Task<IEnumerable<Bin>> GetByShelfIdAsync(int shelfId);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsInShelfAsync(
            string name,
            int shelfId);

        Task AddAsync(Bin bin);

        void Update(Bin bin);

        void Delete(Bin bin);
    }
}