using whm.Models;

namespace whm.DTOs.Stock
{
    public class CreateStockDto
    {
        public decimal Quantity { get; set; }
        public int ProductId { get; set; }
        public int Bin_Id { get; set; }
        public StockStatus StockStatue { get; set; } = StockStatus.InTransit;
        
    }

    public class UpdateStockDto
    {
        public decimal Quantity { get; set; }
        public int Bin_Id { get; set; }
        public bool IsActive { get; set; }
        public StockStatus StockStatue { get; set; }
    }

    public class StockResponseDto
    {
        public int Stock_Id { get; set; }
        public decimal Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public int ProductId { get; set; }
        public int Bin_Id { get; set; }

        public StockStatus StockStatue { get; set; }
    }

    public class UpdateStockStatusDto
    {
        public StockStatus StockStatue { get; set; }
    }
}