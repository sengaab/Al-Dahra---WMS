using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.Services;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IQRCodeService qrCodeService;
        private readonly IBarcodeService barcodeService;

        public ProductsController(
            IUnitOfWork unitOfWork,
            IQRCodeService qrCodeService,
            IBarcodeService barcodeService)
        {
            this.unitOfWork = unitOfWork;
            this.qrCodeService = qrCodeService;
            this.barcodeService = barcodeService;
        }


        // =====================================================
        // 1. CREATE PRODUCT
        // POST: api/Products/create
        // =====================================================

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(
            CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // CHECK CATEGORY
            // =================================================

            var categoryExists =
                await unitOfWork.Categories
                    .GetByIdAsync(dto.CategoryId);

            if (categoryExists == null)
            {
                return BadRequest(
                    "Category not found.");
            }


            // =================================================
            // CHECK UNIT
            // =================================================

            var unitExists =
                await unitOfWork.Units
                    .GetByIdAsync(dto.UnitId);

            if (unitExists == null)
            {
                return BadRequest(
                    "Unit not found.");
            }


            // =================================================
            // GENERATE / CHECK SKU
            // =================================================

            string sku;

            do
            {
                sku =
                    $"PRD-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

            } while (
                await unitOfWork.Products
                    .SKUExistsAsync(sku)
            );
       


            // =================================================
            // GENERATE / CHECK BARCODE
            // =================================================

            string barcode;

            if (string.IsNullOrWhiteSpace(dto.Barcode))
            {
                do
                {
                    barcode =
                        barcodeService.GenerateBarcodeValue();

                } while (
                    await unitOfWork.Products
                        .BarcodeExistsAsync(barcode)
                );
            }
            else
            {
                barcode = dto.Barcode.Trim();

                if (await unitOfWork.Products
                    .BarcodeExistsAsync(barcode))
                {
                    return Conflict(
                        "Barcode already exists.");
                }
            }


            // =================================================
            // CREATE PRODUCT
            // =================================================

            var product = new Product
            {
                ProductName =
                    dto.ProductName.Trim(),

                CategoryId =
                    dto.CategoryId,

                UnitId =
                    dto.UnitId,

                UnitPrice =
                    dto.UnitPrice,

                MinimumStock =
                    dto.MinimumStock,

                SKU =
                    sku,

                // Barcode
                Barcode =
                    barcode,

                // QR uses SAME value as Barcode
                QRValue =
                    barcode,

                CreatedAt =
                    DateTimeOffset.UtcNow,

                UpdatedAt =
                    null
            };


            await unitOfWork.Products
                .AddAsync(product);

            await unitOfWork.SaveAsync();


            // =================================================
            // GENERATE QR IMAGE
            // =================================================

            var qrImage =
                qrCodeService.GenerateQRCode(
                    product.QRValue);


            // =================================================
            // RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Product created successfully.",

                productId =
                    product.ProductId,

                productName =
                    product.ProductName,

                sku =
                    product.SKU,

                barcode =
                    product.Barcode,

                // Same as Barcode
                qrValue =
                    product.QRValue,

                qrCode =
                    $"data:image/png;base64,{Convert.ToBase64String(qrImage)}",

                categoryId =
                    product.CategoryId,

                unitId =
                    product.UnitId,

                unitPrice =
                    product.UnitPrice,

                minimumStock =
                    product.MinimumStock,

                status =
                    product.Status,

                createdAt =
                    product.CreatedAt
            });
        }


        // =====================================================
        // 2. GET ALL PRODUCTS
        // GET: api/Products/Getall
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products =
                await unitOfWork.Products
                    .GetAllAsync();

            var result =
                products.Select(p => new
                {
                    productId =
                        p.ProductId,

                    productName =
                        p.ProductName,

                    sku =
                        p.SKU,

                    barcode =
                        p.Barcode,

                    qrValue =
                        p.QRValue,

                    categoryId =
                        p.CategoryId,

                    categoryName =
                        p.Category != null
                            ? p.Category.Category_Name
                            : null,

                    unitId =
                        p.UnitId,

                    unitName =
                        p.Units != null
                            ? p.Units.Unit_Name
                            : null,

                    unitPrice =
                        p.UnitPrice,

                    minimumStock =
                        p.MinimumStock,

                    status =
                        p.Status,

                    createdAt =
                        p.CreatedAt,

                    updatedAt =
                        p.UpdatedAt
                });

            return Ok(result);
        }


        // =====================================================
        // 3. GET PRODUCT BY ID
        // GET: api/Products/GetProductBy/1
        // =====================================================

        [HttpGet("GetProductBy/{id:int}")]
        public async Task<IActionResult> GetProductById(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(new
            {
                productId =
                    product.ProductId,

                productName =
                    product.ProductName,

                sku =
                    product.SKU,

                barcode =
                    product.Barcode,

                qrValue =
                    product.QRValue,

                categoryId =
                    product.CategoryId,

                categoryName =
                    product.Category != null
                        ? product.Category.Category_Name
                        : null,

                unitId =
                    product.UnitId,

                unitName =
                    product.Units != null
                        ? product.Units.Unit_Name
                        : null,

                unitPrice =
                    product.UnitPrice,

                minimumStock =
                    product.MinimumStock,

                status =
                    product.Status,

                createdAt =
                    product.CreatedAt,

                updatedAt =
                    product.UpdatedAt
            });
        }


        // =====================================================
        // 4. GET BY SKU
        // GET: api/Products/SKU/{sku}
        // =====================================================

        [HttpGet("SKU/{sku}")]
        public async Task<IActionResult> GetProductBySKU(
            string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return BadRequest(
                    "SKU is required.");
            }

            var product =
                await unitOfWork.Products
                    .GetBySKUAsync(sku.Trim());

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(product);
        }


        // =====================================================
        // 5. GET BY BARCODE
        // GET: api/Products/Barcode/{barcode}
        // =====================================================

        [HttpGet("Barcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(
            string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(
                    "Barcode is required.");
            }

            var product =
                await unitOfWork.Products
                    .GetByBarcodeAsync(
                        barcode.Trim());

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(product);
        }


        // =====================================================
        // 6. GET BY QR
        // GET: api/Products/QR/{qrValue}
        // =====================================================

        [HttpGet("QR/{qrValue}")]
        public async Task<IActionResult> GetProductByQR(
            string qrValue)
        {
            if (string.IsNullOrWhiteSpace(qrValue))
            {
                return BadRequest(
                    "QR value is required.");
            }

            var product =
                await unitOfWork.Products
                    .GetByQRValueAsync(
                        qrValue.Trim());

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(new
            {
                productId =
                    product.ProductId,

                productName =
                    product.ProductName,

                sku =
                    product.SKU,

                barcode =
                    product.Barcode,

                qrValue =
                    product.QRValue,

                categoryId =
                    product.CategoryId,

                unitId =
                    product.UnitId,

                unitPrice =
                    product.UnitPrice,

                minimumStock =
                    product.MinimumStock,

                status =
                    product.Status
            });
        }


        // =====================================================
        // 7. SEARCH
        // GET: api/Products/Search?search=laptop
        // =====================================================

        [HttpGet("Search")]
        public async Task<IActionResult> SearchProducts(
            string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest(
                    "Search value is required.");
            }

            var products =
                await unitOfWork.Products
                    .SearchAsync(search);

            return Ok(products);
        }


        // =====================================================
        // 8. UPDATE PRODUCT
        // PUT: api/Products/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            UpdateProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }


            // =================================================
            // CHECK CATEGORY
            // =================================================

            var categoryExists =
                await unitOfWork.Categories
                    .GetByIdAsync(dto.CategoryId);

            if (categoryExists == null)
            {
                return BadRequest(
                    "Category not found.");
            }


            // =================================================
            // CHECK UNIT
            // =================================================

            var unitExists =
                await unitOfWork.Units
                    .GetByIdAsync(dto.UnitId);

            if (unitExists == null)
            {
                return BadRequest(
                    "Unit not found.");
            }


            // =================================================
            // CHECK SKU
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.SKU))
            {
                var sku =
                    dto.SKU.Trim();

                if (await unitOfWork.Products
                    .SKUExistsAsync(
                        sku,
                        id))
                {
                    return Conflict(
                        "SKU already exists.");
                }

                product.SKU = sku;
            }


            // =================================================
            // CHECK BARCODE
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var barcode =
                    dto.Barcode.Trim();

                if (await unitOfWork.Products
                    .BarcodeExistsAsync(
                        barcode,
                        id))
                {
                    return Conflict(
                        "Barcode already exists.");
                }

                product.Barcode = barcode;

                // QR must always equal Barcode
                product.QRValue = barcode;
            }


            // =================================================
            // UPDATE PRODUCT DATA
            // =================================================

            product.ProductName =
                dto.ProductName.Trim();

            product.CategoryId =
                dto.CategoryId;

            product.UnitId =
                dto.UnitId;

            product.UnitPrice =
                dto.UnitPrice;

            product.MinimumStock =
                dto.MinimumStock;

            product.UpdatedAt =
                DateTimeOffset.UtcNow;


            unitOfWork.Products
                .Update(product);

            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Product updated successfully.",

                productId =
                    product.ProductId,

                productName =
                    product.ProductName,

                sku =
                    product.SKU,

                barcode =
                    product.Barcode,

                qrValue =
                    product.QRValue,

                categoryId =
                    product.CategoryId,

                unitId =
                    product.UnitId,

                unitPrice =
                    product.UnitPrice,

                minimumStock =
                    product.MinimumStock,

                status =
                    product.Status,

                updatedAt =
                    product.UpdatedAt
            });
        }


        // =====================================================
        // 9. DELETE PRODUCT
        // DELETE: api/Products/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            unitOfWork.Products
                .Delete(product);

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest(
                    "Product cannot be deleted because it is used in other records.");
            }

            return Ok(new
            {
                message =
                    "Product deleted successfully.",

                productId =
                    id
            });
        }


        // =====================================================
        // 10. GET QR CODE IMAGE
        // GET: api/Products/{id}/GetQRCode
        // =====================================================

        [HttpGet("{id:int}/GetQRCode")]
        public async Task<IActionResult> GetQRCode(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            if (string.IsNullOrWhiteSpace(product.QRValue))
            {
                return BadRequest(
                    "This product does not have a QR value.");
            }

            var qrImage =
                qrCodeService.GenerateQRCode(
                    product.QRValue);

            return File(
                qrImage,
                "image/png",
                $"Product-{product.ProductId}-QR.png");
        }


        // =====================================================
        // 11. GET BARCODE IMAGE
        // GET: api/Products/{id}/barcode
        // =====================================================

        [HttpGet("{id:int}/barcode")]
        public async Task<IActionResult> GetProductBarcode(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return BadRequest(
                    "This product does not have a barcode.");
            }

            var image =
                barcodeService.GenerateBarcode(
                    product.Barcode);

            return File(
                image,
                "image/png",
                $"Product-{product.ProductId}-Barcode.png");
        }
    }
}