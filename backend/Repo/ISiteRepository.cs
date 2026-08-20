using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ISiteRepository
    {
        Task<List<Site>> GetAllAsync();

        Task<Site?> GetByIdAsync(int id);

        Task<Site?> GetByCodeAsync(string code);

        Task AddAsync(Site site);

        void Update(Site site);

        void Delete(Site site);
    }
}