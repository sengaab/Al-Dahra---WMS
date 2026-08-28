using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly DataBaseContext _context;

        public StockTransactionRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL TRANSACTIONS + FILTERS
        // =====================================================

        public async Task<List<StockTransaction>> GetAllAsync(
            int? productId = null,
            int? stockId = null,
            int? warehouseId = null,
            int? locationId = null,
            string? transactionType = null,
            DateTimeOffset? fromDate = null,
            DateTimeOffset? toDate = null,
            Guid? userId = null)
        {
            IQueryable<StockTransaction> query =
                _context.StockTransactions
                    .Include(x => x.Product)
                    .Include(x => x.Stock)
                    .Include(x => x.SourceLocation)
                    .Include(x => x.DestinationLocation)
                    .Include(x => x.Performer)
                    .AsNoTracking();

            // Product
            if (productId.HasValue)
            {
                query = query.Where(x =>
                    x.ProductId == productId.Value);
            }

            // Stock
            if (stockId.HasValue)
            {
                query = query.Where(x =>
                    x.StockId == stockId.Value);
            }

            // Warehouse
            if (warehouseId.HasValue)
            {
                query = query.Where(x =>
                    x.Stock != null &&
                    x.Stock.WarehouseId == warehouseId.Value);
            }

            // Location
            if (locationId.HasValue)
            {
                query = query.Where(x =>
                    x.SourceLocationId == locationId.Value ||
                    x.DestinationLocationId == locationId.Value);
            }

            // Transaction Type
            if (!string.IsNullOrWhiteSpace(transactionType))
            {
                query = query.Where(x =>
                    x.TransactionType == transactionType);
            }

            // From Date
            if (fromDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt >= fromDate.Value);
            }

            // To Date
            if (toDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt <= toDate.Value);
            }

            // User
            if (userId.HasValue)
            {
                query = query.Where(x =>
                    x.PerformedBy == userId.Value);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }


        // =====================================================
        // GET TRANSACTION BY ID
        // =====================================================

        public async Task<StockTransaction?> GetByIdAsync(long id)
        {
            return await _context.StockTransactions
                .Include(x => x.Product)
                .Include(x => x.Stock)
                .Include(x => x.SourceLocation)
                .Include(x => x.DestinationLocation)
                .Include(x => x.Performer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TransactionId == id);
        }
    }
}