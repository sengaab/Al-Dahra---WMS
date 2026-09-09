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


        // =====================================================
        // GET ALL RECEIPTS
        // =====================================================

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


        // =====================================================
        // GET RECEIPT BY ID
        // =====================================================

        public async Task<Receipt?> GetByIdAsync(int id)
        {
            return await db.Receipts
                .AsNoTracking()
                .Include(r => r.PurchaseOrder)
                .Include(r => r.Warehouse)
                .Include(r => r.Receiver)
                .FirstOrDefaultAsync(r =>
                    r.ReceiptId == id);
        }


        // =====================================================
        // GET RECEIPT WITH ITEMS
        // =====================================================

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
                .FirstOrDefaultAsync(r =>
                    r.ReceiptId == id);
        }


        // =====================================================
        // GET RECEIPT ITEMS
        // =====================================================

        public async Task<List<ReceiptItem>> GetItemsAsync(
            int receiptId)
        {
            return await db.ReceiptItems
                .AsNoTracking()
                .Where(i => i.ReceiptId == receiptId)
                .Include(i => i.Product)
                .Include(i => i.PurchaseOrderItem)
                .OrderBy(i => i.ReceiptItemId)
                .ToListAsync();
        }


        // =====================================================
        // GET RECEIPT ITEM BY ID
        // =====================================================

        public async Task<ReceiptItem?> GetItemByIdAsync(
            int receiptId,
            int itemId)
        {
            return await db.ReceiptItems
                .FirstOrDefaultAsync(i =>
                    i.ReceiptId == receiptId &&
                    i.ReceiptItemId == itemId);
        }


        // =====================================================
        // ADD RECEIPT
        // =====================================================

        public async Task AddAsync(Receipt receipt)
        {
            await db.Receipts.AddAsync(receipt);
        }


        // =====================================================
        // ADD RECEIPT ITEM
        // =====================================================

        public async Task AddItemAsync(ReceiptItem item)
        {
            await db.ReceiptItems.AddAsync(item);
        }


        // =====================================================
        // UPDATE RECEIPT
        // =====================================================

        public void Update(Receipt receipt)
        {
            db.Receipts.Update(receipt);
        }


        // =====================================================
        // UPDATE RECEIPT ITEM
        // =====================================================

        public void UpdateItem(ReceiptItem item)
        {
            db.ReceiptItems.Update(item);
        }


        // =====================================================
        // DELETE RECEIPT ITEM
        // =====================================================

        public void DeleteItem(ReceiptItem item)
        {
            db.ReceiptItems.Remove(item);
        }
    }
}