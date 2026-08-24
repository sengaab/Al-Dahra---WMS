using whm.Repositories;
using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        ITransactionRepository Transactions { get; }

        IWarehouseRepository Warehouses { get; }

        ICategoryRepository Categories { get; }
        ISubCategoryRepository SubCategories { get; }

        IUnitRepository Units { get; }
        IUserRepository User { get; }
        IRoleRepository Roles { get; }
        IRowRepository Rows { get; }

        IShelfRepository Shelves { get; }

        IBinRepository Bins { get; }
        IRoomRepository Rooms { get; }
        IStockRepository Stocks { get; }
        ISiteRepository Sites { get; }
        IDepartmentRepository Departments { get; }
        IDashboardRepository Dashboard { get; }
        ISupplierRepository Suppliers { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }

        Task<int> SaveAsync();
    }
}