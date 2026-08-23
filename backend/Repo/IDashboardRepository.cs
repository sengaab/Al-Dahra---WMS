using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        IQueryable<Product> GetProductsQuery();

        IQueryable<Stock> GetStocksQuery();

        IQueryable<Warehouse> GetWarehousesQuery();

        IQueryable<Operations> GetTransactionsQuery();
    }
}