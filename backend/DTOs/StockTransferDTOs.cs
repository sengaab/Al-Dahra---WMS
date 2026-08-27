using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    // =====================================================
    // CREATE
    // =====================================================

    public class CreateStockTransferDTO
    {
        [Required]
        public int SourceWarehouseId { get; set; }

        [Required]
        public int DestinationWarehouseId { get; set; }

        [Required]
        public Guid RequestedBy { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateStockTransferItemDTO> Items { get; set; }
            = new();
    }


    public class CreateStockTransferItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SourceStockId { get; set; }

        public int? SourceLocationId { get; set; }

        public int? DestinationLocationId { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0001", "999999999999")]
        public decimal Quantity { get; set; }
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public class UpdateStockTransferDTO
    {
        [Required]
        public int SourceWarehouseId { get; set; }

        [Required]
        public int DestinationWarehouseId { get; set; }

        public List<CreateStockTransferItemDTO> Items { get; set; }
            = new();
    }


    // =====================================================
    // RESPONSE
    // =====================================================

    public class StockTransferResponseDTO
    {
        public int TransferId { get; set; }

        public string TransferNumber { get; set; } = string.Empty;

        public int SourceWarehouseId { get; set; }

        public int DestinationWarehouseId { get; set; }

        public Guid RequestedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public StockTransferStatus TransferStatus { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }

        public List<StockTransferItemResponseDTO> Items { get; set; }
            = new();
    }


    public class StockTransferItemResponseDTO
    {
        public int TransferItemId { get; set; }

        public int ProductId { get; set; }

        public int SourceStockId { get; set; }

        public int? SourceLocationId { get; set; }

        public int? DestinationLocationId { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReceivedQuantity { get; set; }
    }


    // =====================================================
    // WORKFLOW ACTION
    // =====================================================

    public class StockTransferActionDTO
    {
        [Required]
        public Guid UserId { get; set; }
    }


    // =====================================================
    // RECEIVE
    // =====================================================

    public class ReceiveStockTransferDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public List<ReceiveStockTransferItemDTO> Items { get; set; }
            = new();
    }


    public class ReceiveStockTransferItemDTO
    {
        [Required]
        public int TransferItemId { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0001", "999999999999")]
        public decimal ReceivedQuantity { get; set; }
    }
}