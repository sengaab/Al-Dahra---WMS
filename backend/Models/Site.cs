using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Site
    {
        [Key]
        public int Site_Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Site_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Site_Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Site_Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation
        public List<Warehouse> Warehouses { get; set; }
            = new List<Warehouse>();
    }
}