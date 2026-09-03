namespace whm.DTOs.PurchaseOrder
{
    public class PurchaseOrderDto
    {
        public int PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public int SiteId { get; set; }
        public string SiteName { get; set; } = string.Empty;

        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset? ExpectedDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal TotalValue { get; set; }

        public Guid CreatedBy { get; set; }
        public string? CreatorName { get; set; }

        public Guid? ApprovedBy { get; set; }
        public string? ApproverName { get; set; }

        public DateTimeOffset? ApprovedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public int ItemsCount { get; set; }
        public decimal TotalOrderedQuantity { get; set; }
        public decimal TotalReceivedQuantity { get; set; }
        public decimal TotalRemainingQuantity { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        public string PONumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }

        public int SiteId { get; set; }

        public DateTimeOffset? OrderDate { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }
    }

    public class UpdatePurchaseOrderDto
    {
        public string? PONumber { get; set; }

        public int? SupplierId { get; set; }

        public int? SiteId { get; set; }

        public DateTimeOffset? OrderDate { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }
    }

    public class CreatePurchaseOrderItemDto
    {
        public int ProductId { get; set; }

        public decimal OrderedQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class UpdatePurchaseOrderItemDto
    {
        public int? ProductId { get; set; }

        public decimal? OrderedQuantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        public int PurchaseOrderItemId { get; set; }

        public int PurchaseOrderId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;

        public decimal OrderedQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class PurchaseOrderReceiptDto
    {
        public int ReceiptId { get; set; }

        public string ReceiptNumber { get; set; } = string.Empty;

        public int PurchaseOrderId { get; set; }

        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;

        public Guid ReceivedBy { get; set; }
        public string? ReceiverName { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public string? Notes { get; set; }

        public string Status { get; set; } = string.Empty;

        public int ItemsCount { get; set; }
    }

    public class PurchaseOrderHistoryDto
    {
        public string EventType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? Status { get; set; }

        public DateTimeOffset Date { get; set; }

        public Guid? UserId { get; set; }

        public string? UserName { get; set; }

        public int? ReceiptId { get; set; }

        public int? ReceiptItemId { get; set; }

        public int? InspectionId { get; set; }
    }
}