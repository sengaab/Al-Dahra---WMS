using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<IEnumerable<Stock>> GetByProductIdAsync(
            int productId);

        Task<IEnumerable<Stock>> GetByBinIdAsync(
            int binId);

        Task<List<Stock>> SearchBySiteAndDepartmentAsync(
            int? siteId,
            int? departmentId);

        Task<IEnumerable<Stock>> GetInventoryAsync();

        Task AddAsync(Stock stock);

        void Update(Stock stock);

        void Delete(Stock stock);
    }
}