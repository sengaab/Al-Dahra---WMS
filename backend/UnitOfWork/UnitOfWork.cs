using whm.Models;
using whm.Repositories;
using whm.Repositories.Interfaces;

namespace whm.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext db;

        public IProductRepository Products { get; }

        public ITransactionRepository Transactions { get; }

        public IWarehouseRepository Warehouses { get; }

        public ICategoryRepository Categories { get; }

        public IUnitRepository Units { get; }

        public UnitOfWork(DataBaseContext db)
        {
            this.db = db;

            Products = new ProductRepository(db);

            Transactions = new TransactionRepository(db);

            Warehouses = new WarehouseRepository(db);

            Categories = new CategoryRepository(db);

            Units = new UnitRepository(db);
        }

        public async Task<int> SaveAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}