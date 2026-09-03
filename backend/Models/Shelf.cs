using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Shelf
    {
        [Key]
        public int Shelf_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Shelf_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Shelf_Code { get; set; }

        [MaxLength(500)]
        public string? Shelf_Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Rack - OPTIONAL
        // ==========================================

        public int? Rack_Id { get; set; }

        public Rack? Rack { get; set; }


        // ==========================================
        // Location - OPTIONAL
        // ==========================================

     


        // ==========================================
        // Bins
        // ==========================================

        public List<Bin> Bins { get; set; }
            = new List<Bin>();
        public List<Location> Locations { get; set; }= new List<Location>();
    }
}