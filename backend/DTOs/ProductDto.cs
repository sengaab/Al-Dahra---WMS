namespace whm.DTOs.Product
{
    // =====================================================
    // PRODUCT DTO
    // =====================================================

    public class ProductDto
    {
        public int ProductId { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        public string? QRValue { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int? UnitId { get; set; }

        public string? UnitName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public string? Description { get; set; }

        public string ProductStatus { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public List<ProductSupplierDto> Suppliers { get; set; }
            = new();
    }


    // =====================================================
    // PRODUCT SUPPLIER DTO
    // =====================================================

    public class ProductSupplierDto
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = string.Empty;
    }


    // =====================================================
    // CREATE PRODUCT DTO
    // POST /api/products
    // =====================================================

    public class CreateProductDto
    {
        public string SKU { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        public string? QRValue { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public int? UnitId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumStock { get; set; }

        public string? Description { get; set; }
    }


    // =====================================================
    // UPDATE PRODUCT DTO
    // PUT /api/products/{id}
    // =====================================================

    public class UpdateProductDto
    {
        public string? Barcode { get; set; }

        public string? QRValue { get; set; }

        public string? Name { get; set; }

        public int? CategoryId { get; set; }

        public int? UnitId { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? MinimumStock { get; set; }

        public string? Description { get; set; }

        public string? ProductStatus { get; set; }

        public bool? IsActive { get; set; }
    }
}