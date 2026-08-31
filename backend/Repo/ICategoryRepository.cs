using whm.DTOs.Category;
using whm.DTOs.Product;
using whm.Models;

namespace whm.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        // =====================================================
        // GET ALL
        // =====================================================

        Task<List<CategoryDto>> GetAllAsync();


        // =====================================================
        // GET BY ID
        // =====================================================

        Task<CategoryDto?> GetByIdAsync(int id);


        // =====================================================
        // GET ENTITY
        // =====================================================

        Task<Category?> GetEntityByIdAsync(int id);


        // =====================================================
        // GET PRODUCTS
        // =====================================================

        Task<List<ProductDto>> GetProductsAsync(int categoryId);


        // =====================================================
        // CREATE
        // =====================================================

        Task AddAsync(Category category);


        // =====================================================
        // UPDATE
        // =====================================================

        void Update(Category category);


        // =====================================================
        // DELETE
        // =====================================================

        void Delete(Category category);
    }
}