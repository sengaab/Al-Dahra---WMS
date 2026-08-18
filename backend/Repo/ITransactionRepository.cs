using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);

        Task<List<Transaction>> GetAllAsync();

        Task<List<Transaction>> GetByProductIdAsync(int productId);

        Task<List<Transaction>> GetByBinIdAsync(int binId);

        Task<List<Transaction>> GetByUserIdAsync(Guid userId);

        Task<List<Transaction>> GetByTypeAsync(TransactionType type);

        Task AddAsync(Transaction transaction);
    }
}