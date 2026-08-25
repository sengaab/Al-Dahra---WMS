using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum StockTransferStatus
    {
        Pending,
        Approved,
        Rejected,
        Picking,
        ReadyToShip,
        InTransit,
        PartiallyReceived,
        Received,
        Completed,
        Cancelled
    }
    public class StockTransfer
    {
        [Key]
        public int TransferId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransferNumber { get; set; } = string.Empty;

        [Required]
        public int SourceWarehouseId { get; set; }

        [Required]
        public int DestinationWarehouseId { get; set; }

        [Required]
        public Guid RequestedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        [Required]
        [MaxLength(50)]
        public StockTransferStatus TransferStatus { get; set; }= StockTransferStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Warehouse SourceWarehouse { get; set; } = null!;

        public Warehouse DestinationWarehouse { get; set; } = null!;

        public User Requester { get; set; } = null!;

        public User? Approver { get; set; }

        public List<StockTransferItem> Items { get; set; }
            = new List<StockTransferItem>();
    }
}
