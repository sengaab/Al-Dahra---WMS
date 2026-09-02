
using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Sites;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly DataBaseContext _context;

        public SiteRepository(DataBaseContext context)
        {
            _context = context;
        }

        // GET /api/sites
        public async Task<IEnumerable<SiteDto>> GetAllAsync()
        {
            return await _context.Sites
                .AsNoTracking()
                .Select(s => new SiteDto
                {
                    SiteId = s.SiteId,
                    Code = s.Code,
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    WarehouseCount = s.Warehouses.Count()
                })
                .ToListAsync();
        }

        // GET /api/sites/{id}
        public async Task<SiteDto?> GetByIdAsync(int id)
        {
            return await _context.Sites
                .AsNoTracking()
                .Where(s => s.SiteId == id)
                .Select(s => new SiteDto
                {
                    SiteId = s.SiteId,
                    Code = s.Code,
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    WarehouseCount = s.Warehouses.Count()
                })
                .FirstOrDefaultAsync();
        }

        // Get Site entity
        public async Task<Site?> GetEntityByIdAsync(int id)
        {
            return await _context.Sites
                .FirstOrDefaultAsync(s => s.SiteId == id);
        }

        // Check duplicate Site Code
        public async Task<bool> CodeExistsAsync(
            string code,
            int? excludeSiteId = null)
        {
            var query = _context.Sites
                .AsNoTracking()
                .Where(s => s.Code == code);

            if (excludeSiteId.HasValue)
            {
                query = query.Where(
                    s => s.SiteId != excludeSiteId.Value);
            }

            return await query.AnyAsync();
        }

        // GET /api/sites/{id}/warehouses
        public async Task<IEnumerable<SiteWarehouseDto>> GetWarehousesAsync(
            int siteId)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.SiteId == siteId)
                .Select(w => new SiteWarehouseDto
                {
                    WarehouseId = w.WarehouseId,
                    Code = w.Code,
                    Name = w.Name,
                    Description = w.Description,
                    IsActive = w.IsActive
                })
                .ToListAsync();
        }

        // GET /api/sites/{id}/inventory
        //
        // Inventory query should be implemented according
        // to the actual relationship between:
        // Site -> Warehouse -> Location -> Room -> Rack -> Shelf -> Bin -> Stock
        //
        // We don't reference Stock.Bin here because Stock
        // does not contain a Bin navigation property.
        public async Task<IEnumerable<SiteInventoryDto>> GetInventoryAsync(
            int siteId)
        {
            var warehouseIds = await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.SiteId == siteId)
                .Select(w => w.WarehouseId)
                .ToListAsync();

            // TODO:
            // Implement this query after confirming the actual
            // Stock -> Bin -> Shelf -> Rack -> Room -> Location
            // relationships in your models.

            return new List<SiteInventoryDto>();
        }

        // GET /api/sites/{id}/stats
        //
        // Same reason as inventory:
        // We don't make assumptions about Stock -> Bin.
        public async Task<SiteStatsDto?> GetStatsAsync(int siteId)
        {
            var site = await _context.Sites
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SiteId == siteId);

            if (site == null)
                return null;

            var warehouseCount = await _context.Warehouses
                .AsNoTracking()
                .CountAsync(w => w.SiteId == siteId);

            return new SiteStatsDto
            {
                SiteId = site.SiteId,
                SiteCode = site.Code,
                SiteName = site.Name,
                WarehouseCount = warehouseCount,

                // These depend on the actual inventory hierarchy.
                ProductCount = 0,
                BinCount = 0,
                TotalQuantity = 0
            };
        }

        // Create
        public async Task AddAsync(Site site)
        {
            await _context.Sites.AddAsync(site);
        }

        // Update
        public void Update(Site site)
        {
            _context.Sites.Update(site);
        }

        // Delete
        public void Delete(Site site)
        {
            _context.Sites.Remove(site);
        }
    }
}

