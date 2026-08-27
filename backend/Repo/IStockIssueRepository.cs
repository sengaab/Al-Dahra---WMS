using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockIssueRepository
    {
        Task<List<StockIssue>> GetAllAsync();

        Task<StockIssue?> GetByIdAsync(int issueId);

        Task<StockIssue?> GetByIdForUpdateAsync(int issueId);

        Task AddAsync(StockIssue stockIssue);

        void Update(StockIssue stockIssue);

        Task<bool> ExistsAsync(int issueId);
    }
}