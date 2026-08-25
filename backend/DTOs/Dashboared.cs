namespace whm.DTOs.Dashboard
{
    public class DashboardDto
    {
        public DashboardStatsDto Stats { get; set; } = new();

        public List<WarehouseOverviewDto> WarehouseOverview { get; set; }
            = new();

        public List<StockStatusDto> StockStatus { get; set; }
            = new();

        public List<LowStockDto> LowStock { get; set; }
            = new();

        public List<ValueByCategoryDto> ValueByCategory { get; set; }
            = new();

        public List<RecentTransactionDto> RecentTransactions { get; set; }
            = new();

        public List<PendingItemDto> PendingReceipts { get; set; }
            = new();

        public List<PendingItemDto> PendingRequests { get; set; }
            = new();

        public List<PendingItemDto> PendingPickLists { get; set; }
            = new();

        public List<PendingItemDto> PendingTransfers { get; set; }
            = new();
    }


    public class DashboardStatsDto
    {
        public int TotalProducts { get; set; }

        public int TotalStockItems { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalStockValue { get; set; }

        public int LowStockItems { get; set; }

        public int OutOfStockItems { get; set; }

        public int ActiveWarehouses { get; set; }
    }


    public class WarehouseOverviewDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int StockItems { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalValue { get; set; }
    }


    public class StockStatusDto
    {
        public string Status { get; set; } = string.Empty;

        public int Count { get; set; }

        public decimal Quantity { get; set; }
    }


    public class LowStockDto
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal AvailableQuantity { get; set; }

        public decimal MinimumStock { get; set; }
    }


    public class ValueByCategoryDto
    {
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal TotalValue { get; set; }
    }


    public class RecentTransactionDto
    {
        public long TransactionId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string TransactionType { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string? ReferenceType { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }


    public class PendingItemDto
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }
    }
}