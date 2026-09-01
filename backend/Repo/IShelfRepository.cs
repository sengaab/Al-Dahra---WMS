using whm.DTOs.Shelf;
using whm.Models;

namespace whm.Repositories
{
    public interface IShelfRepository
    {
        Task<IEnumerable<ShelfDto>> GetAllAsync(
            int? rackId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<ShelfDto?> GetByIdAsync(int id);

        Task<Shelf?> GetEntityByIdAsync(int id);

        Task<IEnumerable<ShelfDto>> GetByRackIdAsync(
            int rackId);

        Task<IEnumerable<ShelfDto>> GetByLocationIdAsync(
            int locationId);

        Task AddAsync(Shelf shelf);

        void Update(Shelf shelf);

        void Delete(Shelf shelf);
    }
}