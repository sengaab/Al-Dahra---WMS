using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL USERS
        // =====================================================

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .ToListAsync();
        }


        // =====================================================
        // GET USER BY ID
        // =====================================================

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }


        // =====================================================
        // GET USER BY EMAIL
        // =====================================================

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        // =====================================================
        // CHECK EMAIL
        // =====================================================

        public async Task<bool> EmailExistsAsync(
            string email,
            Guid? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Email == email &&
                    (!excludeUserId.HasValue ||
                     u.UserId != excludeUserId.Value));
        }


        // =====================================================
        // CHECK EMPLOYEE CODE
        // =====================================================

        public async Task<bool> EmployeeCodeExistsAsync(
            string employeeCode,
            Guid? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.EmployeeCode == employeeCode &&
                    (!excludeUserId.HasValue ||
                     u.UserId != excludeUserId.Value));
        }


        // =====================================================
        // USER ACTIVITY
        // =====================================================

        public async Task<List<AuditLog>> GetActivityAsync(Guid userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }


        // =====================================================
        // USER + ROLE
        // =====================================================

        public async Task<User?> GetUserWithPermissionsAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(User user)
        {
            _context.Users.Update(user);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
    }
}