using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateSubCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string SubCategory_Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? SubCategory_Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}