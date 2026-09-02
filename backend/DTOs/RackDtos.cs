namespace whm.DTOs.Rack
{
    // =====================================================
    // RACK DTO
    // =====================================================

    public class RackDto
    {
        public int RackId { get; set; }

        // Room - OPTIONAL
        public int? RoomId { get; set; }

        public string? RoomName { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? LocationName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int ShelfCount { get; set; }
    }


    // =====================================================
    // CREATE RACK
    // POST /api/racks
    // =====================================================

    public class CreateRackDto
    {
        // Room - OPTIONAL
        public int? RoomId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }


    // =====================================================
    // UPDATE RACK
    // PUT /api/racks/{id}
    // =====================================================

    public class UpdateRackDto
    {
        // Room - OPTIONAL
        public int? RoomId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }
    }
}