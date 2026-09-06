using System.ComponentModel.DataAnnotations;

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

        // Number of partitions inside this warehouse
        public int PartitionsCount { get; set; }

        // Number of bins inside this warehouse
        public int BinsCount { get; set; }

        // Number of locations inside this warehouse
        public int LocationsCount { get; set; }
    }


    // =====================================================
    // CREATE
    // POST /api/warehouses
    // =====================================================

    public class CreateWarehouseDto
    {
        [Required]
        public int SiteId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }


    // =====================================================
    // UPDATE
    // PUT /api/warehouses/{id}
    // =====================================================

    public class UpdateWarehouseDto
    {
        public int? SiteId { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }


    // =====================================================
    // WAREHOUSE STATS
    // =====================================================

    public class WarehouseStatsDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        // =================================================
        // STRUCTURE
        // =================================================

        public int TotalPartitions { get; set; }

        public int TotalBins { get; set; }

        public int TotalLocations { get; set; }

        public int ActiveLocations { get; set; }

        public int InactiveLocations { get; set; }

        // =================================================
        // STOCK
        // =================================================

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