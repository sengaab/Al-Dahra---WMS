using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Operations?> GetByIdAsync(int id);

        Task<List<Operations>> GetAllAsync();

        Task<List<Operations>> GetByProductIdAsync(int productId);

        Task<List<Operations>> GetByBinIdAsync(int binId);

        Task<List<Operations>> GetByUserIdAsync(Guid userId);

        Task<List<Operations>> GetByTypeAsync(OperationType type);

        Task AddAsync(Operations transaction);
        Task<List<Operations>> SearchBySiteAndDepartmentAsync( int? siteId, int? departmentId);
    }
}