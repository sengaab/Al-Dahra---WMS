using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Partition
    {
        [Key]
        public int PartitionId { get; set; }


        // ==========================================
        // Warehouse
        // ==========================================

        [Required]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; } = null!;


        // ==========================================
        // Partition Details
        // ==========================================

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Audit
        // ==========================================

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        // ==========================================
        // Bins
        // ==========================================

        public ICollection<Bin> Bins { get; set; }
            = new List<Bin>();
    }
}