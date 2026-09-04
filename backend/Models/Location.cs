using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }


        // ==========================================
        // Bin
        // ==========================================

        [Required]
        public int BinId { get; set; }

        public Bin Bin { get; set; } = null!;


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


        // ==========================================
        // Audit
        // ==========================================

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        // ==========================================
        // Stock
        // ==========================================

        public ICollection<Stock> Stocks { get; set; }
            = new List<Stock>();
    }
}