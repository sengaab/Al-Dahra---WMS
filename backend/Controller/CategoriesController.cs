using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Category;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET /api/categories
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories =
                await _unitOfWork.Categories.GetAllAsync();

            return Ok(categories);
        }


        // =====================================================
        // GET BY ID
        // GET /api/categories/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category =
                await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            return Ok(category);
        }


        // =====================================================
        // GET PRODUCTS
        // GET /api/categories/{id}/products
        // =====================================================

        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetProducts(int id)
        {
            var category =
                await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            var products =
                await _unitOfWork.Categories
                    .GetProductsAsync(id);

            return Ok(products);
        }


        // =====================================================
        // CREATE
        // POST /api/categories
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required."
                });
            }


            var category = new Category
            {
                Name = dto.Name.Trim(),

                Description = string.IsNullOrWhiteSpace(
                    dto.Description)
                    ? null
                    : dto.Description.Trim(),

                IsActive = true,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow
            };


            await _unitOfWork.Categories
                .AddAsync(category);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Categories
                    .GetByIdAsync(category.CategoryId);


            return CreatedAtAction(
                nameof(GetCategory),
                new
                {
                    id = category.CategoryId
                },
                result);
        }


        // =====================================================
        // UPDATE
        // PUT /api/categories/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            var category =
                await _unitOfWork.Categories
                    .GetEntityByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required."
                });
            }


            category.Name = dto.Name.Trim();

            category.Description =
                string.IsNullOrWhiteSpace(dto.Description)
                    ? null
                    : dto.Description.Trim();

            category.IsActive = dto.IsActive;

            category.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Categories
                .Update(category);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Categories
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE
        // DELETE /api/categories/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category =
                await _unitOfWork.Categories
                    .GetEntityByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }


            _unitOfWork.Categories
                .Delete(category);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Category deleted successfully."
            });
        }
    }
}