using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class PutawayRepository : IPutawayRepository
    {
        private readonly DataBaseContext _context;

        public PutawayRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =========================
        // Putaway
        // =========================

        public async Task<IEnumerable<Putaway>> GetAllAsync()
        {
            return await _context.Putaways
                .Include(p => p.Items)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Putaway?> GetByIdAsync(int id)
        {
            return await _context.Putaways
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.PutawayId == id);
        }

        public async Task<Putaway> AddAsync(
            Putaway putaway)
        {
            await _context.Putaways.AddAsync(putaway);

            return putaway;
        }

        public Task UpdateAsync(
            Putaway putaway)
        {
            _context.Putaways.Update(putaway);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Putaways
                .AnyAsync(p => p.PutawayId == id);
        }

        // =========================
        // Putaway Items
        // =========================

        public async Task<IEnumerable<PutawayItem>> GetItemsAsync(
            int putawayId)
        {
            return await _context.PutawayItems
                .Where(i => i.PutawayId == putawayId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PutawayItem?> GetItemByIdAsync(
            int putawayId,
            int itemId)
        {
            return await _context.PutawayItems
                .FirstOrDefaultAsync(i =>
                    i.PutawayItemId == itemId &&
                    i.PutawayId == putawayId);
        }

        public async Task<PutawayItem> AddItemAsync(
            PutawayItem item)
        {
            await _context.PutawayItems.AddAsync(item);

            return item;
        }

        public Task UpdateItemAsync(
            PutawayItem item)
        {
            _context.PutawayItems.Update(item);

            return Task.CompletedTask;
        }
    }
}