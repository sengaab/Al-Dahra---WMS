using whm.DTOs.Rack;
using whm.Models;

namespace whm.Repositories
{
    public interface IRackRepository
    {
        Task<IEnumerable<RackDto>> GetAllAsync(
            int? roomId = null,
            int? locationId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<RackDto?> GetByIdAsync(int id);

        Task<Rack?> GetEntityByIdAsync(int id);

        Task<IEnumerable<RackDto>> GetByRoomIdAsync(
            int roomId);

        Task<IEnumerable<RackDto>> GetByLocationIdAsync(
            int locationId);

        Task AddAsync(Rack rack);

        void Update(Rack rack);

        void Delete(Rack rack);
    }
}