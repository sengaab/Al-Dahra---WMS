using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string Category_Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public int Department_Id { get; set; }

        public bool IsActive { get; set; }
    }
}