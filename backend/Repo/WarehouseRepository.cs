using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DataBaseContext db;

        public WarehouseRepository(DataBaseContext db)
        {
            this.db = db;
        }

        // =========================================================
        // GET ALL
        // =========================================================

        public async Task<List<Warehouse>> GetAllAsync()
        {
            return await db.Warehouses
                .AsNoTracking()
                .OrderBy(w => w.Warehouse_Name)
                .ToListAsync();
        }

        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await db.Warehouses
                .FirstOrDefaultAsync(
                    w => w.Warehouse_Id == id);
        }

        // =========================================================
        // GET BY CODE
        // =========================================================

        public async Task<Warehouse?> GetByCodeAsync(
            string code)
        {
            return await db.Warehouses
                .FirstOrDefaultAsync(
                    w => w.Warehouse_Code == code);
        }

        // =========================================================
        // EXISTS
        // =========================================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await db.Warehouses
                .AnyAsync(
                    w => w.Warehouse_Id == id);
        }

        // =========================================================
        // CHECK CODE
        // =========================================================

        public async Task<bool> CodeExistsAsync(
            string code,
            int? excludeId = null)
        {
            var query = db.Warehouses
                .Where(w => w.Warehouse_Code == code);

            if (excludeId.HasValue)
            {
                query = query.Where(
                    w => w.Warehouse_Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        // =========================================================
        // ADD
        // =========================================================

        public async Task AddAsync(
            Warehouse warehouse)
        {
            await db.Warehouses.AddAsync(warehouse);
        }

        // =========================================================
        // UPDATE
        // =========================================================

        public void Update(
            Warehouse warehouse)
        {
            db.Warehouses.Update(warehouse);
        }

        // =========================================================
        // DELETE
        // =========================================================

        public void Delete(
            Warehouse warehouse)
        {
            db.Warehouses.Remove(warehouse);
        }
    }
}