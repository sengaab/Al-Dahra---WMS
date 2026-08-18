using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =========================================
        // GET ALL
        // GET: api/Categories
        // =========================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories =
                await unitOfWork.Categories.GetAllAsync();

            return Ok(categories.Select(c => new
            {
                categoryId = c.Category_Id,
                categoryName = c.Category_Name,
                description = c.Description,
                departmentId = c.Department_Id,
                departmentName = c.Department?.Department_Name,
                isActive = c.IsActive,
                createdAt = c.CreatedAt,
                updatedAt = c.UpdatedAt
            }));
        }

        // =========================================
        // GET BY ID
        // GET: api/Categories/1
        // =========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category =
                await unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(new
            {
                categoryId = category.Category_Id,
                categoryName = category.Category_Name,
                description = category.Description,
                departmentId = category.Department_Id,
                departmentName = category.Department?.Department_Name,
                isActive = category.IsActive,
                createdAt = category.CreatedAt,
                updatedAt = category.UpdatedAt
            });
        }

        // =========================================
        // GET BY DEPARTMENT
        // GET: api/Categories/Department/1
        // =========================================

        [HttpGet("Department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(
            int departmentId)
        {
            var categories =
                await unitOfWork.Categories
                    .GetByDepartmentIdAsync(departmentId);

            return Ok(categories.Select(c => new
            {
                categoryId = c.Category_Id,
                categoryName = c.Category_Name,
                description = c.Description,
                departmentId = c.Department_Id,
                isActive = c.IsActive
            }));
        }

        // =========================================
        // CREATE
        // POST: api/Categories
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var departmentExists =
            //    await unitOfWork.Departments
            //        .GetByIdAsync(dto.Department_Id);

            //if (departmentExists == null)
            //{
            //    return BadRequest("Department not found.");
            //}

            var existing =
                await unitOfWork.Categories
                    .GetByNameAsync(dto.Category_Name.Trim());

            if (existing != null)
            {
                return Conflict(
                    "Category with this name already exists.");
            }

            var category = new Categories
            {
                Category_Name = dto.Category_Name.Trim(),

                Description = string.IsNullOrWhiteSpace(dto.Description)
                    ? null
                    : dto.Description.Trim(),

                Department_Id = dto.Department_Id,

                IsActive = true,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = null
            };

            await unitOfWork.Categories.AddAsync(category);

            await unitOfWork.SaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Category_Id },
                new
                {
                    message = "Category created successfully.",
                    categoryId = category.Category_Id,
                    categoryName = category.Category_Name
                });
        }

        // =========================================
        // UPDATE
        // PUT: api/Categories/1
        // =========================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category =
                await unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            //var departmentExists =
            //    await unitOfWork.Departments
            //        .GetByIdAsync(dto.Department_Id);

            //if (departmentExists == null)
            //{
            //    return BadRequest("Department not found.");
            //}

            var existing =
                await unitOfWork.Categories
                    .GetByNameAsync(dto.Category_Name.Trim());

            if (existing != null &&
                existing.Category_Id != id)
            {
                return Conflict(
                    "Another category with this name already exists.");
            }

            category.Category_Name =
                dto.Category_Name.Trim();

            category.Description =
                string.IsNullOrWhiteSpace(dto.Description)
                    ? null
                    : dto.Description.Trim();

            category.Department_Id =
                dto.Department_Id;

            category.IsActive =
                dto.IsActive;

            category.UpdatedAt =
                DateTimeOffset.UtcNow;

            unitOfWork.Categories.Update(category);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Category updated successfully."
            });
        }

        // =========================================
        // DELETE
        // DELETE: api/Categories/1
        // =========================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category =
                await unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            // Soft Delete
            category.IsActive = false;
            category.UpdatedAt = DateTimeOffset.UtcNow;

            unitOfWork.Categories.Update(category);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Category deactivated successfully."
            });
        }
    }
}