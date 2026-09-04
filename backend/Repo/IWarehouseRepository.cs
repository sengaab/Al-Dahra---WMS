using whm.DTOs.Stock;
using whm.DTOs.Warehouse;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IWarehouseRepository
    {
        // =====================================================
        // GET ALL
        // =====================================================

        Task<List<WarehouseDto>> GetAllAsync(
            int? siteId = null,
            string? search = null,
            string? status = null,
            int page = 1,
            int pageSize = 20);


        // =====================================================
        // GET BY ID
        // =====================================================

        Task<WarehouseDto?> GetByIdAsync(int id);


        // =====================================================
        // GET ENTITY
        // =====================================================

        Task<Warehouse?> GetEntityByIdAsync(int id);


        // =====================================================
        // GET INVENTORY
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