using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Categories>> GetAllAsync();

        Task<Categories?> GetByIdAsync(int id);

        Task<List<Categories>> GetByDepartmentIdAsync(int departmentId);

        Task<Categories?> GetByNameAsync(string name);

        Task AddAsync(Categories category);

        void Update(Categories category);

        void Delete(Categories category);
    }
}