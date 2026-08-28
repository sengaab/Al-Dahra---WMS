using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockCountRepository : IStockCountRepository
    {
        private readonly DataBaseContext _context;

        public StockCountRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockCount>> GetAllAsync()
        {
            return await _context.StockCounts
                .AsNoTracking()
                .Include(x => x.Items)
                .OrderByDescending(x => x.StockCountId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockCount?> GetByIdAsync(int id)
        {
            return await _context.StockCounts
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.StockCountId == id);
        }


        // =====================================================
        // GET FOR UPDATE
        // =====================================================

        public async Task<StockCount?> GetByIdForUpdateAsync(int id)
        {
            return await _context.StockCounts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.StockCountId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(StockCount stockCount)
        {
            await _context.StockCounts.AddAsync(stockCount);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(StockCount stockCount)
        {
            _context.StockCounts.Update(stockCount);
        }


        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.StockCounts
                .AnyAsync(x =>
                    x.StockCountId == id);
        }
    }
}