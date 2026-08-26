using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        // =====================================================
        // CRUD
        // =====================================================

        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department?> GetByNameAsync(string name);

        Task<bool> NameExistsAsync(
            string name,
            int? excludeDepartmentId = null);

        Task<bool> CodeExistsAsync(
            string code,
            int? excludeDepartmentId = null);

        Task AddAsync(Department department);

        void Update(Department department);

        void Delete(Department department);


        // =====================================================
        // USERS
        // =====================================================

        Task<List<User>> GetUsersAsync(int departmentId);


        // =====================================================
        // REQUESTS
        // =====================================================

        Task<List<StockRequest>> GetRequestsAsync(
            int departmentId);
    }
}