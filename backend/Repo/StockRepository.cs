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
                .AsNoTracking()

                // =================================================
                // PRODUCT
                // =================================================

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                // =================================================
                // LOCATION
                //
                // Bin
                //   ↓
                // Shelf
                //   ↓
                // Row
                //   ↓
                // Room
                //   ↓
                // Warehouse
                // =================================================

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                // =================================================
                // UNIT
                // =================================================

                .Include(s => s.Units)

                .OrderByDescending(s => s.Stock_Id)

                .ToListAsync();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .AsNoTracking()

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                .Include(s => s.Units)

                .FirstOrDefaultAsync(
                    s => s.Stock_Id == id);
        }


        // =========================================================
        // GET BY ID FOR UPDATE
        // =========================================================

        public async Task<Stock?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(
                    s => s.Stock_Id == id);
        }


        // =========================================================
        // GET BY PRODUCT ID
        // =========================================================

        public async Task<IEnumerable<Stock>> GetByProductIdAsync(
            int productId)
        {
            return await _context.Stocks
                .AsNoTracking()

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                .Include(s => s.Units)

                .Where(s => s.ProductId == productId)

                .OrderByDescending(s => s.Stock_Id)

                .ToListAsync();
        }


        // =========================================================
        // GET BY BIN ID
        // =========================================================

        public async Task<IEnumerable<Stock>> GetByBinIdAsync(
            int binId)
        {
            return await _context.Stocks
                .AsNoTracking()

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                .Include(s => s.Units)

                .Where(s => s.Bin_Id == binId)

                .OrderByDescending(s => s.Stock_Id)

                .ToListAsync();
        }


        // =========================================================
        // SEARCH BY SITE AND DEPARTMENT
        // =========================================================

        public async Task<List<Stock>> SearchBySiteAndDepartmentAsync(
            int? siteId,
            int? departmentId)
        {
            var query = _context.Stocks
                .AsNoTracking()

                // =================================================
                // PRODUCT
                // =================================================

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                // =================================================
                // LOCATION
                // =================================================

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                // =================================================
                // UNIT
                // =================================================

                .Include(s => s.Units)

                .AsQueryable();


            // =====================================================
            // FILTER BY SITE
            // =====================================================

            if (siteId.HasValue)
            {
                query = query.Where(stock =>
                    stock.Bin != null &&
                    stock.Bin.Shelf != null &&
                    stock.Bin.Shelf.Row != null &&
                    stock.Bin.Shelf.Row.Room != null &&
                    stock.Bin.Shelf.Row.Room.Warehouse != null &&
                    stock.Bin.Shelf.Row.Room.Warehouse.Site_Id
                        == siteId.Value);
            }


            // =====================================================
            // FILTER BY DEPARTMENT
            // =====================================================

            if (departmentId.HasValue)
            {
                query = query.Where(stock =>
                    stock.Product != null &&
                    stock.Product.Category != null &&
                    stock.Product.Category.Department_Id
                        == departmentId.Value);
            }


            return await query
                .OrderByDescending(s => s.Stock_Id)
                .ToListAsync();
        }


        // =========================================================
        // GET INVENTORY WITH PAGINATION
        // =========================================================

        public async Task<(List<Stock> Stocks, int TotalCount)>
            GetInventoryAsync(
                int pageNumber,
                int pageSize)
        {
            var query = _context.Stocks
                .AsNoTracking()

                // =================================================
                // PRODUCT
                // =================================================

                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)

                // =================================================
                // LOCATION
                // =================================================

                .Include(s => s.Bin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(sh => sh.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(room => room.Warehouse)

                // =================================================
                // UNIT
                // =================================================

                .Include(s => s.Units)

                .AsQueryable();


            // =====================================================
            // TOTAL COUNT
            // =====================================================

            var totalCount =
                await query.CountAsync();


            // =====================================================
            // PAGINATION
            // =====================================================

            var stocks =
                await query
                    .OrderByDescending(
                        s => s.LastUpdatedAt)

                    .Skip(
                        (pageNumber - 1) * pageSize)

                    .Take(pageSize)

                    .ToListAsync();


            return (
                Stocks: stocks,
                TotalCount: totalCount
            );
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