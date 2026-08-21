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

        // =========================================================
        // GET ALL
        // =========================================================

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .ToListAsync();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .FirstOrDefaultAsync(
                    s => s.Stock_Id == id);
        }


        // =========================================================
        // GET BY PRODUCT ID
        // =========================================================

        public async Task<IEnumerable<Stock>>
            GetByProductIdAsync(int productId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }


        // =========================================================
        // GET BY BIN ID
        // =========================================================

        public async Task<IEnumerable<Stock>>
            GetByBinIdAsync(int binId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Bin)
                .Where(s => s.Bin_Id == binId)
                .ToListAsync();
        }


        // =========================================================
        // SEARCH BY SITE AND/OR DEPARTMENT
        //
        // Site:
        // Stock
        // → Bin
        // → Shelf
        // → Row
        // → Room
        // → Warehouse
        // → Site
        //
        // Department:
        // Stock
        // → Product
        // → Category
        // → Department
        // =========================================================

        public async Task<List<Stock>>
            SearchBySiteAndDepartmentAsync(
                int? siteId,
                int? departmentId)
        {
            var query = _context.Stocks
                .AsNoTracking()
                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)
                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(s => s.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(r => r.Warehouse)
                .AsQueryable();


            // =====================================================
            // FILTER BY SITE
            // =====================================================

            if (siteId.HasValue)
            {
                query = query.Where(stock =>
                    stock.Bin.Shelf.Row.Room.Warehouse.Site_Id
                        == siteId.Value);
            }


            // =====================================================
            // FILTER BY DEPARTMENT
            // =====================================================

            if (departmentId.HasValue)
            {
                query = query.Where(stock =>
                    stock.Product.Category.Department_Id
                        == departmentId.Value);
            }


            // =====================================================
            // RETURN
            // =====================================================

            return await query
                .OrderBy(stock => stock.Stock_Id)
                .ToListAsync();
        }


        // =========================================================
        // ADD
        // =========================================================

        public async Task AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public void Update(Stock stock)
        {
            _context.Stocks.Update(stock);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public void Delete(Stock stock)
        {
            _context.Stocks.Remove(stock);
        }
    }
}