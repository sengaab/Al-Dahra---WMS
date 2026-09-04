namespace whm.DTOs.Location
{
    // =====================================================
    // LOCATION DTO
    // =====================================================

    public class LocationDto
    {
        public int LocationId { get; set; }


        // =========================
        // Warehouse
        // =========================

        public int? WarehouseId { get; set; }

        public string? WarehouseName { get; set; }


        // =========================
        // Partition
        // =========================

        public int? PartitionId { get; set; }

        public string? PartitionName { get; set; }

        public string? PartitionCode { get; set; }


        // =========================
        // Bin
        // =========================

        public int BinId { get; set; }

        public string? BinName { get; set; }

        public string? BinCode { get; set; }


        // =========================
        // Location Details
        // =========================

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int ChildrenCount { get; set; }
    }


    // =====================================================
    // CREATE LOCATION
    // POST /api/locations
    // =====================================================

    public class CreateLocationDto
    {
        // =========================
        // Bin
        // =========================

        public int BinId { get; set; }


        // =========================
        // Location Details
        // =========================

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }


    // =====================================================
    // UPDATE LOCATION
    // PUT /api/locations/{id}
    // =====================================================

    public class UpdateLocationDto
    {
        // =========================
        // Bin
        // =========================

        public int? BinId { get; set; }


        // =========================
        // Location Details
        // =========================

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public bool? IsActive { get; set; }
    }


    // =====================================================
    // LOCATION TREE
    // =====================================================

    public class LocationTreeDto
    {
        public int LocationId { get; set; }

        public int BinId { get; set; }

        public int PartitionId { get; set; }

        public int WarehouseId { get; set; }


        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public List<LocationTreeDto> Children { get; set; }
            = new();
    }


    // =====================================================
    // WAREHOUSE TREE
    // =====================================================

    public class WarehouseTreeDto
    {
        public int WarehouseId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public List<LocationTreeDto> Locations { get; set; }
            = new();
    }


    // =====================================================
    // LOCATION OCCUPANCY
    // =====================================================

    public class LocationOccupancyDto
    {
        public int LocationId { get; set; }

        public string LocationName { get; set; } = string.Empty;

        public string LocationType { get; set; } = string.Empty;

        public int TotalStockItems { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalReservedQuantity { get; set; }

        public decimal TotalAvailableQuantity { get; set; }

        public decimal TotalValue { get; set; }

        public bool IsOccupied { get; set; }
    }


    // =====================================================
    // LOCATION STRUCTURE
    // =====================================================

    public class LocationStructureDto
    {
        public int LocationId { get; set; }


        // =========================
        // Warehouse
        // =========================

        public int WarehouseId { get; set; }

        public string? WarehouseName { get; set; }


        // =========================
        // Partition
        // =========================

        public int PartitionId { get; set; }

        public string? PartitionName { get; set; }

        public string? PartitionCode { get; set; }


        // =========================
        // Bin
        // =========================

        public int BinId { get; set; }

        public string? BinName { get; set; }

        public string? BinCode { get; set; }


        // =========================
        // Location
        // =========================

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }


        // =========================
        // Stock
        // =========================

        public int StockCount { get; set; }
    }
}