using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        public int WarehouseId { get; set; }

        public int? ParentLocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public Warehouse Warehouse { get; set; } = null!;

        public Location? ParentLocation { get; set; }

        public ICollection<Location> ChildLocations { get; set; }
            = new List<Location>();
    }
}