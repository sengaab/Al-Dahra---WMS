using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllAsync();

        Task<Users?> GetByIdAsync(Guid id);

        Task<Users?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

        Task AddAsync(Users user);

        void Update(Users user);

        void Delete(Users user);
    }
}