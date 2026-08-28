using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockTransferRepository : IStockTransferRepository
    {
        private readonly DataBaseContext _context;

        public StockTransferRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockTransfer>> GetAllAsync()
        {
            return await _context.StockTransfers
                .AsNoTracking()
                .Include(x => x.Items)
                .OrderByDescending(x => x.TransferId)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockTransfer?> GetByIdAsync(int id)
        {
            return await _context.StockTransfers
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.TransferId == id);
        }


        // =====================================================
        // GET FOR UPDATE
        // =====================================================

        public async Task<StockTransfer?> GetByIdForUpdateAsync(int id)
        {
            return await _context.StockTransfers
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.TransferId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(StockTransfer transfer)
        {
            await _context.StockTransfers.AddAsync(transfer);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(StockTransfer transfer)
        {
            _context.StockTransfers.Update(transfer);
        }


        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.StockTransfers
                .AnyAsync(x =>
                    x.TransferId == id);
        }
    }
}