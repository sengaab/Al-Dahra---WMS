using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Supplier;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public SupplierController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET: api/Supplier
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var suppliers =
                await unitOfWork.Suppliers.GetAllAsync();

            var result = suppliers.Select(s =>
                new SupplierResponseDto
                {
                    SupplierId = s.SupplierId,

                    SupplierCode =
                        s.SupplierCode,

                    SupplierName =
                        s.SupplierName,

                    ContactPerson =
                        s.ContactPerson,

                    Phone =
                        s.Phone,

                    Email =
                        s.Email,

                    Address =
                        s.Address,

                    Country =
                        s.Country,

                    TaxNumber =
                        s.TaxNumber,

                    PaymentTerms =
                        s.PaymentTerms,

                    Currency =
                        s.Currency,

                    Status =
                        s.Status,

                    Notes =
                        s.Notes,

                    CreatedAt =
                        s.CreatedAt,

                    UpdatedAt =
                        s.UpdatedAt,

                    OrdersCount =
                        s.Orders?.Count ?? 0
                });

            return Ok(result);
        }


        // =====================================================
        // GET BY ID
        // GET: api/Supplier/1
        // =====================================================

        [HttpGet("GetbyId{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier =
                await unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(
                    "Supplier not found.");
            }

            var result =
                new SupplierResponseDto
                {
                    SupplierId =
                        supplier.SupplierId,

                    SupplierCode =
                        supplier.SupplierCode,

                    SupplierName =
                        supplier.SupplierName,

                    ContactPerson =
                        supplier.ContactPerson,

                    Phone =
                        supplier.Phone,

                    Email =
                        supplier.Email,

                    Address =
                        supplier.Address,

                    Country =
                        supplier.Country,

                    TaxNumber =
                        supplier.TaxNumber,

                    PaymentTerms =
                        supplier.PaymentTerms,

                    Currency =
                        supplier.Currency,

                    Status =
                        supplier.Status,

                    Notes =
                        supplier.Notes,

                    CreatedAt =
                        supplier.CreatedAt,

                    UpdatedAt =
                        supplier.UpdatedAt,

                    OrdersCount =
                        supplier.Orders?.Count ?? 0
                };

            return Ok(result);
        }


        // =====================================================
        // GET BY CODE
        // GET: api/Supplier/code/SUP-001
        // =====================================================

        [HttpGet("GetBycode/{code}")]
        public async Task<IActionResult> GetByCode(
            string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(
                    "Supplier code is required.");
            }

            var supplier =
                await unitOfWork.Suppliers
                    .GetByCodeAsync(code);

            if (supplier == null)
            {
                return NotFound(
                    "Supplier not found.");
            }

            return Ok(
                new SupplierResponseDto
                {
                    SupplierId =
                        supplier.SupplierId,

                    SupplierCode =
                        supplier.SupplierCode,

                    SupplierName =
                        supplier.SupplierName,

                    ContactPerson =
                        supplier.ContactPerson,

                    Phone =
                        supplier.Phone,

                    Email =
                        supplier.Email,

                    Address =
                        supplier.Address,

                    Country =
                        supplier.Country,

                    TaxNumber =
                        supplier.TaxNumber,

                    PaymentTerms =
                        supplier.PaymentTerms,

                    Currency =
                        supplier.Currency,

                    Status =
                        supplier.Status,

                    Notes =
                        supplier.Notes,

                    CreatedAt =
                        supplier.CreatedAt,

                    UpdatedAt =
                        supplier.UpdatedAt,

                    OrdersCount =
                        supplier.Orders?.Count ?? 0
                });
        }


        // =====================================================
        // SEARCH
        // GET: api/Supplier/search?search=abc
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest(
                    "Search value is required.");
            }

            var suppliers =
                await unitOfWork.Suppliers
                    .SearchAsync(search);

            var result = suppliers.Select(s =>
                new SupplierResponseDto
                {
                    SupplierId =
                        s.SupplierId,

                    SupplierCode =
                        s.SupplierCode,

                    SupplierName =
                        s.SupplierName,

                    ContactPerson =
                        s.ContactPerson,

                    Phone =
                        s.Phone,

                    Email =
                        s.Email,

                    Address =
                        s.Address,

                    Country =
                        s.Country,

                    TaxNumber =
                        s.TaxNumber,

                    PaymentTerms =
                        s.PaymentTerms,

                    Currency =
                        s.Currency,

                    Status =
                        s.Status,

                    Notes =
                        s.Notes,

                    CreatedAt =
                        s.CreatedAt,

                    UpdatedAt =
                        s.UpdatedAt,

                    OrdersCount =
                        s.Orders?.Count ?? 0
                });

            return Ok(result);
        }


        // =====================================================
        // CREATE
        // POST: api/Supplier
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var code =
                dto.SupplierCode.Trim();

            var name =
                dto.SupplierName.Trim();

            var codeExists =
                await unitOfWork.Suppliers
                    .SupplierCodeExistsAsync(code);

            if (codeExists)
            {
                return Conflict(
                    "Supplier code already exists.");
            }

            var nameExists =
                await unitOfWork.Suppliers
                    .SupplierNameExistsAsync(name);

            if (nameExists)
            {
                return Conflict(
                    "Supplier name already exists.");
            }

            var supplier = new Supplier
            {
                SupplierCode =
                    code,

                SupplierName =
                    name,

                ContactPerson =
                    string.IsNullOrWhiteSpace(
                        dto.ContactPerson)
                        ? null
                        : dto.ContactPerson.Trim(),

                Phone =
                    string.IsNullOrWhiteSpace(dto.Phone)
                        ? null
                        : dto.Phone.Trim(),

                Email =
                    string.IsNullOrWhiteSpace(dto.Email)
                        ? null
                        : dto.Email.Trim(),

                Address =
                    string.IsNullOrWhiteSpace(dto.Address)
                        ? null
                        : dto.Address.Trim(),

                Country =
                    string.IsNullOrWhiteSpace(dto.Country)
                        ? null
                        : dto.Country.Trim(),

                TaxNumber =
                    string.IsNullOrWhiteSpace(
                        dto.TaxNumber)
                        ? null
                        : dto.TaxNumber.Trim(),

                PaymentTerms =
                    string.IsNullOrWhiteSpace(
                        dto.PaymentTerms)
                        ? null
                        : dto.PaymentTerms.Trim(),

                Currency =
                    string.IsNullOrWhiteSpace(
                        dto.Currency)
                        ? null
                        : dto.Currency.Trim(),

                Status =
                    dto.Status,

                Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim(),

                CreatedAt =
                    DateTimeOffset.UtcNow
            };

            await unitOfWork.Suppliers
                .AddAsync(supplier);

            await unitOfWork.SaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = supplier.SupplierId },
                new
                {
                    message =
                        "Supplier created successfully.",

                    supplierId =
                        supplier.SupplierId,

                    supplierCode =
                        supplier.SupplierCode,

                    supplierName =
                        supplier.SupplierName
                });
        }


        // =====================================================
        // UPDATE
        // PUT: api/Supplier/1
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSupplierDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier =
                await unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(
                    "Supplier not found.");
            }


            // =================================================
            // CODE
            // =================================================

            if (dto.SupplierCode != null)
            {
                var code =
                    dto.SupplierCode.Trim();

                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest(
                        "Supplier code cannot be empty.");
                }

                var exists =
                    await unitOfWork.Suppliers
                        .SupplierCodeExistsAsync(
                            code,
                            id);

                if (exists)
                {
                    return Conflict(
                        "Supplier code already exists.");
                }

                supplier.SupplierCode = code;
            }


            // =================================================
            // NAME
            // =================================================

            if (dto.SupplierName != null)
            {
                var name =
                    dto.SupplierName.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(
                        "Supplier name cannot be empty.");
                }

                var exists =
                    await unitOfWork.Suppliers
                        .SupplierNameExistsAsync(
                            name,
                            id);

                if (exists)
                {
                    return Conflict(
                        "Supplier name already exists.");
                }

                supplier.SupplierName = name;
            }


            // =================================================
            // OPTIONAL VALUES
            // =================================================

            if (dto.ContactPerson != null)
            {
                supplier.ContactPerson =
                    string.IsNullOrWhiteSpace(
                        dto.ContactPerson)
                        ? null
                        : dto.ContactPerson.Trim();
            }

            if (dto.Phone != null)
            {
                supplier.Phone =
                    string.IsNullOrWhiteSpace(dto.Phone)
                        ? null
                        : dto.Phone.Trim();
            }

            if (dto.Email != null)
            {
                supplier.Email =
                    string.IsNullOrWhiteSpace(dto.Email)
                        ? null
                        : dto.Email.Trim();
            }

            if (dto.Address != null)
            {
                supplier.Address =
                    string.IsNullOrWhiteSpace(dto.Address)
                        ? null
                        : dto.Address.Trim();
            }

            if (dto.Country != null)
            {
                supplier.Country =
                    string.IsNullOrWhiteSpace(dto.Country)
                        ? null
                        : dto.Country.Trim();
            }

            if (dto.TaxNumber != null)
            {
                supplier.TaxNumber =
                    string.IsNullOrWhiteSpace(
                        dto.TaxNumber)
                        ? null
                        : dto.TaxNumber.Trim();
            }

            if (dto.PaymentTerms != null)
            {
                supplier.PaymentTerms =
                    string.IsNullOrWhiteSpace(
                        dto.PaymentTerms)
                        ? null
                        : dto.PaymentTerms.Trim();
            }

            if (dto.Currency != null)
            {
                supplier.Currency =
                    string.IsNullOrWhiteSpace(
                        dto.Currency)
                        ? null
                        : dto.Currency.Trim();
            }

            if (dto.Status.HasValue)
            {
                supplier.Status =
                    dto.Status.Value;
            }

            if (dto.Notes != null)
            {
                supplier.Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim();
            }

            supplier.UpdatedAt =
                DateTimeOffset.UtcNow;

            unitOfWork.Suppliers
                .Update(supplier);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Supplier updated successfully.",

                supplierId =
                    supplier.SupplierId,

                supplierCode =
                    supplier.SupplierCode,

                supplierName =
                    supplier.SupplierName,

                status =
                    supplier.Status,

                updatedAt =
                    supplier.UpdatedAt
            });
        }


        // =====================================================
        // DELETE
        // DELETE: api/Supplier/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier =
                await unitOfWork.Suppliers
                    .GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound(
                    "Supplier not found.");
            }

            if (supplier.Orders != null &&
                supplier.Orders.Any())
            {
                return Conflict(
                    "Cannot delete supplier because it has orders.");
            }

            unitOfWork.Suppliers
                .Delete(supplier);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Supplier deleted successfully.",

                supplierId =
                    supplier.SupplierId
            });
        }
    }
}