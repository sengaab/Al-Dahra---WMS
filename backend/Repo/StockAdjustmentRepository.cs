using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockAdjustmentRepository
        : IStockAdjustmentRepository
    {
        private readonly DataBaseContext _context;

        public StockAdjustmentRepository(
            DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockAdjustment>> GetAllAsync()
        {
            return await _context.StockAdjustments
                .AsNoTracking()
                .OrderByDescending(x => x.AdjustmentId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockAdjustment?> GetByIdAsync(int id)
        {
            return await _context.StockAdjustments
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.AdjustmentId == id);
        }


        // =====================================================
        // GET FOR UPDATE
        // =====================================================

        public async Task<StockAdjustment?> GetByIdForUpdateAsync(
            int id)
        {
            return await _context.StockAdjustments
                .FirstOrDefaultAsync(x =>
                    x.AdjustmentId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(
            StockAdjustment adjustment)
        {
            await _context.StockAdjustments
                .AddAsync(adjustment);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(
            StockAdjustment adjustment)
        {
            _context.StockAdjustments
                .Update(adjustment);
        }


        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.StockAdjustments
                .AnyAsync(x =>
                    x.AdjustmentId == id);
        }
    }
}