using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataBaseContext _context;

        public DashboardRepository(DataBaseContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProductsQuery()
        {
            return _context.Products
                .AsNoTracking();
        }

        public IQueryable<Stock> GetStocksQuery()
        {
            return _context.Stocks
                .AsNoTracking()
                .Where(s => s.IsActive);
        }

        public IQueryable<Warehouse> GetWarehousesQuery()
        {
            return _context.Warehouses
                .AsNoTracking()
                .Where(w => w.IsActive);
        }

        public IQueryable<Operations> GetTransactionsQuery()
        {
            return _context.Operations
                .AsNoTracking();
        }
    }
}