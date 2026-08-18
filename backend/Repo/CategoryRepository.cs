using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataBaseContext db;

        public CategoryRepository(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<List<Categories>> GetAllAsync()
        {
            return await db.Categories
                .Include(c => c.Department)
                .OrderBy(c => c.Category_Name)
                .ToListAsync();
        }

        public async Task<Categories?> GetByIdAsync(int id)
        {
            return await db.Categories
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Category_Id == id);
        }

        public async Task<List<Categories>> GetByDepartmentIdAsync(
            int departmentId)
        {
            return await db.Categories
                .Where(c => c.Department_Id == departmentId)
                .OrderBy(c => c.Category_Name)
                .ToListAsync();
        }

        public async Task<Categories?> GetByNameAsync(string name)
        {
            return await db.Categories
                .FirstOrDefaultAsync(c =>
                    c.Category_Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Categories category)
        {
            await db.Categories.AddAsync(category);
        }

        public void Update(Categories category)
        {
            db.Categories.Update(category);
        }

        public void Delete(Categories category)
        {
            db.Categories.Remove(category);
        }
    }
}