using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Rack
    {
        [Key]
        public int Rack_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Rack_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Rack_Code { get; set; }

        [MaxLength(500)]
        public string? Rack_Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Room - OPTIONAL
        // ==========================================

        public int? Room_Id { get; set; }

        public Room? Room { get; set; }


        // ==========================================
        // Location - OPTIONAL
        // ==========================================

     public List<Location> Locations { get; set; }= new List<Location>();


        // ==========================================
        // Shelves
        // ==========================================

        public List<Shelf> Shelves { get; set; }
            = new List<Shelf>();
    }
}