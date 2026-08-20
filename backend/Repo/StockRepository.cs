using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly DataBaseContext _context;

        public StockRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .FirstOrDefaultAsync(s => s.Stock_Id == id);
        }

        public async Task<IEnumerable<Stock>> GetByProductIdAsync(int productId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetByBinIdAsync(int binId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .Where(s => s.Bin_Id == binId)
                .ToListAsync();
        }

        public async Task AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
        }

        public void Update(Stock stock)
        {
            _context.Stocks.Update(stock);
        }

        public void Delete(Stock stock)
        {
            _context.Stocks.Remove(stock);
        }
    }
}