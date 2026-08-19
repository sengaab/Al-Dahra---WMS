using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateShelfDTO
    {
        [Required]
        [MaxLength(50)]
        public string Shelf_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Shelf_Code { get; set; }

        [MaxLength(500)]
        public string? Shelf_Description { get; set; }

        public bool IsActive { get; set; }
    }
}