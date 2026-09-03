using whm.DTOs.Location;
using whm.DTOs.Stock;
using whm.Models;

namespace whm.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<LocationDto>> GetAllAsync(
            int? warehouseId = null,
            int? parentLocationId = null,
            int? roomId = null,
            int? rackId = null,
            int? shelfId = null,
            int? binId = null,
            string? search = null,
            string? type = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<LocationDto?> GetByIdAsync(int id);

        Task<Location?> GetEntityByIdAsync(int id);

        Task<IEnumerable<LocationDto>> GetChildrenAsync(
            int parentLocationId);

        Task<LocationStructureDto?> GetStructureAsync(
            int locationId);

        Task<LocationOccupancyDto?> GetOccupancyAsync(
            int locationId);

        Task<IEnumerable<StockDto>> GetInventoryAsync(
            int locationId);

        Task<IEnumerable<LocationTreeDto>> GetTreeAsync();

        Task<int> GetLocationIdByBinIdAsync(
            int binId);

        Task AddAsync(Location location);

        void Update(Location location);

        void Delete(Location location);
    }
}