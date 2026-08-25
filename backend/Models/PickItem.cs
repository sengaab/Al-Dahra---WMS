using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum PickItemStatus
    {
        Pending,
        Picking,
        PartiallyPicked,
        Picked,
        Short,
        Cancelled
    }
    public class PickItem
    {
        [Key]
        public int PickItemId { get; set; }

        [Required]
        public int PickListId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal RequiredQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PickedQuantity { get; set; } = 0;

       
        public PickItemStatus pickItemStatus { get; set; }= PickItemStatus.Pending;

        // =========================
        // Navigation Properties
        // =========================

        public PickList PickList { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public Location Location { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}