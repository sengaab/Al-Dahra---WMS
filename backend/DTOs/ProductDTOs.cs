using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int UnitId { get; set; }
        public int subCategoryId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }

       

       
    }


    public class UpdateProductDTO
    {
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int UnitId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }

        [StringLength(100)]
        public string? SKU { get; set; }

        [StringLength(100)]
        public string? Barcode { get; set; }

        
    }
}