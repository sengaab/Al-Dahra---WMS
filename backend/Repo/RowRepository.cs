using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class RowRepository : IRowRepository
    {
        private readonly DataBaseContext context;

        public RowRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Row>> GetAllAsync()
        {
            return await context.Rows
                .Include(r => r.Room)
                .Include(r => r.Shelves)
                .ToListAsync();
        }

        public async Task<Row?> GetByIdAsync(int id)
        {
            return await context.Rows
                .Include(r => r.Room)
                .Include(r => r.Shelves)
                .FirstOrDefaultAsync(
                    r => r.Row_Id == id);
        }

        public async Task<IEnumerable<Row>> GetByRoomIdAsync(
            int roomId)
        {
            return await context.Rows
                .Where(r => r.Room_Id == roomId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Rows
                .AnyAsync(r => r.Row_Id == id);
        }

        public async Task<bool> NameExistsInRoomAsync(
            string name,
            int roomId)
        {
            return await context.Rows
                .AnyAsync(r =>
                    r.Row_Name == name &&
                    r.Room_Id == roomId);
        }

        public async Task AddAsync(Row row)
        {
            await context.Rows.AddAsync(row);
        }

        public void Update(Row row)
        {
            context.Rows.Update(row);
        }

        public void Delete(Row row)
        {
            context.Rows.Remove(row);
        }
    }
}