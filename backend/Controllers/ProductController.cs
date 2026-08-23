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
        // Helper
        // =====================================================

        private object ProductResponse(Product product)
        {
            return new
            {
                productId = product.ProductId,
                productName = product.ProductName,

                sku = product.SKU,
                barcode = product.Barcode,
                qrValue = product.QRValue,

                categoryId = product.CategoryId,
                categoryName = product.Category?.Category_Name,

                subCategoryId = product.SubCategoryId,
                subCategoryName =
                    product.SubCategory?.SubCategory_Name,

               /* status = product.Status*/

                createdAt = product.CreatedAt,
                updatedAt = product.UpdatedAt
            };
        }

        // =====================================================
        // 1. CREATE PRODUCT
        // POST: api/Products/create
        // =====================================================

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // =================================================
            // CHECK CATEGORY
            // =================================================

            var category =
                await unitOfWork.Categories
                    .GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                return BadRequest(new
                {
                    message = "Category not found."
                });
            }

            // =================================================
            // CHECK SUB CATEGORY
            // =================================================

            var subCategory =
                await unitOfWork.SubCategories
                    .GetByIdAsync(dto.subCategoryId);

            if (subCategory == null)
            {
                return BadRequest(new
                {
                    message = "SubCategory not found."
                });
            }

            // =================================================
            // CHECK SUB CATEGORY BELONGS TO CATEGORY
            // =================================================

            if (subCategory.CategoryId != dto.CategoryId)
            {
                return BadRequest(new
                {
                    message =
                        "SubCategory does not belong to the selected Category."
                });
            }

            // =================================================
            // GENERATE SKU
            // =================================================

            var categoryName =
                category.Category_Name
                    .Trim()
                    .ToUpper();

            var prefix =
                categoryName.Length >= 3
                    ? categoryName[..3]
                    : categoryName;

            var lastSKU =
                await unitOfWork.Products
                    .GetLastSKUByPrefixAsync(prefix);

            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastSKU))
            {
                var numberPart =
                    lastSKU[prefix.Length..];

                if (int.TryParse(
                    numberPart,
                    out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var productCode =
                $"{prefix}{nextNumber:D6}";

            // =================================================
            // MAKE SURE SKU IS UNIQUE
            // =================================================

            while (await unitOfWork.Products
                .SKUExistsAsync(productCode))
            {
                nextNumber++;

                productCode =
                    $"{prefix}{nextNumber:D6}";
            }

            // =================================================
            // MAKE PRODUCT
            // =================================================

            var product = new Product
            {
                ProductName =
                    dto.ProductName.Trim(),

                CategoryId =
                    dto.CategoryId,

                SubCategoryId =
                    dto.subCategoryId,

                SKU =
                    productCode,

                Barcode =
                    productCode,

                QRValue =
                    productCode,

                //Status =
                //    ProductStatus.Available,

                CreatedAt =
                    DateTimeOffset.UtcNow,

                UpdatedAt =
                    null
            };

            // =================================================
            // SAVE
            // =================================================

            await unitOfWork.Products
                .AddAsync(product);

            await unitOfWork.SaveAsync();

            // =================================================
            // GENERATE QR
            // =================================================

            var qrImage =
                qrCodeService.GenerateQRCode(
                    product.QRValue);

            // =================================================
            // GENERATE BARCODE
            // =================================================

            var barcodeImage =
                barcodeService.GenerateBarcode(
                    product.Barcode);

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

                qrValue =
                    product.QRValue,

                categoryId =
                    product.CategoryId,

                subCategoryId =
                    product.SubCategoryId,

                //status =
                //    product.Status,

                qrCode =
                    $"data:image/png;base64," +
                    Convert.ToBase64String(qrImage),

                barcodeImage =
                    $"data:image/png;base64," +
                    Convert.ToBase64String(barcodeImage),

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
            try
            {
                var products =
                    await unitOfWork.Products
                        .GetAllAsync();

                var result =
                    products.Select(ProductResponse);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException =
                        ex.InnerException?.Message
                });
            }
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
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(ProductResponse(product));
        }

        // =====================================================
        // 4. GET PRODUCT BY SKU
        // GET: api/Products/GetbySKU/{sku}
        // =====================================================

        [HttpGet("GetbySKU/{sku}")]
        public async Task<IActionResult> GetProductBySKU(
            string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return BadRequest(new
                {
                    message = "SKU is required."
                });
            }

            var product =
                await unitOfWork.Products
                    .GetBySKUAsync(sku.Trim());

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(ProductResponse(product));
        }

        // =====================================================
        // 5. GET PRODUCT BY BARCODE
        // GET: api/Products/GetbyBarcode/{barcode}
        // =====================================================

        [HttpGet("GetbyBarcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(
            string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new
                {
                    message = "Barcode is required."
                });
            }

            var product =
                await unitOfWork.Products
                    .GetByBarcodeAsync(
                        barcode.Trim());

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(ProductResponse(product));
        }

        // =====================================================
        // 6. GET PRODUCT BY QR
        // GET: api/Products/GetbyQR/{qrValue}
        // =====================================================

        [HttpGet("GetbyQR/{qrValue}")]
        public async Task<IActionResult> GetProductByQR(
            string qrValue)
        {
            if (string.IsNullOrWhiteSpace(qrValue))
            {
                return BadRequest(new
                {
                    message = "QR value is required."
                });
            }

            var product =
                await unitOfWork.Products
                    .GetByQRValueAsync(
                        qrValue.Trim());

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(ProductResponse(product));
        }

        // =====================================================
        // 7. SEARCH PRODUCTS
        // GET: api/Products/Searchproducts?search=laptop
        // =====================================================

        [HttpGet("Searchproducts")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest(new
                {
                    message =
                        "Search value is required."
                });
            }

            var products =
                await unitOfWork.Products
                    .SearchAsync(search.Trim());

            return Ok(
                products.Select(ProductResponse));
        }

        // =====================================================
        // 8. UPDATE PRODUCT
        // PUT: api/Products/Update/1
        // =====================================================

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] UpdateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            // =================================================
            // CHECK CATEGORY
            // =================================================

            var category =
                await unitOfWork.Categories
                    .GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                return BadRequest(new
                {
                    message = "Category not found."
                });
            }

            // =================================================
            // CHECK SUB CATEGORY
            // =================================================

            var subCategory =
                await unitOfWork.SubCategories
                    .GetByIdAsync(dto.subCategoryId);

            if (subCategory == null)
            {
                return BadRequest(new
                {
                    message = "SubCategory not found."
                });
            }

            // =================================================
            // CHECK SUB CATEGORY RELATION
            // =================================================

            if (subCategory.CategoryId != dto.CategoryId)
            {
                return BadRequest(new
                {
                    message =
                        "SubCategory does not belong to the selected Category."
                });
            }

            // =================================================
            // CHECK SKU
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.SKU))
            {
                var sku = dto.SKU.Trim();

                if (await unitOfWork.Products
                    .SKUExistsAsync(
                        sku,
                        id))
                {
                    return Conflict(new
                    {
                        message =
                            "SKU already exists."
                    });
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
                    return Conflict(new
                    {
                        message =
                            "Barcode already exists."
                    });
                }

                product.Barcode =
                    barcode;

                product.QRValue =
                    barcode;
            }

            // =================================================
            // UPDATE PRODUCT
            // =================================================

            product.ProductName =
                dto.ProductName.Trim();

            product.CategoryId =
                dto.CategoryId;

            product.SubCategoryId =
                dto.subCategoryId;

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

                subCategoryId =
                    product.SubCategoryId,

                //status =
                //    product.Status,

                updatedAt =
                    product.UpdatedAt
            });
        }

        // =====================================================
        // 9. DELETE PRODUCT
        // DELETE: api/Products/1
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
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            unitOfWork.Products
                .Delete(product);

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest(new
                {
                    message =
                        "Product cannot be deleted because it is used in other records."
                });
            }

            return Ok(new
            {
                message =
                    "Product deleted successfully.",

                productId = id
            });
        }

        // =====================================================
        // 10. GET QR CODE IMAGE
        // GET: api/Products/1/GetQRCodeImage
        // =====================================================

        [HttpGet("{id:int}/GetQRCodeImage")]
        public async Task<IActionResult> GetQRCode(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            if (string.IsNullOrWhiteSpace(
                product.QRValue))
            {
                return BadRequest(new
                {
                    message =
                        "This product does not have a QR value."
                });
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
        // GET: api/Products/1/barcodeImage
        // =====================================================

        [HttpGet("{id:int}/barcodeImage")]
        public async Task<IActionResult> GetProductBarcode(
            int id)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            if (string.IsNullOrWhiteSpace(
                product.Barcode))
            {
                return BadRequest(new
                {
                    message =
                        "This product does not have a barcode."
                });
            }

            var image =
                barcodeService.GenerateBarcode(
                    product.Barcode);

            return File(
                image,
                "image/png",
                $"Product-{product.ProductId}-Barcode.png");
        }

        // =====================================================
        // 12. UPDATE PRODUCT STATUS
        // PUT: api/Products/UpdateStatus/1
        // =====================================================

        [HttpPut("UpdateStatus/{id:int}")]
        public async Task<IActionResult> UpdateProductStatus(
            int id,
            [FromBody] UpdateProductStatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product =
                await unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            //product.Status =
            //    dto.Status;

            product.UpdatedAt =
                DateTimeOffset.UtcNow;

            unitOfWork.Products
                .Update(product);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Product status updated successfully.",

                productId =
                    product.ProductId,

                //status =
                //    product.Status,

                updatedAt =
                    product.UpdatedAt
            });
        }

        // =====================================================
        // 13. SEARCH BY SITE / DEPARTMENT
        // =====================================================

        [HttpGet("SearchBySiteAndDepartment")]
        public async Task<IActionResult>
            SearchBySiteAndDepartment(
                [FromQuery] int? siteId,
                [FromQuery] int? departmentId)
        {
            if (!siteId.HasValue &&
                !departmentId.HasValue)
            {
                return BadRequest(new
                {
                    message =
                        "SiteId or DepartmentId is required."
                });
            }

            // =================================================
            // CHECK SITE
            // =================================================

            if (siteId.HasValue)
            {
                var site =
                    await unitOfWork.Sites
                        .GetByIdAsync(
                            siteId.Value);

                if (site == null)
                {
                    return NotFound(new
                    {
                        message = "Site not found."
                    });
                }
            }

            // =================================================
            // CHECK DEPARTMENT
            // =================================================

            if (departmentId.HasValue)
            {
                var department =
                    await unitOfWork.Departments
                        .GetByIdAsync(
                            departmentId.Value);

                if (department == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Department not found."
                    });
                }
            }

            // =================================================
            // SEARCH
            // =================================================

            var products =
                await unitOfWork.Products
                    .SearchBySiteAndDepartmentAsync(
                        siteId,
                        departmentId);

            if (products == null ||
                !products.Any())
            {
                return NotFound(new
                {
                    message =
                        "No products found."
                });
            }

            var result =
                products.Select(ProductResponse);

            return Ok(new
            {
                count = products.Count,

                filters = new
                {
                    siteId,
                    departmentId
                },

                products = result
            });
        }
    }
}