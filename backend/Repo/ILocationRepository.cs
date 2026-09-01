using whm.DTOs.Location;
using whm.DTOs.Stock;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        // =====================================================
        // GET
        // =====================================================

        Task<List<LocationDto>> GetAllAsync();

        Task<LocationDto?> GetByIdAsync(int id);

        Task<Location?> GetEntityByIdAsync(int id);


        // =====================================================
        // CHILDREN
        // =====================================================

        Task<List<LocationDto>> GetChildrenAsync(
            int locationId);


        // =====================================================
        // INVENTORY
        // =====================================================

        Task<List<StockDto>> GetInventoryAsync(
            int locationId);


        // =====================================================
        // OCCUPANCY
        // =====================================================

        Task<LocationOccupancyDto?> GetOccupancyAsync(
            int locationId);


        // =====================================================
        // TREE
        // =====================================================

        Task<List<WarehouseTreeDto>> GetTreeAsync();


        // =====================================================
        // STRUCTURE
        // =====================================================

        Task<LocationStructureDto?> GetStructureAsync(
            int locationId);


        // =====================================================
        // CRUD
        // =====================================================

        Task AddAsync(Location location);

        void Update(Location location);

        void Delete(Location location);
    }
}