namespace whm.DTOs.Stock
{
    public class StockDto
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int? LocationId { get; set; }

        public string? LocationName { get; set; }

        public string StockCode { get; set; } = string.Empty;

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal AvailableQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public string StockStatus { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
    public class CreateStockDto
    {
        public int ProductId { get; set; }

        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }
    }
    public class UpdateStockDto
    {
        public int? LocationId { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public string? StockStatus { get; set; }
    }
    public class StockSummaryDto
    {
        public int TotalStockItems { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalReservedQuantity { get; set; }

        public decimal TotalAvailableQuantity { get; set; }

        public decimal TotalValue { get; set; }

        public int AvailableItems { get; set; }

        public int QuarantinedItems { get; set; }

        public int DamagedItems { get; set; }

        public int ExpiredItems { get; set; }

        public int BlockedItems { get; set; }

        public int LowStockItems { get; set; }

        public int OutOfStockItems { get; set; }
    }

}
