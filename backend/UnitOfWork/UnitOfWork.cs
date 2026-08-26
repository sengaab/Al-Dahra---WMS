using whm.Repositories;
using whm.Repositories.Interfaces;
using whm.Models;
using whm.Data;

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


        public UnitOfWork(
            DataBaseContext context,
            IStockRepository stockRepository,
            IDashboardRepository dashboardRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IDepartmentRepository departmentRepository
            )
        {
            _context = context;

            Stocks = stockRepository;

            Dashboard = dashboardRepository;
            User = userRepository;
            Roles = roleRepository;
            Department = departmentRepository;
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}