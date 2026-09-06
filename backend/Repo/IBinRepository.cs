using whm.DTOs.Bin;
using whm.Models;

namespace whm.Repositories
{
    public interface IBinRepository
    {
        Task<IEnumerable<BinDto>> GetAllAsync(
            int? warehouseId = null,
            int? partitionId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<BinDto?> GetByIdAsync(int id);

        Task<Bin?> GetEntityByIdAsync(int id);

        Task<IEnumerable<BinDto>> GetByWarehouseIdAsync(
            int warehouseId);

        Task<IEnumerable<BinDto>> GetByPartitionIdAsync(
            int partitionId);

        Task<IEnumerable<BinDto>> GetByLocationIdAsync(
            int locationId);

        Task<bool> ExistsByPartitionIdAsync(
            int partitionId);

        Task AddAsync(Bin bin);

        void Update(Bin bin);

        void Delete(Bin bin);
    }
}