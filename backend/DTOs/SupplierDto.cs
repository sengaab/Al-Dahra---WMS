namespace whm.DTOs.Supplier
{
    // =====================================================
    // SUPPLIER DTO
    // =====================================================

    public class SupplierDto
    {
        public int SupplierId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ContactName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public string SupplierStatus { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int ProductsCount { get; set; }
    }


    // =====================================================
    // CREATE SUPPLIER DTO
    // =====================================================

    public class CreateSupplierDto
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ContactName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }


    // =====================================================
    // UPDATE SUPPLIER DTO
    // =====================================================

    public class UpdateSupplierDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? ContactName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? SupplierStatus { get; set; }

        public bool? IsActive { get; set; }
    }


    // =====================================================
    // SUPPLIER PRODUCT DTO
    // =====================================================

    public class SupplierProductDto
    {
        public int SupplierProductId { get; set; }

        public int SupplierId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string? SupplierSKU { get; set; }

        public decimal UnitPrice { get; set; }

        public int? LeadTimeDays { get; set; }

        public bool IsPreferred { get; set; }
    }


    // =====================================================
    // CREATE SUPPLIER PRODUCT DTO
    // =====================================================

    public class CreateSupplierProductDto
    {
        public int ProductId { get; set; }

        public string? SupplierSKU { get; set; }

        public decimal UnitPrice { get; set; }

        public int? LeadTimeDays { get; set; }

        public bool IsPreferred { get; set; } = false;
    }


    // =====================================================
    // UPDATE SUPPLIER PRODUCT DTO
    // =====================================================

    public class UpdateSupplierProductDto
    {
        public string? SupplierSKU { get; set; }

        public decimal UnitPrice { get; set; }

        public int? LeadTimeDays { get; set; }

        public bool IsPreferred { get; set; }
    }
}