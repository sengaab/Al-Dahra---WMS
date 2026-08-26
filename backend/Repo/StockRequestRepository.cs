using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockRequestRepository : IStockRequestRepository
    {
        private readonly DataBaseContext _context;

        public StockRequestRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<List<StockRequest>> GetAllAsync()
        {
            return await _context.StockRequests
                .Include(x => x.Department)
                .Include(x => x.Site)
                .Include(x => x.Requester)
                .Include(x => x.Approver)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.RequestedAt)
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StockRequest?> GetByIdAsync(int id)
        {
            return await _context.StockRequests
                .Include(x => x.Department)
                .Include(x => x.Site)
                .Include(x => x.Requester)
                .Include(x => x.Approver)
                .FirstOrDefaultAsync(x => x.RequestId == id);
        }


        // =====================================================
        // GET BY ID WITH ITEMS
        // =====================================================

        public async Task<StockRequest?> GetByIdWithItemsAsync(int id)
        {
            return await _context.StockRequests
                .Include(x => x.Department)
                .Include(x => x.Site)
                .Include(x => x.Requester)
                .Include(x => x.Approver)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.RequestId == id);
        }


        // =====================================================
        // CHECK REQUEST NUMBER
        // =====================================================

        public async Task<bool> RequestNumberExistsAsync(
            string requestNumber,
            int? excludeRequestId = null)
        {
            return await _context.StockRequests
                .AnyAsync(x =>
                    x.RequestNumber == requestNumber &&
                    (!excludeRequestId.HasValue ||
                     x.RequestId != excludeRequestId.Value));
        }


        // =====================================================
        // ADD REQUEST
        // =====================================================

        public async Task AddAsync(StockRequest request)
        {
            await _context.StockRequests.AddAsync(request);
        }


        // =====================================================
        // UPDATE REQUEST
        // =====================================================

        public void Update(StockRequest request)
        {
            _context.StockRequests.Update(request);
        }


        // =====================================================
        // DELETE REQUEST
        // =====================================================

        public void Delete(StockRequest request)
        {
            _context.StockRequests.Remove(request);
        }


        // =====================================================
        // GET ITEMS
        // =====================================================

        public async Task<List<StockRequestItem>> GetItemsAsync(
            int requestId)
        {
            return await _context.StockRequestItems
                .Where(x => x.RequestId == requestId)
                .Include(x => x.Product)
                .AsNoTracking()
                .OrderBy(x => x.RequestItemId)
                .ToListAsync();
        }


        // =====================================================
        // GET ITEM BY ID
        // =====================================================

        public async Task<StockRequestItem?> GetItemByIdAsync(
            int requestId,
            int itemId)
        {
            return await _context.StockRequestItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.RequestItemId == itemId &&
                    x.RequestId == requestId);
        }


        // =====================================================
        // CHECK PRODUCT IN REQUEST
        // =====================================================

        public async Task<bool> ProductExistsInRequestAsync(
            int requestId,
            int productId,
            int? excludeItemId = null)
        {
            return await _context.StockRequestItems
                .AnyAsync(x =>
                    x.RequestId == requestId &&
                    x.ProductId == productId &&
                    (!excludeItemId.HasValue ||
                     x.RequestItemId != excludeItemId.Value));
        }


        // =====================================================
        // ADD ITEM
        // =====================================================

        public async Task AddItemAsync(StockRequestItem item)
        {
            await _context.StockRequestItems.AddAsync(item);
        }


        // =====================================================
        // UPDATE ITEM
        // =====================================================

        public void UpdateItem(StockRequestItem item)
        {
            _context.StockRequestItems.Update(item);
        }


        // =====================================================
        // DELETE ITEM
        // =====================================================

        public void DeleteItem(StockRequestItem item)
        {
            _context.StockRequestItems.Remove(item);
        }
    }
}