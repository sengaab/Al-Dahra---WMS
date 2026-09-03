using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Bin
    {
        [Key]
        public int Bin_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Bin_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Bin_Code { get; set; }

        [MaxLength(500)]
        public string? Bin_Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Shelf - OPTIONAL
        // ==========================================

        public int? Shelf_Id { get; set; }

        public Shelf? Shelf { get; set; }


        // ==========================================
        // Location - OPTIONAL
        // Location contains the FK: BinId
        // ==========================================

        public Location? Location { get; set; }


        // ==========================================
        // Stocks
        // ==========================================

        public List<Stock> Stocks { get; set; }
            = new List<Stock>();
    }
}