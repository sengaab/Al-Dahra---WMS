using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs.Order
{
    // =====================================================
    // CREATE ORDER ITEM
    // =====================================================

    public class CreateOrderItemDto
    {
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
        public decimal ReceivedQuantity { get; set; } = 0;
    }


    // =====================================================
    // UPDATE ORDER ITEM
    // =====================================================

    public class UpdateOrderItemDto
    {
        [Range(0.01, double.MaxValue)]
        public decimal? Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 100)]
        public decimal? TaxRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ReceivedQuantity { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }


    // =====================================================
    // CREATE ORDER
    // =====================================================

    public class CreateOrderDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }

        public OrderStatus Status { get; set; }
            = OrderStatus.Draft;

        public OrderPriority Priority { get; set; }
            = OrderPriority.Normal;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDto> Items { get; set; }
            = new();
    }


    // =====================================================
    // UPDATE ORDER
    // =====================================================

    public class UpdateOrderDto
    {
        public int? SupplierId { get; set; }

        public int? WarehouseId { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }

        public OrderStatus? Status { get; set; }

        public OrderPriority? Priority { get; set; }

        public Guid? ApprovedBy { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }


    // =====================================================
    // ORDER RESPONSE
    // =====================================================

    public class OrderResponseDto
    {
        public int OrderId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public int WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }

        public OrderStatus Status { get; set; }

        public OrderPriority Priority { get; set; }

        public string? Notes { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public List<OrderItemResponseDto> Items { get; set; }
            = new();
    }


    // =====================================================
    // ORDER ITEM RESPONSE
    // =====================================================

    public class OrderItemResponseDto
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? SKU { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public decimal RemainingQuantity { get; set; }

        public string? Notes { get; set; }
    }
}