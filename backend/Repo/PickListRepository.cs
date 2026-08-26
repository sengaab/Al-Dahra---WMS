using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class PickListRepository : IPickListRepository
    {
        private readonly DataBaseContext _context;

        public PickListRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL PICK LISTS
        // =====================================================

        public async Task<List<PickList>> GetAllAsync()
        {
            return await _context.PickLists
                .Include(x => x.StockRequest)
                .Include(x => x.Warehouse)
                .Include(x => x.Assignee)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.PickListId)
                .ToListAsync();
        }


        // =====================================================
        // GET PICK LIST BY ID
        // =====================================================

        public async Task<PickList?> GetByIdAsync(int id)
        {
            return await _context.PickLists
                .Include(x => x.StockRequest)
                .Include(x => x.Warehouse)
                .Include(x => x.Assignee)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.PickListId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(PickList pickList)
        {
            await _context.PickLists.AddAsync(pickList);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public Task UpdateAsync(PickList pickList)
        {
            _context.PickLists.Update(pickList);

            return Task.CompletedTask;
        }


        // =====================================================
        // GET ITEMS
        // =====================================================

        public async Task<List<PickItem>> GetItemsAsync(int pickListId)
        {
            return await _context.PickItems
                .Where(x => x.PickListId == pickListId)
                .Include(x => x.Product)
                .Include(x => x.Stock)
                .Include(x => x.Location)
                .AsNoTracking()
                .ToListAsync();
        }


        // =====================================================
        // GET ITEM
        // =====================================================

        public async Task<PickItem?> GetItemAsync(
            int pickListId,
            int itemId)
        {
            return await _context.PickItems
                .Include(x => x.Product)
                .Include(x => x.Stock)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x =>
                    x.PickListId == pickListId &&
                    x.PickItemId == itemId);
        }
    }
}