using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public interface IUnitOfWork
    {
        IStockRepository Stocks { get; }

        IDashboardRepository Dashboard { get; }
        IUserRepository User { get; }
        IRoleRepository Roles { get; }
        IDepartmentRepository Department { get; }
        IStockTransactionRepository StockTransactions { get; }
        IStockRequestRepository StockRequests { get; }
        IPickListRepository PickLists { get; }
        IStockIssueRepository StockIssues { get; }
        IStockTransferRepository StockTransfers { get; }
        IStockCountRepository StockCounts { get; }

        Task<int> SaveAsync();
    }
}