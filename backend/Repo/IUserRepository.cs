using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email, Guid? excludeUserId = null);

        Task<bool> EmployeeCodeExistsAsync(
            string employeeCode,
            Guid? excludeUserId = null);

        Task<List<AuditLog>> GetActivityAsync(Guid userId);

        Task<User?> GetUserWithPermissionsAsync(Guid userId);

        Task AddAsync(User user);

        void Update(User user);

        void Delete(User user);
    }
}