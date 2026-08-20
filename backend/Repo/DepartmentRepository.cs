using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataBaseContext db;

        public DepartmentRepository(DataBaseContext db)
        {
            this.db = db;
        }


        public async Task<List<Department>> GetAllAsync()
        {
            return await db.Departments
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Department?> GetByIdAsync(int id)
        {
            return await db.Departments
                .FirstOrDefaultAsync(
                    d => d.Department_Id == id);
        }


        public async Task<Department?> GetByNameAsync(
            string name)
        {
            return await db.Departments
                .FirstOrDefaultAsync(
                    d => d.Department_Name == name);
        }


        public async Task AddAsync(
            Department department)
        {
            await db.Departments
                .AddAsync(department);
        }


        public void Update(
            Department department)
        {
            db.Departments.Update(department);
        }


        public void Delete(
            Department department)
        {
            db.Departments.Remove(department);
        }
    }
}