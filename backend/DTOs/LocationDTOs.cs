namespace whm.DTOs.Location
{
    // =====================================================
    // LOCATION DTO
    // =====================================================

    public class LocationDto
    {
        public int LocationId { get; set; }

        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        // Parent Location - OPTIONAL
        public int? ParentLocationId { get; set; }
        public string? ParentLocationName { get; set; }

        // Room - OPTIONAL
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }

        // Rack - OPTIONAL
        public int? RackId { get; set; }
        public string? RackName { get; set; }

        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }
        public string? ShelfName { get; set; }

        // Bin - OPTIONAL
        public int? BinId { get; set; }
        public string? BinName { get; set; }

        // Location Details
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
        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }

        // Parent Location - OPTIONAL
        public int? ParentLocationId { get; set; }

        // Room - OPTIONAL
        public int? RoomId { get; set; }

        // Rack - OPTIONAL
        public int? RackId { get; set; }

        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }

        // Bin - OPTIONAL
        public int? BinId { get; set; }

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
        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }

        // Parent Location - OPTIONAL
        public int? ParentLocationId { get; set; }

        // Room - OPTIONAL
        public int? RoomId { get; set; }

        // Rack - OPTIONAL
        public int? RackId { get; set; }

        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }

        // Bin - OPTIONAL
        public int? BinId { get; set; }

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

        public int? WarehouseId { get; set; }

        public int? RoomId { get; set; }

        public int? RackId { get; set; }

        public int? ShelfId { get; set; }

        public int? BinId { get; set; }

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

        public int? WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public int? ParentLocationId { get; set; }

        public string? ParentLocationName { get; set; }

        public int? RoomId { get; set; }

        public int? RackId { get; set; }

        public int? ShelfId { get; set; }

        public int? BinId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // ROOMS
        public List<LocationRoomDto> Rooms { get; set; }
            = new();

        // RACKS
        public List<LocationRackDto> Racks { get; set; }
            = new();

        // SHELVES
        public List<LocationShelfDto> Shelves { get; set; }
            = new();

        // BINS
        public List<LocationBinDto> Bins { get; set; }
            = new();
    }


    // =====================================================
    // LOCATION ROOM
    // =====================================================

    public class LocationRoomDto
    {
        public int RoomId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int RackCount { get; set; }
    }


    // =====================================================
    // LOCATION RACK
    // =====================================================

    public class LocationRackDto
    {
        public int RackId { get; set; }

        public int? RoomId { get; set; }

        public string? RoomName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int ShelfCount { get; set; }
    }


    // =====================================================
    // LOCATION SHELF
    // =====================================================

    public class LocationShelfDto
    {
        public int ShelfId { get; set; }

        public int? RackId { get; set; }

        public string? RackName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int BinCount { get; set; }
    }


    // =====================================================
    // LOCATION BIN
    // =====================================================

    public class LocationBinDto
    {
        public int BinId { get; set; }

        public int? ShelfId { get; set; }

        public string? ShelfName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int StockCount { get; set; }
    }
}