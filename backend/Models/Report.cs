using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum ReportType
    {
        StockMovement,
        StockSummary,
        LowStock,
        ProductInventory
    }

    public class Report
    {
        [Key]
        public int Report_Id { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public DateTimeOffset FromDate { get; set; }
        [Required]
        public DateTimeOffset ToDate { get; set; }
        [ForeignKey(nameof(craeteByUserId))]
        public Guid craeteByUserId { get; set; }
        public Users CreateByUser { get; set; }
        public DateTimeOffset CreateAt { get; set; }=DateTimeOffset.Now;
        [ForeignKey(nameof(Warehouse))]
        public int?Warehouse_Id { get; set; }
        public Warehouse? Warehouses { get; set; }
        [ForeignKey(nameof(Product))]
        public int? Product_Id { get; set; }
        public Product? Products { get; set; }



    }
}
