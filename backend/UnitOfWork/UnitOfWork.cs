using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;

        public IStockRepository Stocks { get; }

        public IDashboardRepository Dashboard { get; }


        public UnitOfWork(
            DataBaseContext context,
            IStockRepository stockRepository,
            IDashboardRepository dashboardRepository)
        {
            _context = context;

            Stocks = stockRepository;

            Dashboard = dashboardRepository;
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}