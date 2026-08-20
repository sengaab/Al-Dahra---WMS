using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataBaseContext context;

        public RoleRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await context.Roles
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await context.Roles
                .Include(r => r.User)
                .FirstOrDefaultAsync(
                    r => r.Role_Id == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await context.Roles
                .FirstOrDefaultAsync(
                    r => r.Role_Name == name);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Roles
                .AnyAsync(r => r.Role_Id == id);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await context.Roles
                .AnyAsync(r => r.Role_Name == name);
        }

        public async Task AddAsync(Role role)
        {
            await context.Roles.AddAsync(role);
        }

        public void Update(Role role)
        {
            context.Roles.Update(role);
        }

        public void Delete(Role role)
        {
            context.Roles.Remove(role);
        }
    }
}