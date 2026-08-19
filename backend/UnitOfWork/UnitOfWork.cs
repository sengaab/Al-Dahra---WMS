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

        public ISubCategoryRepository SubCategories { get; }
        public IUserRepository User { get; }
        public IRoleRepository Roles { get; }
        public IRowRepository Rows { get; }

        public IShelfRepository Shelves { get; }

        public IBinRepository Bins { get; }
        public IRoomRepository Rooms { get; }


        public UnitOfWork(DataBaseContext db)
        {
            this.db = db;

            Products = new ProductRepository(db);

            Transactions = new TransactionRepository(db);

            Warehouses = new WarehouseRepository(db);

            Categories = new CategoryRepository(db);

            Units = new UnitRepository(db);

            SubCategories = new SubCategoryRepository(db);
            User = new UserRepository(db);
            Roles = new RoleRepository(db);
            Rows = new RowRepository(db);

            Shelves = new ShelfRepository(db);

            Bins = new BinRepository(db);
            Rooms= new RoomRepository(db);
        }


        public async Task<int> SaveAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}