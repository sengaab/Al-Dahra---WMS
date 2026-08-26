using whm.Data;
using whm.Repositories;
using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;

        public IStockRepository Stocks { get; }

        public IDashboardRepository Dashboard { get; }

        public IUserRepository User { get; }

        public IRoleRepository Roles { get; }

        public IDepartmentRepository Department { get; }

        public IStockTransactionRepository StockTransactions { get; }

        public IStockRequestRepository StockRequests { get; }
        public IPickListRepository PickLists { get; }


        public UnitOfWork(
            DataBaseContext context,
            IStockRepository stockRepository,
            IDashboardRepository dashboardRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IDepartmentRepository departmentRepository,
            IStockTransactionRepository stockTransactionRepository,
            IStockRequestRepository stockRequestRepository
            , IPickListRepository pickListRepository
            )
        {
            _context = context;

            Stocks = stockRepository;

            Dashboard = dashboardRepository;

            User = userRepository;

            Roles = roleRepository;

            Department = departmentRepository;

            StockTransactions = stockTransactionRepository;

            StockRequests = stockRequestRepository;
            PickLists = pickListRepository;


        }


        // =====================================================
        // SAVE CHANGES
        // =====================================================

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}