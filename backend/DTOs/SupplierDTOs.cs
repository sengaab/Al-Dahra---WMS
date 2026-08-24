using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs.Supplier
{
    public class CreateSupplierDto
    {
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
    }


    public class UpdateSupplierDto
    {
        [MaxLength(50)]
        public string? SupplierCode { get; set; }

        [MaxLength(200)]
        public string? SupplierName { get; set; }

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

        public SupplierStatus? Status { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }


    public class SupplierResponseDto
    {
        public int SupplierId { get; set; }

        public string SupplierCode { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string? ContactPerson { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public string? TaxNumber { get; set; }

        public string? PaymentTerms { get; set; }

        public string? Currency { get; set; }

        public SupplierStatus Status { get; set; }

        public string? Notes { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public int OrdersCount { get; set; }
    }
}