using whm.Models;

namespace whm.DTOs.Stock
{
    public class CreateStockDto
    {
        public decimal Quantity { get; set; }

        public int ProductId { get; set; }

        // Optional
        public int? Bin_Id { get; set; }

        // Optional
        public int? UnitId { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }

        public int ReservedQuantity { get; set; } = 0;

        public StockStatus StockStatus { get; set; }
            = StockStatus.Available;

        public DeliveryStatus DeliveryStatus { get; set; }
            = DeliveryStatus.Pending;
    }


    public class UpdateStockDto
    {
        public decimal? Quantity { get; set; }

        public int? ReservedQuantity { get; set; }

        public decimal? UnitPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int? MinimumStock { get; set; }

        public int? UnitId { get; set; }

        public int? Bin_Id { get; set; }

        public bool? IsActive { get; set; }

        public StockStatus? StockStatus { get; set; }

        public DeliveryStatus? DeliveryStatus { get; set; }
    }


    public class UpdateStockStatusDto
    {
        public StockStatus StockStatus { get; set; }
    }


    public class UpdateDeliveryStatusDto
    {
        public DeliveryStatus DeliveryStatus { get; set; }
    }


    public class StockResponseDto
    {
        public int Stock_Id { get; set; }

        public string StockCode { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public int ReservedQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }

        // Optional
        public int? UnitId { get; set; }

        public int ProductId { get; set; }

        // Optional
        public int? Bin_Id { get; set; }

        public StockStatus StockStatus { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
    public class InventoryStockResponseDto
    {
        public int Stock_Id { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }

        public List<string> Aliases { get; set; } = new();

        public string? Location { get; set; }

        public string LotBatch { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string? UOM { get; set; }

        public decimal Available { get; set; }

        public int Reserved { get; set; }

        public StockStatus Status { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}