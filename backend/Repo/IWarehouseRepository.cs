using whm.DTOs.Stock;
using whm.DTOs.Warehouse;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IWarehouseRepository
    {
        // =====================================================
        // GET
        // =====================================================

        Task<List<WarehouseDto>> GetAllAsync(
            int? siteId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);

        Task<WarehouseDto?> GetByIdAsync(int id);

        Task<Warehouse?> GetEntityByIdAsync(int id);


        // =====================================================
        // LOCATIONS
        // =====================================================

        Task<List<WarehouseLocationDto>> GetLocationsAsync(
            int warehouseId);


        // =====================================================
        // INVENTORY
        // =====================================================

        Task<List<StockDto>> GetInventoryAsync(
            int warehouseId);


        // =====================================================
        // STATS
        // =====================================================

        Task<WarehouseStatsDto?> GetStatsAsync(
            int warehouseId);


        // =====================================================
        // OCCUPANCY
        // =====================================================

        Task<WarehouseOccupancyDto?> GetOccupancyAsync(
            int warehouseId);


        // =====================================================
        // CRUD
        // =====================================================

        Task AddAsync(Warehouse warehouse);

        void Update(Warehouse warehouse);

        void Delete(Warehouse warehouse);
    }
}
