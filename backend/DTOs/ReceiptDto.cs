namespace whm.DTOs.Receipt
{
    public class ReceiptDto
    {
        public int ReceiptId { get; set; }

        public string ReceiptNumber { get; set; } = string.Empty;

        public int PurchaseOrderId { get; set; }

        public int WarehouseId { get; set; }

        public Guid ReceivedBy { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public string? Notes { get; set; }

        public string ReceiptStatus { get; set; } = string.Empty;

        public List<ReceiptItemDto> Items { get; set; }
            = new();
    }
    public class CreateReceiptDto
    {
        public int PurchaseOrderId { get; set; }

        public int WarehouseId { get; set; }

        public Guid ReceivedBy { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public string? Notes { get; set; }
    }
    public class UpdateReceiptDto
    {
        public int WarehouseId { get; set; }

        public Guid ReceivedBy { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public string? Notes { get; set; }
    }
    public class ReceiptItemDto
    {
        public int ReceiptItemId { get; set; }

        public int ReceiptId { get; set; }

        public int PurchaseOrderItemId { get; set; }

        public int ProductId { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public decimal AcceptedQuantity { get; set; }

        public decimal QuarantineQuantity { get; set; }

        public decimal RejectedQuantity { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
    public class CreateReceiptItemDto
    {
        public int PurchaseOrderItemId { get; set; }

        public int ProductId { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public decimal AcceptedQuantity { get; set; }

        public decimal QuarantineQuantity { get; set; }

        public decimal RejectedQuantity { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
    public class UpdateReceiptItemDto
    {
        public decimal ReceivedQuantity { get; set; }

        public decimal AcceptedQuantity { get; set; }

        public decimal QuarantineQuantity { get; set; }

        public decimal RejectedQuantity { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
}