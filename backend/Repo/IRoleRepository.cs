using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        // CRUD
        Task<List<Role>> GetAllAsync();

        Task<Role?> GetByIdAsync(int id);

        Task<Role?> GetByNameAsync(string name);

        Task<bool> NameExistsAsync(
            string name,
            int? excludeRoleId = null);

        Task AddAsync(Role role);

        void Update(Role role);

        void Delete(Role role);

        // Permissions
        Task<Role?> GetRoleWithPermissionsAsync(int roleId);

        Task UpdatePermissionsAsync(
            int roleId,
            List<string> permissions);
    }
}