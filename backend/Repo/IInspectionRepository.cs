using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IInspectionRepository
    {
        Task<IEnumerable<Inspection>> GetAllAsync();

        Task<Inspection?> GetByIdAsync(int id);

        Task<Inspection?> GetByReceiptItemIdAsync(int receiptItemId);

        Task<Inspection> AddAsync(Inspection inspection);

        Task UpdateAsync(Inspection inspection);

        Task<bool> ExistsAsync(int id);
    }
}