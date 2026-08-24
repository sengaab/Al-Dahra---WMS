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

        // =========================================================
        // GET INVENTORY WITH PAGINATION
        // =========================================================

        Task<(List<Stock> Stocks, int TotalCount)>
            GetInventoryAsync(
                int pageNumber,
                int pageSize);

        Task AddAsync(Stock stock);

        void Update(Stock stock);

        void Delete(Stock stock);
    }
}