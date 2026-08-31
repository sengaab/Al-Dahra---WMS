namespace whm.DTOs.Category
{
    // =====================================================
    // CATEGORY DTO
    // =====================================================

    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }


    // =====================================================
    // CREATE CATEGORY DTO
    // =====================================================

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }


    // =====================================================
    // UPDATE CATEGORY DTO
    // =====================================================

    public class UpdateCategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}