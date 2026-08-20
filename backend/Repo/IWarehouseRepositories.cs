using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetAllAsync();

        Task<Warehouse?> GetByIdAsync(int id);

        Task<Warehouse?> GetByCodeAsync(string code);

        Task<bool> ExistsAsync(int id);

        Task<bool> CodeExistsAsync(string code, int? excludeId = null);

        Task AddAsync(Warehouse warehouse);

        void Update(Warehouse warehouse);

        void Delete(Warehouse warehouse);
    }
}