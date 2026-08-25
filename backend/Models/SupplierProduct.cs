using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }

        public int SupplierId { get; set; }

        public int ProductId { get; set; }

        [MaxLength(100)]
        public string? SupplierSKU { get; set; }

        public decimal UnitPrice { get; set; }

        public int? LeadTimeDays { get; set; }

        public bool IsPreferred { get; set; } = false;

        // Navigation
        public Supplier Supplier { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}