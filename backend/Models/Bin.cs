using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Bin
    {
        [Key]
        public int Bin_Id { get; set; }


        // ==========================================
        // Warehouse
        // ==========================================

        [Required]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; } = null!;


        // ==========================================
        // Partition
        // ==========================================

        [Required]
        public int PartitionId { get; set; }

        public Partition Partition { get; set; } = null!;


        // ==========================================
        // Bin Details
        // ==========================================

        [Required]
        [MaxLength(50)]
        public string Bin_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Bin_Code { get; set; }

        [MaxLength(500)]
        public string? Bin_Description { get; set; }

        public bool IsActive { get; set; } = true;


        // ==========================================
        // Locations
        // ==========================================

        public ICollection<Location> Locations { get; set; }
            = new List<Location>();


        // ==========================================
        // Stocks
        // ==========================================

        public ICollection<Stock> Stocks { get; set; }
            = new List<Stock>();
    }
}