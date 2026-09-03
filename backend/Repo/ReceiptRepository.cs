using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;

namespace whm.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly DataBaseContext db;

        public ReceiptRepository(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<List<Receipt>> GetAllAsync()
        {
            return await db.Receipts
                .AsNoTracking()
                .Include(r => r.PurchaseOrder)
                .Include(r => r.Warehouse)
                .Include(r => r.Receiver)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(r => r.ReceiptId)
                .ToListAsync();
        }

        public async Task<Receipt?> GetByIdAsync(int id)
        {
            return await db.Receipts
                .AsNoTracking()
                .Include(r => r.PurchaseOrder)
                .Include(r => r.Warehouse)
                .Include(r => r.Receiver)
                .FirstOrDefaultAsync(r => r.ReceiptId == id);
        }

        public async Task<Receipt?> GetByIdWithItemsAsync(int id)
        {
            return await db.Receipts
                .Include(r => r.PurchaseOrder)
                .Include(r => r.Warehouse)
                .Include(r => r.Receiver)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Product)
                .Include(r => r.Items)
                    .ThenInclude(i => i.PurchaseOrderItem)
                .FirstOrDefaultAsync(r => r.ReceiptId == id);
        }

        public async Task<List<ReceiptItem>> GetItemsAsync(int receiptId)
        {
            return await db.ReceiptItems
                .AsNoTracking()
                .Where(i => i.ReceiptId == receiptId)
                .Include(i => i.Product)
                .Include(i => i.PurchaseOrderItem)
                .OrderBy(i => i.ReceiptItemId)
                .ToListAsync();
        }

        public async Task<ReceiptItem?> GetItemByIdAsync(
            int receiptId,
            int itemId)
        {
            return await db.ReceiptItems
                .FirstOrDefaultAsync(i =>
                    i.ReceiptId == receiptId &&
                    i.ReceiptItemId == itemId);
        }

        public async Task AddAsync(Receipt receipt)
        {
            await db.Receipts.AddAsync(receipt);
        }

        public async Task AddItemAsync(ReceiptItem item)
        {
            await db.ReceiptItems.AddAsync(item);
        }

        public void Update(Receipt receipt)
        {
            db.Receipts.Update(receipt);
        }

        public void UpdateItem(ReceiptItem item)
        {
            db.ReceiptItems.Update(item);
        }

        public void DeleteItem(ReceiptItem item)
        {
            db.ReceiptItems.Remove(item);
        }
    }
}
