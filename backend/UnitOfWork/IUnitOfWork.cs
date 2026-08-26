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

        Task<int> SaveAsync();
    }
}