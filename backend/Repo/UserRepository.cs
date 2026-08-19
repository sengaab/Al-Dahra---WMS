using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext context;

        public UserRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await context.Users
                .Include(u => u.role)
                .ToListAsync();
        }

        public async Task<Users?> GetByIdAsync(Guid id)
        {
            return await context.Users
                .Include(u => u.role)
                .FirstOrDefaultAsync(
                    u => u.User_Id == id);
        }

        public async Task<Users?> GetByEmailAsync(
            string email)
        {
            return await context.Users
                .Include(u => u.role)
                .FirstOrDefaultAsync(
                    u => u.User_Email == email);
        }

        public async Task<bool> EmailExistsAsync(
            string email)
        {
            return await context.Users
                .AnyAsync(
                    u => u.User_Email == email);
        }

        public async Task AddAsync(Users user)
        {
            await context.Users.AddAsync(user);
        }

        public void Update(Users user)
        {
            context.Users.Update(user);
        }

        public void Delete(Users user)
        {
            context.Users.Remove(user);
        }
    }
}