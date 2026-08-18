using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        ITransactionRepository Transactions { get; }

        IWarehouseRepository Warehouses { get; }

        ICategoryRepository Categories { get; }

        IUnitRepository Units { get; }

        Task<int> SaveAsync();
    }
}