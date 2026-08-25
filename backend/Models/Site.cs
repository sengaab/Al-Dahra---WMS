using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Site
    {
        [Key]
        public int SiteId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public ICollection<Warehouse> Warehouses { get; set; }
            = new List<Warehouse>();
    }
}