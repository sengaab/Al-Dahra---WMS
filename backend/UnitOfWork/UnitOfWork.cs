using DocumentFormat.OpenXml.Bibliography;
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
        public IStockIssueRepository StockIssues { get; }
        public IStockTransferRepository StockTransfers { get; }
        public IStockCountRepository StockCounts { get; }
        public IStockAdjustmentRepository StockAdjustments { get; }
        public IStockReturnRepository StockReturns { get; }


        public UnitOfWork(
            DataBaseContext context,
            IStockRepository stockRepository,
            IDashboardRepository dashboardRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IDepartmentRepository departmentRepository,
            IStockTransactionRepository stockTransactionRepository,
            IStockRequestRepository stockRequestRepository
            , IPickListRepository pickListRepository,
            IStockIssueRepository stockIssueRepository,
            IStockTransferRepository stockTransferRepository,
            IStockCountRepository stockCountRepository,
            IStockAdjustmentRepository stockAdjustmentRepository,
            IStockReturnRepository stockReturnRepository
            

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
            StockIssues = stockIssueRepository;
            StockTransfers = stockTransferRepository;
            StockCounts = stockCountRepository;
            StockAdjustments = stockAdjustmentRepository;
            StockReturns= stockReturnRepository;


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