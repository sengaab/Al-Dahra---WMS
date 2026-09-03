namespace whm.DTOs.Room
{
    // =====================================================
    // ROOM DTO
    // =====================================================

    public class RoomDto
    {
        public int RoomId { get; set; }

        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int RackCount { get; set; }
    }


    // =====================================================
    // CREATE ROOM
    // POST /api/rooms
    // =====================================================

    public class CreateRoomDto
    {
        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }


    // =====================================================
    // UPDATE ROOM
    // PUT /api/rooms/{id}
    // =====================================================

    public class UpdateRoomDto
    {
        // Warehouse - OPTIONAL
        public int? WarehouseId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}