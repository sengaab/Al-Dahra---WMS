using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITransactionRepository Transactions { get; }

        IWarehouseRepository Warehouses { get; }

        Task<int> SaveChangesAsync();
    }
}