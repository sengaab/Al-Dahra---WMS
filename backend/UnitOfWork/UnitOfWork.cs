using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext db;

        public ITransactionRepository Transactions { get; }

        public IWarehouseRepository Warehouses { get; }

        public UnitOfWork(
            DataBaseContext db)
        {
            this.db = db;

            Transactions =
                new TransactionRepository(db);

            Warehouses =
                new WarehouseRepository(db);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}