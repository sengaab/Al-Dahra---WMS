using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataBaseContext db;

        public TransactionRepository(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<Operations?> GetByIdAsync(int id)
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .FirstOrDefaultAsync(
                    t => t.Operation_Id == id
                );
        }

        public async Task<List<Operations>> GetAllAsync()
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Operations>> GetByProductIdAsync(
            int productId)
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.Product_Id == productId)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Operations>> GetByBinIdAsync(
            int binId)
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t =>
                    t.FromBinId == binId ||
                    t.ToBinId == binId)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Operations>> GetByUserIdAsync(
            Guid userId)
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.User_Id == userId)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Operations>> GetByTypeAsync(
            OperationType type)
        {
            return await db.Operations
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.OperationType == type)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task AddAsync(Operations transaction)
        {
            await db.Operations.AddAsync(transaction);
        }
        public async Task<List<Operations>>
    SearchBySiteAndDepartmentAsync(
        int? siteId,
        int? departmentId)
        {
            var query = db.Operations
                .AsNoTracking()
                .Include(t => t.Product)
                    .ThenInclude(p => p.Category)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(s => s.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(r => r.Warehouse)
                .Include(t => t.ToBin)
                    .ThenInclude(b => b.Shelf)
                        .ThenInclude(s => s.Row)
                            .ThenInclude(r => r.Room)
                                .ThenInclude(r => r.Warehouse)
                .AsQueryable();


            // =====================================================
            // FILTER BY SITE
            //
            // Transaction
            // → FromBin / ToBin
            // → Shelf
            // → Row
            // → Room
            // → Warehouse
            // → Site
            // =====================================================

            if (siteId.HasValue)
            {
                query = query.Where(t =>
                    (t.FromBin != null &&
                     t.FromBin.Shelf.Row.Room.Warehouse.Site_Id
                        == siteId.Value)

                    ||

                    (t.ToBin != null &&
                     t.ToBin.Shelf.Row.Room.Warehouse.Site_Id
                        == siteId.Value)
                );
            }


            // =====================================================
            // FILTER BY DEPARTMENT
            //
            // Transaction
            // → Product
            // → Category
            // → Department
            // =====================================================

            if (departmentId.HasValue)
            {
                query = query.Where(t =>
                    t.Product.Category.Department_Id
                        == departmentId.Value);
            }


            return await query
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }
    }
}