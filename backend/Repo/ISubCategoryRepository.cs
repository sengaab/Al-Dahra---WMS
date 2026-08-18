using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory>> GetAllAsync();

        Task<SubCategory?> GetByIdAsync(int id);

        Task<IEnumerable<SubCategory>> GetByCategoryIdAsync(
            int categoryId);

        Task AddAsync(SubCategory subCategory);

        void Update(SubCategory subCategory);

        void Delete(SubCategory subCategory);

        Task<bool> ExistsAsync(int id);
    }
}