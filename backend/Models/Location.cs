using System.ComponentModel.DataAnnotations;
using whm.Models;

public class Location
{
    [Key]
    public int LocationId { get; set; }

    // Warehouse - OPTIONAL
    public int? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    // Parent Location - OPTIONAL
    public int? ParentLocationId { get; set; }
    public Location? ParentLocation { get; set; }

    public ICollection<Location> ChildLocations { get; set; }
        = new List<Location>();

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

    // Physical Locations
    public ICollection<Room> Rooms { get; set; }
        = new List<Room>();

    public ICollection<Rack> Racks { get; set; }
        = new List<Rack>();

    public ICollection<Shelf> Shelves { get; set; }
        = new List<Shelf>();

    public ICollection<Bin> Bins { get; set; }
        = new List<Bin>();
}