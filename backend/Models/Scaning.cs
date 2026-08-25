using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class BarcodeScan
    {
        [Key]
        public long ScanId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Barcode { get; set; } = string.Empty;

        public int? ProductId { get; set; }

        public int? StockId { get; set; }

        public int? LocationId { get; set; }

        [Required]
        public Guid ScannedBy { get; set; }

        [Required]
        [MaxLength(50)]
        public string ScanType { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ReferenceType { get; set; }

        public int? ReferenceId { get; set; }

        public DateTimeOffset ScannedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Product? Product { get; set; }

        public Stock? Stock { get; set; }

        public Location? Location { get; set; }

        public User Scanner { get; set; } = null!;
    }
}
