namespace whm.DTOs
{
    public class ImportProductDTO
    {
        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int UnitId { get; set; }

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }

        public string? Barcode { get; set; }
    }
}