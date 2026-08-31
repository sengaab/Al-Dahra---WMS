namespace whm.DTOs.Warehouse
{
    // =====================================================
    // WAREHOUSE DTO
    // =====================================================

    public class WarehouseDto
    {
        public int WarehouseId { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int LocationCount { get; set; }
    }


    // =====================================================
    // CREATE
    // POST /api/warehouses
    // =====================================================

    public class CreateWarehouseDto
    {
        public int SiteId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }


    // =====================================================
    // UPDATE
    // PUT /api/warehouses/{id}
    // =====================================================

    public class UpdateWarehouseDto
    {
        public int? SiteId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }


    // =====================================================
    // LOCATION DTO
    // =====================================================

    public class WarehouseLocationDto
    {
        public int LocationId { get; set; }

        public int WarehouseId { get; set; }

        public int? ParentLocationId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }


    // =====================================================
    // WAREHOUSE STATS
    // =====================================================

    public class WarehouseStatsDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int TotalLocations { get; set; }

        public int ActiveLocations { get; set; }

        public int InactiveLocations { get; set; }

        public int TotalStockItems { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalReservedQuantity { get; set; }

        public decimal TotalAvailableQuantity { get; set; }

        public decimal TotalValue { get; set; }
    }


    // =====================================================
    // OCCUPANCY
    // =====================================================

    public class WarehouseOccupancyDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int TotalLocations { get; set; }

        public int OccupiedLocations { get; set; }

        public int EmptyLocations { get; set; }

        public decimal OccupancyPercentage { get; set; }
    }
}