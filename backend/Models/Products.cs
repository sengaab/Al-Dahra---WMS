using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using whm.Models;

namespace whm.Models
{
   
        public enum ProductStatus
        {
            Active,
            Inactive,
            Discontinued
        }

          
}
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string SKU { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [Required]
    [MaxLength(200)]
    public string QRValue { get; set; } = string.Empty;

    // Foreign Key
    [ForeignKey(nameof(Categories))]
    public int CategoryId { get; set; }

    // Navigation Property
    public Categories Category { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    public int MinimumStock { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation Property
   
    [ForeignKey(nameof(Units))]
    public int UnitId { get; set; }

    public Unit Units { get; set; } = null!;
    //Stock 
    public List<Stock>Stock { get; set; } = new List<Stock>();
    public List<Report> reports { get; set; } = new List<Report>();
    public List<ReportSchedule> reportSchedules { get; set; } = new List<ReportSchedule>();
    public List<Transaction> transactions { get; set; }= new List<Transaction>();
}



