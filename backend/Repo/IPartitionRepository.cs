using whm.DTOs.Partition;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IPartitionRepository
    {
        Task<IEnumerable<PartitionDto>> GetAllAsync(
            int? warehouseId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<PartitionDto?> GetByIdAsync(int id);

        Task<IEnumerable<PartitionDto>> GetByWarehouseIdAsync(
            int warehouseId);

        Task<IEnumerable<PartitionSummaryDto>> GetSummaryAsync(
            int? warehouseId = null);

        Task<Partition?> GetEntityByIdAsync(int id);

        Task AddAsync(Partition partition);

        void Update(Partition partition);

        void Delete(Partition partition);
    }
}