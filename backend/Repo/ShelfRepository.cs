using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly DataBaseContext context;

        public ShelfRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Shelf>> GetAllAsync()
        {
            return await context.Shelves
                .Include(s => s.Row)
                .Include(s => s.Bins)
                .ToListAsync();
        }

        public async Task<Shelf?> GetByIdAsync(int id)
        {
            return await context.Shelves
                .Include(s => s.Row)
                .Include(s => s.Bins)
                .FirstOrDefaultAsync(
                    s => s.Shelf_Id == id);
        }

        public async Task<IEnumerable<Shelf>> GetByRowIdAsync(
            int rowId)
        {
            return await context.Shelves
                .Where(s => s.Row_Id == rowId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Shelves
                .AnyAsync(s => s.Shelf_Id == id);
        }

        public async Task<bool> NameExistsInRowAsync(
            string name,
            int rowId)
        {
            return await context.Shelves
                .AnyAsync(s =>
                    s.Shelf_Name == name &&
                    s.Row_Id == rowId);
        }

        public async Task AddAsync(Shelf shelf)
        {
            await context.Shelves.AddAsync(shelf);
        }

        public void Update(Shelf shelf)
        {
            context.Shelves.Update(shelf);
        }

        public void Delete(Shelf shelf)
        {
            context.Shelves.Remove(shelf);
        }
    }
}