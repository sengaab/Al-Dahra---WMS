using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

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
        public Site Site { get; set; } = null!;

        public ICollection<Location> Locations { get; set; }
            = new List<Location>();
    }
}