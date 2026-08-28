using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataBaseContext _context;

        public DepartmentRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL DEPARTMENTS
        // =====================================================

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Users)
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();
        }


        // =====================================================
        // GET DEPARTMENT BY ID
        // =====================================================

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }


        // =====================================================
        // GET BY NAME
        // =====================================================

        public async Task<Department?> GetByNameAsync(
            string name)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d =>
                    d.Name.ToLower() == name.ToLower());
        }


        // =====================================================
        // CHECK NAME
        // =====================================================

        public async Task<bool> NameExistsAsync(
            string name,
            int? excludeDepartmentId = null)
        {
            return await _context.Departments
                .AnyAsync(d =>
                    d.Name.ToLower() == name.ToLower() &&
                    (!excludeDepartmentId.HasValue ||
                     d.DepartmentId !=
                     excludeDepartmentId.Value));
        }


        // =====================================================
        // CHECK CODE
        // =====================================================

        public async Task<bool> CodeExistsAsync(
            string code,
            int? excludeDepartmentId = null)
        {
            return await _context.Departments
                .AnyAsync(d =>
                    
                    (!excludeDepartmentId.HasValue ||
                     d.DepartmentId !=
                     excludeDepartmentId.Value));
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(
            Department department)
        {
            await _context.Departments.AddAsync(department);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Department department)
        {
            _context.Departments.Update(department);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Department department)
        {
            _context.Departments.Remove(department);
        }


        // =====================================================
        // GET USERS
        // =====================================================

        public async Task<List<User>> GetUsersAsync(
            int departmentId)
        {
            return await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .Include(u => u.Role)
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .ToListAsync();
        }


        // =====================================================
        // GET REQUESTS
        // =====================================================

        public async Task<List<StockRequest>> GetRequestsAsync(
            int departmentId)
        {
            return await _context.StockRequests
                .Where(r => r.DepartmentId == departmentId)
                .Include(r => r.Requester)
                .AsNoTracking()
               
                .ToListAsync();
        }
    }
}