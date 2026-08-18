using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DataBaseContext context;

        public SubCategoryRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            return await context.SubCategories
                .Include(sc => sc.Category)
                .ToListAsync();
        }

        public async Task<SubCategory?> GetByIdAsync(int id)
        {
            return await context.SubCategories
                .Include(sc => sc.Category)
                .FirstOrDefaultAsync(
                    sc => sc.SubCategoryId == id);
        }

        public async Task<IEnumerable<SubCategory>> GetByCategoryIdAsync(
            int categoryId)
        {
            return await context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task AddAsync(SubCategory subCategory)
        {
            await context.SubCategories
                .AddAsync(subCategory);
        }

        public void Update(SubCategory subCategory)
        {
            context.SubCategories
                .Update(subCategory);
        }

        public void Delete(SubCategory subCategory)
        {
            context.SubCategories
                .Remove(subCategory);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.SubCategories
                .AnyAsync(sc => sc.SubCategoryId == id);
        }
    }
}