using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department?> GetByNameAsync(string name);

        Task AddAsync(Department department);

        void Update(Department department);

        void Delete(Department department);
    }
}