using whm.Repositories;
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
        IStockAdjustmentRepository StockAdjustments { get; }
        IStockReturnRepository StockReturns { get; }
        IProductRepository Products { get; }
         IUnitRepository Units { get; }
        ICategoryRepository Categories { get; }
        ISupplierRepository Suppliers { get; }
         IWarehouseRepository Warehouses { get; }
        ILocationRepository Locations { get; }
     
        IBinRepository Bins { get; }
        ISiteRepository Sites { get; }
        IPurchaseOrderRepository PurchaseOrders { get; }
        IReceiptRepository ReceiptRepository { get; }
        IPartitionRepository Partitions { get; }


        Task<int> SaveAsync();
    }
}