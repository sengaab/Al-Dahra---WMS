using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly DataBaseContext db;

        public SiteRepository(DataBaseContext db)
        {
            this.db = db;
        }


        public async Task<List<Site>> GetAllAsync()
        {
            return await db.Sites
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Site?> GetByIdAsync(int id)
        {
            return await db.Sites
                .FirstOrDefaultAsync(s => s.Site_Id == id);
        }


        public async Task<Site?> GetByCodeAsync(string code)
        {
            return await db.Sites
                .FirstOrDefaultAsync(
                    s => s.Site_Code == code);
        }


        public async Task AddAsync(Site site)
        {
            await db.Sites.AddAsync(site);
        }


        public void Update(Site site)
        {
            db.Sites.Update(site);
        }


        public void Delete(Site site)
        {
            db.Sites.Remove(site);
        }
    }
}