namespace whm.DTOs.Bin
{
    public class BinDto
    {
        public int BinId { get; set; }

        public int? ShelfId { get; set; }
        public string? ShelfName { get; set; }

        public int? LocationId { get; set; }
        public string? LocationName { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int StockCount { get; set; }
    }

    public class CreateBinDto
    {
        public int? ShelfId { get; set; }

        public int? LocationId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }

    public class UpdateBinDto
    {
        public int? ShelfId { get; set; }

        public int? LocationId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }
    }
}