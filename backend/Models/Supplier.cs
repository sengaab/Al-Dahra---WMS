using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum SupplierStatus
    {
        Active,
        Inactive,
        Suspended,
        Blocked
    }
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ContactName { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        
       
        [Required]

        public SupplierStatus SupplierStatus { get; set; }= SupplierStatus.Active;
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public ICollection<SupplierProduct> SupplierProducts { get; set; }
            = new List<SupplierProduct>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();
    }
}