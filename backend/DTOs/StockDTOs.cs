namespace whm.DTOs.Stock
{
    // =====================================================
    // STOCK RESPONSE DTO
    // =====================================================

    public class StockDto
    {
        public int StockId { get; set; }


        // =========================
        // Product
        // =========================

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string? Barcode { get; set; }


        // =========================
        // Warehouse
        // =========================

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;


        // =========================
        // Partition
        // =========================

        public int? PartitionId { get; set; }

        public string? PartitionName { get; set; }

        public string? PartitionCode { get; set; }


        // =========================
        // Bin
        // =========================

        public int? BinId { get; set; }

        public string? BinName { get; set; }

        public string? BinCode { get; set; }


        // =========================
        // Location
        // =========================

        public int? LocationId { get; set; }

        public string? LocationName { get; set; }

        public string? LocationCode { get; set; }


        // =========================
        // Supplier
        // =========================

        public int? SupplierId { get; set; }

        public string? SupplierName { get; set; }


        // =========================
        // Stock Details
        // =========================

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


    // =====================================================
    // CREATE STOCK DTO
    // POST /api/stocks
    // =====================================================

    public class CreateStockDto
    {
        public int ProductId { get; set; }

        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        public int? SupplierId { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }
    }


    // =====================================================
    // UPDATE STOCK DTO
    // PUT /api/stocks/{id}
    // =====================================================

    public class UpdateStockDto
    {
        public int? LocationId { get; set; }

        public int? SupplierId { get; set; }

        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public string? StockStatus { get; set; }
    }


    // =====================================================
    // STOCK SUMMARY DTO
    // =====================================================

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