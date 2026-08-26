using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataBaseContext _context;

        public RoleRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.Users)
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }


        // =====================================================
        // GET BY NAME
        // =====================================================

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r =>
                    r.Name.ToLower() == name.ToLower());
        }


        // =====================================================
        // CHECK NAME
        // =====================================================

        public async Task<bool> NameExistsAsync(
            string name,
            int? excludeRoleId = null)
        {
            return await _context.Roles
                .AnyAsync(r =>
                    r.Name.ToLower() == name.ToLower() &&
                    (!excludeRoleId.HasValue ||
                     r.RoleId != excludeRoleId.Value));
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
        }


        // =====================================================
        // GET ROLE + PERMISSIONS
        // =====================================================

        public async Task<Role?> GetRoleWithPermissionsAsync(
            int roleId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId);
        }


        // =====================================================
        // UPDATE PERMISSIONS
        // =====================================================

        public async Task UpdatePermissionsAsync(
            int roleId,
            List<string> permissions)
        {
            // This method should be connected to your
            // actual Role-Permission table when it exists.

            // For now this intentionally does nothing because
            // the Role model/table structure provided does not
            // contain a permissions collection/table.
            await Task.CompletedTask;
        }
    }
}