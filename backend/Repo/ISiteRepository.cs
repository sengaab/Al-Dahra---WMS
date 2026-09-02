using whm.DTOs.Sites;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ISiteRepository
    {
        Task<IEnumerable<SiteDto>> GetAllAsync();

        Task<SiteDto?> GetByIdAsync(int id);

        Task<Site?> GetEntityByIdAsync(int id);

        Task<bool> CodeExistsAsync(string code, int? excludeSiteId = null);

        Task<IEnumerable<SiteWarehouseDto>> GetWarehousesAsync(int siteId);

        Task<IEnumerable<SiteInventoryDto>> GetInventoryAsync(int siteId);

        Task<SiteStatsDto?> GetStatsAsync(int siteId);

        Task AddAsync(Site site);

        void Update(Site site);

        void Delete(Site site);
    }
}