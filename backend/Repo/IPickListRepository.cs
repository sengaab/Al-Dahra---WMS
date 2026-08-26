using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface IPickListRepository
    {
        Task<List<PickList>> GetAllAsync();

        Task<PickList?> GetByIdAsync(int id);

        Task AddAsync(PickList pickList);

        Task UpdateAsync(PickList pickList);

        Task<List<PickItem>> GetItemsAsync(int pickListId);

        Task<PickItem?> GetItemAsync(int pickListId, int itemId);
    }
}