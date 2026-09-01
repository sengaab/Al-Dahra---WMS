namespace whm.DTOs.Bin
{
    // =====================================================
    // BIN DTO
    // =====================================================

    public class BinDto
    {
        public int BinId { get; set; }

        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }

        public string? ShelfName { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? LocationName { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int StockCount { get; set; }
    }


    // =====================================================
    // CREATE BIN
    // POST /api/bins
    // =====================================================

    public class CreateBinDto
    {
        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }


    // =====================================================
    // UPDATE BIN
    // PUT /api/bins/{id}
    // =====================================================

    public class UpdateBinDto
    {
        // Shelf - OPTIONAL
        public int? ShelfId { get; set; }

        // Location - OPTIONAL
        public int? LocationId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }
    }
}