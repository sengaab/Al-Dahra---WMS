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

        // =========================================================
        // GET ALL
        // =========================================================

        public async Task<List<Categories>> GetAllAsync()
        {
            return await db.Categories
                .Include(c => c.Department)
                .OrderBy(c => c.Category_Name)
                .ToListAsync();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<Categories?> GetByIdAsync(int id)
        {
            return await db.Categories
                .Include(c => c.Department)
                .FirstOrDefaultAsync(
                    c => c.Category_Id == id);
        }


        // =========================================================
        // GET BY DEPARTMENT ID
        // =========================================================

        public async Task<List<Categories>>
            GetByDepartmentIdAsync(int departmentId)
        {
            return await db.Categories
                .Where(c =>
                    c.Department_Id == departmentId)
                .OrderBy(c => c.Category_Name)
                .ToListAsync();
        }


        // =========================================================
        // GET BY NAME
        // =========================================================

        public async Task<Categories?> GetByNameAsync(
            string name)
        {
            return await db.Categories
                .FirstOrDefaultAsync(c =>
                    c.Category_Name.ToLower()
                    == name.ToLower());
        }


        // =========================================================
        // SEARCH BY SITE AND/OR DEPARTMENT
        //
        // Department:
        // Category → Department
        //
        // Site:
        // Category
        // → Product
        // → Stock
        // → Bin
        // → Shelf
        // → Row
        // → Room
        // → Warehouse
        // → Site
        // =========================================================

        public async Task<List<Categories>>
            SearchBySiteAndDepartmentAsync(
                int? siteId,
                int? departmentId)
        {
            var query = db.Categories
                .AsNoTracking()
                .Include(c => c.Department)
                .AsQueryable();


            // =====================================================
            // FILTER BY DEPARTMENT
            // =====================================================

            if (departmentId.HasValue)
            {
                query = query.Where(c =>
                    c.Department_Id
                    == departmentId.Value);
            }


            // =====================================================
            // FILTER BY SITE
            // =====================================================

            if (siteId.HasValue)
            {
                query = query.Where(c =>
                    c.Products.Any(product =>
                        product.Stock.Any(stock =>
                            stock.Bin
                                .Shelf
                                .Row
                                .Room
                                .Warehouse
                                .Site_Id
                            == siteId.Value
                        )
                    )
                );
            }


            // =====================================================
            // RETURN
            // =====================================================

            return await query
                .OrderBy(c => c.Category_Name)
                .ToListAsync();
        }


        // =========================================================
        // ADD
        // =========================================================

        public async Task AddAsync(Categories category)
        {
            await db.Categories.AddAsync(category);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public void Update(Categories category)
        {
            db.Categories.Update(category);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public void Delete(Categories category)
        {
            db.Categories.Remove(category);
        }
    }
}