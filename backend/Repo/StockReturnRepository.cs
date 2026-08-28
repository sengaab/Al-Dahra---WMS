using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockReturnRepository : IStockReturnRepository
    {
        private readonly DataBaseContext _context;

        public StockReturnRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockReturn>> GetAllAsync()
        {
            return await _context.StockReturns
                .AsNoTracking()
                .Include(x => x.Items)
                .OrderByDescending(x => x.ReturnId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockReturn?> GetByIdAsync(int id)
        {
            return await _context.StockReturns
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.ReturnId == id);
        }


        // =====================================================
        // GET FOR UPDATE
        // =====================================================

        public async Task<StockReturn?> GetByIdForUpdateAsync(int id)
        {
            return await _context.StockReturns
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.ReturnId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(StockReturn stockReturn)
        {
            await _context.StockReturns.AddAsync(stockReturn);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(StockReturn stockReturn)
        {
            _context.StockReturns.Update(stockReturn);
        }


        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.StockReturns
                .AnyAsync(x =>
                    x.ReturnId == id);
        }
    }
}