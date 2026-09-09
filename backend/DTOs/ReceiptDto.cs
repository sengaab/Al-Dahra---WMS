namespace whm.DTOs.Receipt
{
    // =========================================================
    // Receipt DTO
    // =========================================================

    public class ReceiptDto
    {
        public int ReceiptId { get; set; }

        public string ReceiptNumber { get; set; } = string.Empty;

        public int PurchaseOrderId { get; set; }

        public int WarehouseId { get; set; }

        public Guid? ReceivedBy { get; set; }

        public DateTimeOffset? ReceivedAt { get; set; }

        public string? Notes { get; set; }

        public string ReceiptStatus { get; set; } = "Pending";

        public List<ReceiptItemDto> Items { get; set; } = new();
    }


    // =========================================================
    // Create Receipt DTO
    // =========================================================

    public class CreateReceiptDto
    {
        // Required FK
        public int PurchaseOrderId { get; set; }

        // Required FK
        public int WarehouseId { get; set; }

        // Optional
        public Guid? ReceivedBy { get; set; }

        // Optional
        public DateTimeOffset? ReceivedAt { get; set; }

        // Optional
        public string? Notes { get; set; }
    }


    // =========================================================
    // Update Receipt DTO
    // =========================================================

    public class UpdateReceiptDto
    {
        // Required FK
        public int WarehouseId { get; set; }

        // Optional
        public Guid? ReceivedBy { get; set; }

        // Optional
        public DateTimeOffset? ReceivedAt { get; set; }

        // Optional
        public string? Notes { get; set; }
    }


    // =========================================================
    // Receipt Item DTO
    // =========================================================

    public class ReceiptItemDto
    {
        public int ReceiptItemId { get; set; }

        // Required FK
        public int ReceiptId { get; set; }

        // Required FK
        public int PurchaseOrderItemId { get; set; }

        // Required FK
        public int ProductId { get; set; }

        // Optional
        public decimal? ReceivedQuantity { get; set; }

        // Optional
        public decimal? AcceptedQuantity { get; set; }

        // Optional
        public decimal? QuarantineQuantity { get; set; }

        // Optional
        public decimal? RejectedQuantity { get; set; }

        // Optional
        public string? BatchNumber { get; set; }

        // Optional
        public DateOnly? ExpiryDate { get; set; }
    }


    // =========================================================
    // Create Receipt Item DTO
    // =========================================================

    public class CreateReceiptItemDto
    {
        // Required FK
        public int PurchaseOrderItemId { get; set; }

        // Required FK
        public int ProductId { get; set; }

        // Optional
        public decimal? ReceivedQuantity { get; set; }

        // Optional
        public decimal? AcceptedQuantity { get; set; }

        // Optional
        public decimal? QuarantineQuantity { get; set; }

        // Optional
        public decimal? RejectedQuantity { get; set; }

        // Optional
        public string? BatchNumber { get; set; }

        // Optional
        public DateOnly? ExpiryDate { get; set; }
    }


    // =========================================================
    // Update Receipt Item DTO
    // =========================================================

    public class UpdateReceiptItemDto
    {
        // Optional
        public decimal? ReceivedQuantity { get; set; }

        // Optional
        public decimal? AcceptedQuantity { get; set; }

        // Optional
        public decimal? QuarantineQuantity { get; set; }

        // Optional
        public decimal? RejectedQuantity { get; set; }

        // Optional
        public string? BatchNumber { get; set; }

        // Optional
        public DateOnly? ExpiryDate { get; set; }
    }
}