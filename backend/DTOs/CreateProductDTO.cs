namespace WMS.DTOs
{
    public class CreateProductDTO
    {
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public int UnitId { get; set; }

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }
    }
}
