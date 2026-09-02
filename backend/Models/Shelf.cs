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

        public int? Row_Id { get; set; }

        public Rack? Row { get; set; }


        // ==========================================
        // Location - OPTIONAL
        // ==========================================

        public int? LocationId { get; set; }

        public Location? Location { get; set; }


        // ==========================================
        // Bins
        // ==========================================

        public List<Bin> Bins { get; set; }
            = new List<Bin>();
    }
}