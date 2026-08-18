using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataBaseContext db;

        public UnitRepository(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<List<Unit>> GetAllAsync()
        {
            return await db.Units
                .OrderBy(u => u.Unit_Name)
                .ToListAsync();
        }

        public async Task<Unit?> GetByIdAsync(int id)
        {
            return await db.Units
                .FirstOrDefaultAsync(u => u.Unit_Id == id);
        }

        public async Task<Unit?> GetByNameAsync(string name)
        {
            return await db.Units
                .FirstOrDefaultAsync(u =>
                    u.Unit_Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Unit unit)
        {
            await db.Units.AddAsync(unit);
        }

        public void Update(Unit unit)
        {
            db.Units.Update(unit);
        }

        public void Delete(Unit unit)
        {
            db.Units.Remove(unit);
        }
    }
}