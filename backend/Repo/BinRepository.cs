using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class BinRepository : IBinRepository
    {
        private readonly DataBaseContext context;

        public BinRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Bin>> GetAllAsync()
        {
            return await context.Bins
                .Include(b => b.Shelf)
                .Include(b => b.Stocks)
                .ToListAsync();
        }

        public async Task<Bin?> GetByIdAsync(int id)
        {
            return await context.Bins
                .Include(b => b.Shelf)
                .Include(b => b.Stocks)
                .FirstOrDefaultAsync(
                    b => b.Bin_Id == id);
        }

        public async Task<IEnumerable<Bin>> GetByShelfIdAsync(
            int shelfId)
        {
            return await context.Bins
                .Where(b => b.Shelf_Id == shelfId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Bins
                .AnyAsync(b => b.Bin_Id == id);
        }

        public async Task<bool> NameExistsInShelfAsync(
            string name,
            int shelfId)
        {
            return await context.Bins
                .AnyAsync(b =>
                    b.Bin_Name == name &&
                    b.Shelf_Id == shelfId);
        }

        public async Task AddAsync(Bin bin)
        {
            await context.Bins.AddAsync(bin);
        }

        public void Update(Bin bin)
        {
            context.Bins.Update(bin);
        }

        public void Delete(Bin bin)
        {
            context.Bins.Remove(bin);
        }
    }
}