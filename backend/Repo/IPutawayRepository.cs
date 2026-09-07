using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IPutawayRepository
    {
        // =========================
        // Putaway
        // =========================

        Task<IEnumerable<Putaway>> GetAllAsync();

        Task<Putaway?> GetByIdAsync(int id);

        Task<Putaway> AddAsync(Putaway putaway);

        Task UpdateAsync(Putaway putaway);

        Task<bool> ExistsAsync(int id);

        // =========================
        // Putaway Items
        // =========================

        Task<IEnumerable<PutawayItem>> GetItemsAsync(int putawayId);

        Task<PutawayItem?> GetItemByIdAsync(
            int putawayId,
            int itemId);

        Task<PutawayItem> AddItemAsync(
            PutawayItem item);

        Task UpdateItemAsync(
            PutawayItem item);
    }
}