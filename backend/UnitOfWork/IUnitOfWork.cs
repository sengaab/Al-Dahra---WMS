using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public interface IUnitOfWork
    {
        IStockRepository Stocks { get; }

        IDashboardRepository Dashboard { get; }

        Task<int> SaveAsync();
    }
}