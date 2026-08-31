using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Supplier;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuppliersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET /api/suppliers
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers =
                await _unitOfWork.Suppliers.GetAllAsync();

            return Ok(suppliers);
        }


        // =====================================================
        // GET BY ID
        // GET /api/suppliers/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }

            return Ok(supplier);
        }


        // =====================================================
        // CREATE
        // POST /api/suppliers
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(
            [FromBody] CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Supplier code is required."
                });
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Supplier name is required."
                });
            }


            var existing =
                await _unitOfWork.Suppliers
                    .GetByCodeAsync(dto.Code);

            if (existing != null)
            {
                return Conflict(new
                {
                    message =
                        "A supplier with this code already exists."
                });
            }


            var supplier = new Supplier
            {
                Code = dto.Code.Trim(),

                Name = dto.Name.Trim(),

                ContactName =
                    string.IsNullOrWhiteSpace(dto.ContactName)
                        ? null
                        : dto.ContactName.Trim(),

                Email =
                    string.IsNullOrWhiteSpace(dto.Email)
                        ? null
                        : dto.Email.Trim(),

                Phone =
                    string.IsNullOrWhiteSpace(dto.Phone)
                        ? null
                        : dto.Phone.Trim(),

                Address =
                    string.IsNullOrWhiteSpace(dto.Address)
                        ? null
                        : dto.Address.Trim(),

                IsActive = true,

                SupplierStatus =
                    SupplierStatus.Active,

                CreatedAt =
                    DateTimeOffset.UtcNow,

                UpdatedAt =
                    DateTimeOffset.UtcNow
            };


            await _unitOfWork.Suppliers
                .AddAsync(supplier);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(supplier.SupplierId);


            return CreatedAtAction(
                nameof(GetSupplier),
                new
                {
                    id = supplier.SupplierId
                },
                result);
        }


        // =====================================================
        // UPDATE
        // PUT /api/suppliers/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSupplier(
            int id,
            [FromBody] UpdateSupplierDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            var supplier =
                await _unitOfWork.Suppliers
                    .GetEntityByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            // -------------------------------------------------
            // CODE
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var existing =
                    await _unitOfWork.Suppliers
                        .GetByCodeAsync(dto.Code);

                if (existing != null &&
                    existing.SupplierId != id)
                {
                    return Conflict(new
                    {
                        message =
                            "A supplier with this code already exists."
                    });
                }

                supplier.Code = dto.Code.Trim();
            }


            // -------------------------------------------------
            // NAME
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                supplier.Name =
                    dto.Name.Trim();
            }


            // -------------------------------------------------
            // CONTACT
            // -------------------------------------------------

            if (dto.ContactName != null)
            {
                supplier.ContactName =
                    string.IsNullOrWhiteSpace(dto.ContactName)
                        ? null
                        : dto.ContactName.Trim();
            }


            // -------------------------------------------------
            // EMAIL
            // -------------------------------------------------

            if (dto.Email != null)
            {
                supplier.Email =
                    string.IsNullOrWhiteSpace(dto.Email)
                        ? null
                        : dto.Email.Trim();
            }


            // -------------------------------------------------
            // PHONE
            // -------------------------------------------------

            if (dto.Phone != null)
            {
                supplier.Phone =
                    string.IsNullOrWhiteSpace(dto.Phone)
                        ? null
                        : dto.Phone.Trim();
            }


            // -------------------------------------------------
            // ADDRESS
            // -------------------------------------------------

            if (dto.Address != null)
            {
                supplier.Address =
                    string.IsNullOrWhiteSpace(dto.Address)
                        ? null
                        : dto.Address.Trim();
            }


            // -------------------------------------------------
            // STATUS
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                dto.SupplierStatus))
            {
                if (!Enum.TryParse<SupplierStatus>(
                    dto.SupplierStatus,
                    true,
                    out var status))
                {
                    return BadRequest(new
                    {
                        message =
                            "Invalid supplier status."
                    });
                }

                supplier.SupplierStatus =
                    status;

                supplier.IsActive =
                    status == SupplierStatus.Active;
            }


            // -------------------------------------------------
            // ACTIVE
            // -------------------------------------------------

            if (dto.IsActive.HasValue)
            {
                supplier.IsActive =
                    dto.IsActive.Value;
            }


            supplier.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Suppliers
                .Update(supplier);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE
        // DELETE /api/suppliers/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers
                    .GetEntityByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            _unitOfWork.Suppliers
                .Delete(supplier);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Supplier deleted successfully."
            });
        }


        // =====================================================
        // GET PRODUCTS
        // GET /api/suppliers/{id}/products
        // =====================================================

        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetProducts(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            var products =
                await _unitOfWork.Suppliers
                    .GetProductsAsync(id);

            return Ok(products);
        }


        // =====================================================
        // ADD PRODUCT TO SUPPLIER
        // POST /api/suppliers/{id}/products
        // =====================================================

        [HttpPost("{id:int}/products")]
        public async Task<IActionResult> AddProduct(
            int id,
            [FromBody] CreateSupplierProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }


            var existing =
                await _unitOfWork.Suppliers
                    .GetSupplierProductAsync(
                        id,
                        dto.ProductId);

            if (existing != null)
            {
                return Conflict(new
                {
                    message =
                        "This product is already assigned to this supplier."
                });
            }


            if (dto.UnitPrice < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Unit price cannot be negative."
                });
            }


            if (dto.LeadTimeDays.HasValue &&
                dto.LeadTimeDays.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Lead time cannot be negative."
                });
            }


            var supplierProduct =
                new SupplierProduct
                {
                    SupplierId = id,

                    ProductId = dto.ProductId,

                    SupplierSKU =
                        string.IsNullOrWhiteSpace(
                            dto.SupplierSKU)
                            ? null
                            : dto.SupplierSKU.Trim(),

                    UnitPrice =
                        dto.UnitPrice,

                    LeadTimeDays =
                        dto.LeadTimeDays,

                    IsPreferred =
                        dto.IsPreferred
                };


            await _unitOfWork.Suppliers
                .AddSupplierProductAsync(
                    supplierProduct);

            await _unitOfWork.SaveAsync();


            var products =
                await _unitOfWork.Suppliers
                    .GetProductsAsync(id);

            var result =
                products.FirstOrDefault(
                    x => x.ProductId == dto.ProductId);


            return Created(
                $"/api/suppliers/{id}/products/{dto.ProductId}",
                result);
        }


        // =====================================================
        // UPDATE SUPPLIER PRODUCT
        // PUT /api/suppliers/{id}/products/{productId}
        // =====================================================

        [HttpPut("{id:int}/products/{productId:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            int productId,
            [FromBody] UpdateSupplierProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            var supplierProduct =
                await _unitOfWork.Suppliers
                    .GetSupplierProductAsync(
                        id,
                        productId);

            if (supplierProduct == null)
            {
                return NotFound(new
                {
                    message =
                        "This product is not assigned to this supplier."
                });
            }


            if (dto.UnitPrice < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Unit price cannot be negative."
                });
            }


            if (dto.LeadTimeDays.HasValue &&
                dto.LeadTimeDays.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Lead time cannot be negative."
                });
            }


            supplierProduct.SupplierSKU =
                string.IsNullOrWhiteSpace(
                    dto.SupplierSKU)
                    ? null
                    : dto.SupplierSKU.Trim();

            supplierProduct.UnitPrice =
                dto.UnitPrice;

            supplierProduct.LeadTimeDays =
                dto.LeadTimeDays;

            supplierProduct.IsPreferred =
                dto.IsPreferred;


            _unitOfWork.Suppliers
                .UpdateSupplierProduct(
                    supplierProduct);

            await _unitOfWork.SaveAsync();


            var products =
                await _unitOfWork.Suppliers
                    .GetProductsAsync(id);

            var result =
                products.FirstOrDefault(
                    x => x.ProductId == productId);

            return Ok(result);
        }


        // =====================================================
        // DELETE SUPPLIER PRODUCT
        // DELETE /api/suppliers/{id}/products/{productId}
        // =====================================================

        [HttpDelete("{id:int}/products/{productId:int}")]
        public async Task<IActionResult> DeleteProduct(
            int id,
            int productId)
        {
            var supplierProduct =
                await _unitOfWork.Suppliers
                    .GetSupplierProductAsync(
                        id,
                        productId);

            if (supplierProduct == null)
            {
                return NotFound(new
                {
                    message =
                        "This product is not assigned to this supplier."
                });
            }


            _unitOfWork.Suppliers
                .DeleteSupplierProduct(
                    supplierProduct);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Product removed from supplier successfully."
            });
        }


        // =====================================================
        // PURCHASE ORDERS
        // GET /api/suppliers/{id}/purchase-orders
        // =====================================================

        [HttpGet("{id:int}/purchase-orders")]
        public async Task<IActionResult> GetPurchaseOrders(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            // TODO:
            // Implement after providing PurchaseOrder model/repository.

            return Ok(new List<object>());
        }


        // =====================================================
        // RECEIPTS
        // GET /api/suppliers/{id}/receipts
        // =====================================================

        [HttpGet("{id:int}/receipts")]
        public async Task<IActionResult> GetReceipts(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            // TODO:
            // Implement after providing Receipt model/repository.

            return Ok(new List<object>());
        }


        // =====================================================
        // PERFORMANCE
        // GET /api/suppliers/{id}/performance
        // =====================================================

        [HttpGet("{id:int}/performance")]
        public async Task<IActionResult> GetPerformance(int id)
        {
            var supplier =
                await _unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(new
                {
                    message = "Supplier not found."
                });
            }


            // TODO:
            // Performance depends on PurchaseOrder,
            // Receipt, Inspection and delivery fields.

            return Ok(new
            {
                supplierId = id,
                message =
                    "Supplier performance will be calculated from purchase orders, receipts and inspections."
            });
        }
    }
}