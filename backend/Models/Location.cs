using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        // ==========================================
        // Warehouse - OPTIONAL
        // ==========================================

        public int? WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }


        // ==========================================
        // Parent Location - OPTIONAL
        // ==========================================

        public int? ParentLocationId { get; set; }

        public Location? ParentLocation { get; set; }

        public ICollection<Location> ChildLocations { get; set; }
            = new List<Location>();


        // ==========================================
        // Room - OPTIONAL
        // ==========================================

        public int? RoomId { get; set; }

        public Room? Room { get; set; }


        // ==========================================
        // Rack - OPTIONAL
        // ==========================================

        public int? RackId { get; set; }

        public Rack? Rack { get; set; }


        // ==========================================
        // Shelf - OPTIONAL
        // ==========================================

        public int? ShelfId { get; set; }

        public Shelf? Shelf { get; set; }


        // ==========================================
        // Bin - OPTIONAL
        // ==========================================

        public int? BinId { get; set; }

        public Bin? Bin { get; set; }


        // ==========================================
        // Location Details
        // ==========================================

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
    }
}