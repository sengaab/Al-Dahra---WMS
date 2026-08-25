using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum ProductStatus
    {
        Active,
        Inactive,
        Discontinued
    }
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? Barcode { get; set; }
        public string? QRValue { get; set; }


        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        [Required]
        public ProductStatus ProductStatus { get; set; }=ProductStatus.Active;

        public int? UnitId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public bool IsActive { get; set; } = true;

      

        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public Category? Category { get; set; }

        public Unit? Unit { get; set; }
        

        public ICollection<SupplierProduct> SupplierProducts { get; set; }= new List<SupplierProduct>();
        public ICollection<BarcodeScan> BarcodeScans { get; set; }  = new List<BarcodeScan>();
    }
}