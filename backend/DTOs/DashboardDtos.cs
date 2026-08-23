namespace whm.Models
{
    public class DashboardResponse
    {
        public DashboardStatsDto Stats { get; set; } = new();

        public List<CategoryValueDto> ValueByCategory { get; set; } = new();

        public List<WarehouseOverviewDto> WarehouseOverview { get; set; } = new();

        public List<StockStatusDto> StockStatus { get; set; } = new();

        public IncomingStockDto IncomingStock { get; set; } = new();

        public List<RecentActivityDto> RecentActivity { get; set; } = new();
    }

    public class DashboardStatsDto
    {
        public int TotalSkus { get; set; }

        public decimal StockUnits { get; set; }

        public decimal StockValue { get; set; }

        public int LowStock { get; set; }
    }

    public class CategoryValueDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Value { get; set; }
    }

    public class WarehouseOverviewDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public string SiteName { get; set; } = string.Empty;

        public int Skus { get; set; }

        public decimal Units { get; set; }

        public decimal Occupancy { get; set; }

        public string StockStatus { get; set; } = "Good";
    }

    public class StockStatusDto
    {
        public string Status { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal Percentage { get; set; }
    }

    public class IncomingStockDto
    {
        public int ExpectedToday { get; set; }

        public int ExpectedThisWeek { get; set; }

        public int PendingReceiving { get; set; }

        public int InTransit { get; set; }

        public List<IncomingStockOrderDto> Orders { get; set; } = new();
    }

    public class IncomingStockOrderDto
    {
        public string PORef { get; set; } = string.Empty;

        public string Supplier { get; set; } = string.Empty;

        public decimal Units { get; set; }

        public DateTime? ExpectedDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }

    public class RecentActivityDto
    {
        public DateTimeOffset Time { get; set; }

        public string User { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? From { get; set; }

        public string? To { get; set; }
    }
}