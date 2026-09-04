using whm.DTOs.Location;
using whm.DTOs.Stock;
using whm.Models;

namespace whm.Repositories
{
    public interface ILocationRepository
    {
        // =====================================================
        // GET ALL
        // =====================================================

        Task<IEnumerable<LocationDto>> GetAllAsync(
            int? warehouseId = null,
            int? partitionId = null,
            int? binId = null,
            string? search = null,
            string? type = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);


        // =====================================================
        // GET BY ID
        // =====================================================

        Task<LocationDto?> GetByIdAsync(int id);


        // =====================================================
        // GET ENTITY BY ID
        // =====================================================

        Task<Location?> GetEntityByIdAsync(int id);


        // =====================================================
        // GET LOCATIONS BY BIN
        // =====================================================

        Task<IEnumerable<LocationDto>> GetByBinIdAsync(int binId);


        // =====================================================
        // GET STRUCTURE
        // =====================================================

        Task<LocationStructureDto?> GetStructureAsync(
            int locationId);


        // =====================================================
        // GET OCCUPANCY
        // =====================================================

        Task<LocationOccupancyDto?> GetOccupancyAsync(
            int locationId);


        // =====================================================
        // GET INVENTORY
        // =====================================================

        Task<IEnumerable<StockDto>> GetInventoryAsync(
            int locationId);


        // =====================================================
        // GET LOCATION ID BY BIN
        // =====================================================

        Task<int?> GetLocationIdByBinIdAsync(
            int binId);


        // =====================================================
        // ADD
        // =====================================================

        Task AddAsync(Location location);


        // =====================================================
        // UPDATE
        // =====================================================

        void Update(Location location);


        // =====================================================
        // DELETE
        // =====================================================

        void Delete(Location location);
    }
}