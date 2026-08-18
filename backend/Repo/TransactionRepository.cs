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

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .FirstOrDefaultAsync(
                    t => t.transaction_Id == id
                );
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByProductIdAsync(
            int productId)
        {
            return await db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.Product_Id == productId)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByBinIdAsync(
            int binId)
        {
            return await db.Transactions
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

        public async Task<List<Transaction>> GetByUserIdAsync(
            Guid userId)
        {
            return await db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.User_Id == userId)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByTypeAsync(
            TransactionType type)
        {
            return await db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.TransactionType == type)
                .OrderByDescending(t => t.CreateAt)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await db.Transactions.AddAsync(transaction);
        }
    }
}