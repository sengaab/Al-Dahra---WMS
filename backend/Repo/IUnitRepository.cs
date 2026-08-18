using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        Task<List<Unit>> GetAllAsync();

        Task<Unit?> GetByIdAsync(int id);

        Task<Unit?> GetByNameAsync(string name);

        Task AddAsync(Unit unit);

        void Update(Unit unit);

        void Delete(Unit unit);
    }
}