using whm.Models;

namespace whm.Repositories
{
    public interface IReceiptRepository
    {
        Task<List<Receipt>> GetAllAsync();

        Task<Receipt?> GetByIdAsync(int id);

        Task<Receipt?> GetByIdWithItemsAsync(int id);

        Task<List<ReceiptItem>> GetItemsAsync(int receiptId);

        Task<ReceiptItem?> GetItemByIdAsync(int receiptId, int itemId);

        Task AddAsync(Receipt receipt);

        Task AddItemAsync(ReceiptItem item);

        void Update(Receipt receipt);

        void UpdateItem(ReceiptItem item);

        void DeleteItem(ReceiptItem item);
    }
}