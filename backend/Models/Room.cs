using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Room
    {
        [Key]
        public int Room_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Room_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Room_Code { get; set; }

        [MaxLength(500)]
        public string? Room_Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Warehouse - OPTIONAL
        // ==========================================

        public int? Warehouse_Id { get; set; }

        public Warehouse? Warehouse { get; set; }


        // ==========================================
        // Location - OPTIONAL
        // ==========================================

        public int? LocationId { get; set; }

        public Location? Location { get; set; }


        // ==========================================
        // Racks
        // ==========================================

        public List<Rack> Rows { get; set; }
            = new List<Rack>();
    }
}