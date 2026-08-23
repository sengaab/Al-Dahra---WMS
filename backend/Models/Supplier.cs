using DocumentFormat.OpenXml.Drawing.Charts;
using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum SupplierStatus
    {
        Active,
        Inactive
    }

    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? TaxNumber { get; set; }

        [MaxLength(50)]
        public string? PaymentTerms { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        public SupplierStatus Status { get; set; }
            = SupplierStatus.Active;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }


        // Navigation
        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
    }
}