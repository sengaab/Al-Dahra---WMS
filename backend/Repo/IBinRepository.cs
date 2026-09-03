using whm.DTOs.Bin;
using whm.Models;

namespace whm.Repositories
{
    public interface IBinRepository
    {
        Task<IEnumerable<BinDto>> GetAllAsync(
            int? shelfId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<BinDto?> GetByIdAsync(int id);

        Task<Bin?> GetEntityByIdAsync(int id);

        Task<IEnumerable<BinDto>> GetByShelfIdAsync(int shelfId);

        Task<IEnumerable<BinDto>> GetByLocationIdAsync(int locationId);

        Task AddAsync(Bin bin);

        void Update(Bin bin);

        void Delete(Bin bin);
    }
}