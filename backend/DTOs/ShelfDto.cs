namespace whm.DTOs.Shelf
{
    // =====================================================
    // SHELF DTO
    // =====================================================

    public class ShelfDto
    {
        public int ShelfId { get; set; }

        // Rack - OPTIONAL
        public int? RackId { get; set; }

        public string? RackName { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? LocationName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int BinCount { get; set; }
    }


    // =====================================================
    // CREATE SHELF
    // POST /api/shelves
    // =====================================================

    public class CreateShelfDto
    {
        // Rack - OPTIONAL
        public int? RackId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }


    // =====================================================
    // UPDATE SHELF
    // PUT /api/shelves/{id}
    // =====================================================

    public class UpdateShelfDto
    {
        // Rack - OPTIONAL
        public int? RackId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }
    }
}