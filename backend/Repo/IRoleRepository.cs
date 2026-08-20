using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();

        Task<Role?> GetByIdAsync(int id);

        Task<Role?> GetByNameAsync(string name);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsAsync(string name);

        Task AddAsync(Role role);

        void Update(Role role);

        void Delete(Role role);
    }
}