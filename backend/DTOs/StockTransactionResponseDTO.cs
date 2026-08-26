using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class StockTransactionResponseDTO
    {
        public long TransactionId { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? SKU { get; set; }

        public int? StockId { get; set; }
        public string? StockCode { get; set; }

        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        public int? LocationId { get; set; }
        public string? LocationName { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public int? SourceLocationId { get; set; }
        public string? SourceLocationName { get; set; }

        public int? DestinationLocationId { get; set; }
        public string? DestinationLocationName { get; set; }

        public Guid? PerformedBy { get; set; }
        public string? PerformerName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }

        public string? Notes { get; set; }
    }
}
