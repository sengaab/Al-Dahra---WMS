
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using whm.DTOs.Product;
//using whm.Models;
//using whm.UnitOfWork;

//namespace whm.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class ProductsController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public ProductsController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }


//        // =====================================================
//        // GET ALL PRODUCTS
//        // GET /api/products
//        // =====================================================

//        [HttpGet]
//        public async Task<IActionResult> GetProducts()
//        {
//            var products =
//                await _unitOfWork.Products.GetAllAsync();

//            return Ok(products);
//        }


//        // =====================================================
//        // GET PRODUCT BY ID
//        // GET /api/products/{id}
//        // =====================================================

//        [HttpGet("{id:int}")]
//        public async Task<IActionResult> GetProduct(int id)
//        {
//            var product =
//                await _unitOfWork.Products.GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            return Ok(product);
//        }


//        // =====================================================
//        // SEARCH PRODUCTS
//        // GET /api/products/search?q=
//        // =====================================================

//        [HttpGet("search")]
//        public async Task<IActionResult> Search(
//            [FromQuery] string q)
//        {
//            if (string.IsNullOrWhiteSpace(q))
//            {
//                return BadRequest(new
//                {
//                    message = "Search query is required."
//                });
//            }

//            var products =
//                await _unitOfWork.Products.SearchAsync(q);

//            return Ok(products);
//        }


//        // =====================================================
//        // GET PRODUCT BY BARCODE
//        // GET /api/products/barcode/{barcode}
//        // =====================================================

//        [HttpGet("barcode/{barcode}")]
//        public async Task<IActionResult> GetByBarcode(
//            string barcode)
//        {
//            if (string.IsNullOrWhiteSpace(barcode))
//            {
//                return BadRequest(new
//                {
//                    message = "Barcode is required."
//                });
//            }

//            var product =
//                await _unitOfWork.Products
//                    .GetByBarcodeAsync(barcode);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message =
//                        "Product with this barcode was not found."
//                });
//            }

//            return Ok(product);
//        }


//        // =====================================================
//        // GET PRODUCT BY SKU
//        // GET /api/products/sku/{sku}
//        // =====================================================

//        [HttpGet("sku/{sku}")]
//        public async Task<IActionResult> GetBySku(
//            string sku)
//        {
//            if (string.IsNullOrWhiteSpace(sku))
//            {
//                return BadRequest(new
//                {
//                    message = "SKU is required."
//                });
//            }

//            var product =
//                await _unitOfWork.Products
//                    .GetBySkuAsync(sku);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message =
//                        "Product with this SKU was not found."
//                });
//            }

//            return Ok(product);
//        }


//        // =====================================================
//        // CREATE PRODUCT
//        // POST /api/products
//        // =====================================================

//        [HttpPost]
//        public async Task<IActionResult> CreateProduct(
//            [FromBody] CreateProductDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ValidationProblem(ModelState);
//            }


//            // =================================================
//            // 1. SKU
//            // =================================================

//            string sku;

//            if (!string.IsNullOrWhiteSpace(dto.SKU) &&
//                !IsSwaggerPlaceholder(dto.SKU))
//            {
//                sku = dto.SKU.Trim();

//                var existingSku =
//                    await _unitOfWork.Products
//                        .GetBySkuAsync(sku);

//                if (existingSku != null)
//                {
//                    return Conflict(new
//                    {
//                        message =
//                            "A product with this SKU already exists."
//                    });
//                }
//            }
//            else
//            {
//                // Generate SKU automatically
//                sku = await GenerateUniqueSkuAsync();
//            }


//            // =================================================
//            // 2. BARCODE
//            // =================================================

//            string barcode;

//            // If user entered Barcode
//            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
//                !IsSwaggerPlaceholder(dto.Barcode))
//            {
//                barcode = dto.Barcode.Trim();

//                var existingBarcode =
//                    await _unitOfWork.Products
//                        .GetByBarcodeAsync(barcode);

//                if (existingBarcode != null)
//                {
//                    return Conflict(new
//                    {
//                        message =
//                            "A product with this barcode already exists."
//                    });
//                }
//            }
//            else
//            {
//                // Generate Barcode automatically
//                barcode =
//                    await GenerateUniqueBarcodeAsync();
//            }


//            // =================================================
//            // 3. QR VALUE
//            // =================================================

//            string qrValue;

//            // If user entered QRValue
//            if (!string.IsNullOrWhiteSpace(dto.QRValue) &&
//                !IsSwaggerPlaceholder(dto.QRValue))
//            {
//                qrValue = dto.QRValue.Trim();

//                var existingQr =
//                    await _unitOfWork.Products
//                        .GetByQrValueAsync(qrValue);

//                if (existingQr != null)
//                {
//                    return Conflict(new
//                    {
//                        message =
//                            "A product with this QR value already exists."
//                    });
//                }
//            }
//            else
//            {
//                // Generate QR automatically
//                qrValue =
//                    await GenerateUniqueQrValueAsync();
//            }


//            // =================================================
//            // 4. CREATE PRODUCT
//            // =================================================

//            var product = new Product
//            {
//                SKU = sku,

//                Barcode = barcode,

//                QRValue = qrValue,

//                Name = dto.Name,

//                CategoryId = dto.CategoryId,

//                UnitId = dto.UnitId,

//                UnitPrice = dto.UnitPrice,

//                MinimumStock = dto.MinimumStock,

//                Description = dto.Description,

//                ProductStatus = ProductStatus.Active,

//                IsActive = true,

//                CreatedAt = DateTimeOffset.UtcNow,

//                UpdatedAt = DateTimeOffset.UtcNow
//            };


//            // =================================================
//            // 5. SAVE
//            // =================================================

//            await _unitOfWork.Products
//                .AddAsync(product);

//            await _unitOfWork.SaveAsync();


//            // =================================================
//            // 6. GET CREATED PRODUCT
//            // =================================================

//            var result =
//                await _unitOfWork.Products
//                    .GetByIdAsync(product.ProductId);


//            return CreatedAtAction(
//                nameof(GetProduct),
//                new
//                {
//                    id = product.ProductId
//                },
//                result);
//        }


//        // =====================================================
//        // UPDATE PRODUCT
//        // PUT /api/products/{id}
//        // =====================================================

//        [HttpPut("{id:int}")]
//        public async Task<IActionResult> UpdateProduct(
//            int id,
//            [FromBody] UpdateProductDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ValidationProblem(ModelState);
//            }


//            // =================================================
//            // FIND PRODUCT
//            // =================================================

//            var product =
//                await _unitOfWork.Products
//                    .GetEntityByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }


//            // =================================================
//            // UPDATE BARCODE
//            // =================================================

//            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
//                !IsSwaggerPlaceholder(dto.Barcode))
//            {
//                var barcode = dto.Barcode.Trim();

//                var existingBarcode =
//                    await _unitOfWork.Products
//                        .GetByBarcodeAsync(barcode);

//                if (existingBarcode != null &&
//                    existingBarcode.ProductId != id)
//                {
//                    return Conflict(new
//                    {
//                        message =
//                            "A product with this barcode already exists."
//                    });
//                }

//                product.Barcode = barcode;
//            }


//            // =================================================
//            // UPDATE QR VALUE
//            // =================================================

//            if (!string.IsNullOrWhiteSpace(dto.QRValue) &&
//                !IsSwaggerPlaceholder(dto.QRValue))
//            {
//                var qrValue = dto.QRValue.Trim();

//                var existingQr =
//                    await _unitOfWork.Products
//                        .GetByQrValueAsync(qrValue);

//                if (existingQr != null &&
//                    existingQr.ProductId != id)
//                {
//                    return Conflict(new
//                    {
//                        message =
//                            "A product with this QR value already exists."
//                    });
//                }

//                product.QRValue = qrValue;
//            }


//            // =================================================
//            // UPDATE NAME
//            // =================================================

//            if (!string.IsNullOrWhiteSpace(dto.Name) &&
//                !IsSwaggerPlaceholder(dto.Name))
//            {
//                product.Name =
//                    dto.Name.Trim();
//            }


//            // =================================================
//            // UPDATE CATEGORY
//            // =================================================

//            if (dto.CategoryId.HasValue)
//            {
//                product.CategoryId =
//                    dto.CategoryId.Value;
//            }


//            // =================================================
//            // UPDATE UNIT
//            // =================================================

//            if (dto.UnitId.HasValue)
//            {
//                product.UnitId =
//                    dto.UnitId.Value;
//            }


//            // =================================================
//            // UPDATE UNIT PRICE
//            // =================================================

//            if (dto.UnitPrice.HasValue)
//            {
//                product.UnitPrice =
//                    dto.UnitPrice.Value;
//            }


//            // =================================================
//            // UPDATE MINIMUM STOCK
//            // =================================================

//            if (dto.MinimumStock.HasValue)
//            {
//                product.MinimumStock =
//                    dto.MinimumStock.Value;
//            }


//            // =================================================
//            // UPDATE DESCRIPTION
//            // =================================================

//            if (dto.Description != null &&
//                !IsSwaggerPlaceholder(dto.Description))
//            {
//                product.Description =
//                    dto.Description.Trim();
//            }


//            // =================================================
//            // UPDATE PRODUCT STATUS
//            // =================================================

//            if (!string.IsNullOrWhiteSpace(dto.ProductStatus) &&
//                !IsSwaggerPlaceholder(dto.ProductStatus) &&
//                Enum.TryParse<ProductStatus>(
//                    dto.ProductStatus,
//                    true,
//                    out var status))
//            {
//                product.ProductStatus = status;

//                product.IsActive =
//                    status == ProductStatus.Active;
//            }


//            // =================================================
//            // UPDATE IS ACTIVE
//            // =================================================

//            if (dto.IsActive.HasValue)
//            {
//                product.IsActive =
//                    dto.IsActive.Value;
//            }


//            // =================================================
//            // UPDATED AT
//            // =================================================

//            product.UpdatedAt =
//                DateTimeOffset.UtcNow;


//            // =================================================
//            // SAVE
//            // =================================================

//            _unitOfWork.Products
//                .Update(product);

//            await _unitOfWork.SaveAsync();


//            // =================================================
//            // RETURN UPDATED PRODUCT
//            // =================================================

//            var result =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            return Ok(result);
//        }


//        // =====================================================
//        // DELETE PRODUCT
//        // DELETE /api/products/{id}
//        // =====================================================

//        [HttpDelete("{id:int}")]
//        public async Task<IActionResult> DeleteProduct(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetEntityByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            _unitOfWork.Products
//                .Delete(product);

//            await _unitOfWork.SaveAsync();

//            return Ok(new
//            {
//                message =
//                    "Product deleted successfully."
//            });
//        }


//        // =====================================================
//        // PRODUCT INVENTORY
//        // GET /api/products/{id}/inventory
//        // =====================================================

//        [HttpGet("{id:int}/inventory")]
//        public async Task<IActionResult> GetInventory(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var inventory =
//                await _unitOfWork.Products
//                    .GetInventoryAsync(id);

//            return Ok(inventory);
//        }


//        // =====================================================
//        // PRODUCT STOCK
//        // GET /api/products/{id}/stock
//        // =====================================================

//        [HttpGet("{id:int}/stock")]
//        public async Task<IActionResult> GetStock(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var stock =
//                await _unitOfWork.Products
//                    .GetStockAsync(id);

//            return Ok(stock);
//        }


//        // =====================================================
//        // PRODUCT LOCATIONS
//        // GET /api/products/{id}/locations
//        // =====================================================

//        [HttpGet("{id:int}/locations")]
//        public async Task<IActionResult> GetLocations(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var locations =
//                await _unitOfWork.Products
//                    .GetLocationsAsync(id);

//            return Ok(locations);
//        }


//        // =====================================================
//        // PRODUCT TRANSACTIONS
//        // GET /api/products/{id}/transactions
//        // =====================================================

//        [HttpGet("{id:int}/transactions")]
//        public async Task<IActionResult> GetTransactions(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var transactions =
//                await _unitOfWork.Products
//                    .GetTransactionsAsync(id);

//            return Ok(transactions);
//        }


//        // =====================================================
//        // PRODUCT SUPPLIERS
//        // GET /api/products/{id}/suppliers
//        // =====================================================

//        [HttpGet("{id:int}/suppliers")]
//        public async Task<IActionResult> GetSuppliers(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var suppliers =
//                await _unitOfWork.Products
//                    .GetSuppliersAsync(id);

//            return Ok(suppliers);
//        }


//        // =====================================================
//        // PURCHASE HISTORY
//        // GET /api/products/{id}/purchase-history
//        // =====================================================

//        [HttpGet("{id:int}/purchase-history")]
//        public async Task<IActionResult> GetPurchaseHistory(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var history =
//                await _unitOfWork.Products
//                    .GetPurchaseHistoryAsync(id);

//            return Ok(history);
//        }


//        // =====================================================
//        // STOCK SUMMARY
//        // GET /api/products/{id}/stock-summary
//        // =====================================================

//        [HttpGet("{id:int}/stock-summary")]
//        public async Task<IActionResult> GetStockSummary(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var summary =
//                await _unitOfWork.Products
//                    .GetStockSummaryAsync(id);

//            return Ok(summary);
//        }


//        // =====================================================
//        // STOCK BY WAREHOUSE
//        // GET /api/products/{id}/stock-by-warehouse
//        // =====================================================

//        [HttpGet("{id:int}/stock-by-warehouse")]
//        public async Task<IActionResult> GetStockByWarehouse(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var result =
//                await _unitOfWork.Products
//                    .GetStockByWarehouseAsync(id);

//            return Ok(result);
//        }


//        // =====================================================
//        // STOCK BY LOCATION
//        // GET /api/products/{id}/stock-by-location
//        // =====================================================

//        [HttpGet("{id:int}/stock-by-location")]
//        public async Task<IActionResult> GetStockByLocation(int id)
//        {
//            var product =
//                await _unitOfWork.Products
//                    .GetByIdAsync(id);

//            if (product == null)
//            {
//                return NotFound(new
//                {
//                    message = "Product not found."
//                });
//            }

//            var result =
//                await _unitOfWork.Products
//                    .GetStockByLocationAsync(id);

//            return Ok(result);
//        }


//        // =====================================================
//        // GENERATE UNIQUE SKU
//        // =====================================================

//        private async Task<string> GenerateUniqueSkuAsync()
//        {
//            string sku;

//            do
//            {
//                sku =
//                    $"SKU-{Random.Shared.Next(10000000, 99999999)}";

//            } while (
//                await _unitOfWork.Products
//                    .GetBySkuAsync(sku) != null
//            );

//            return sku;
//        }


//        // =====================================================
//        // GENERATE UNIQUE BARCODE
//        // =====================================================

//        private async Task<string> GenerateUniqueBarcodeAsync()
//        {
//            string barcode;

//            do
//            {
//                // 12 digit numeric barcode

//                barcode =
//                    Random.Shared
//                        .NextInt64(
//                            100000000000,
//                            999999999999)
//                        .ToString();

//            } while (
//                await _unitOfWork.Products
//                    .GetByBarcodeAsync(barcode) != null
//            );

//            return barcode;
//        }


//        // =====================================================
//        // GENERATE UNIQUE QR VALUE
//        // =====================================================

//        private async Task<string> GenerateUniqueQrValueAsync()
//        {
//            string qrValue;

//            do
//            {
//                // Example:
//                // QR-8F72A91C45D1

//                qrValue =
//                    $"QR-{Guid.NewGuid():N}"
//                        .Substring(0, 15)
//                        .ToUpper();

//            } while (
//                await _unitOfWork.Products
//                    .GetByQrValueAsync(qrValue) != null
//            );

//            return qrValue;
//        }


//        // =====================================================
//        // CHECK SWAGGER PLACEHOLDER
//        // =====================================================

//        private static bool IsSwaggerPlaceholder(string? value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return true;

//            return value.Trim()
//                .Equals(
//                    "string",
//                    StringComparison.OrdinalIgnoreCase);
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Product;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL PRODUCTS
        // GET /api/products
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =
                await _unitOfWork.Products.GetAllAsync();

            return Ok(products);
        }


        // =====================================================
        // GET PRODUCT BY ID
        // GET /api/products/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =
                await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(product);
        }


        // =====================================================
        // SEARCH PRODUCTS
        // GET /api/products/search?q=
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(new
                {
                    message = "Search query is required."
                });
            }

            var products =
                await _unitOfWork.Products.SearchAsync(q);

            return Ok(products);
        }


        // =====================================================
        // GET PRODUCT BY BARCODE
        // GET /api/products/barcode/{barcode}
        // =====================================================

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(
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
                await _unitOfWork.Products
                    .GetByBarcodeAsync(barcode);

            if (product == null)
            {
                return NotFound(new
                {
                    message =
                        "Product with this barcode was not found."
                });
            }

            return Ok(product);
        }


        // =====================================================
        // GET PRODUCT BY SKU
        // GET /api/products/sku/{sku}
        // =====================================================

        [HttpGet("sku/{sku}")]
        public async Task<IActionResult> GetBySku(
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
                await _unitOfWork.Products
                    .GetBySkuAsync(sku);

            if (product == null)
            {
                return NotFound(new
                {
                    message =
                        "Product with this SKU was not found."
                });
            }

            return Ok(product);
        }


        // =====================================================
        // CREATE PRODUCT
        // POST /api/products
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            // =================================================
            // 1. CATEGORY - OPTIONAL
            // =================================================

            if (dto.CategoryId.HasValue)
            {
                if (dto.CategoryId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid CategoryId."
                    });
                }

                var category =
                    await _unitOfWork.Categories
                        .GetEntityByIdAsync(dto.CategoryId.Value);

                if (category == null)
                {
                    return BadRequest(new
                    {
                        message = "Category not found."
                    });
                }
            }


            // =================================================
            // 2. UNIT - OPTIONAL
            // =================================================

            if (dto.UnitId.HasValue)
            {
                if (dto.UnitId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid UnitId."
                    });
                }

                var unit =
                    await _unitOfWork.Units
                        .GetEntityByIdAsync(dto.UnitId.Value);

                if (unit == null)
                {
                    return BadRequest(new
                    {
                        message = "Unit not found."
                    });
                }
            }


            // =================================================
            // 3. SKU
            // =================================================

            string sku;

            if (!string.IsNullOrWhiteSpace(dto.SKU) &&
                !IsSwaggerPlaceholder(dto.SKU))
            {
                sku = dto.SKU.Trim();

                var existingSku =
                    await _unitOfWork.Products
                        .GetBySkuAsync(sku);

                if (existingSku != null)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this SKU already exists."
                    });
                }
            }
            else
            {
                sku = await GenerateUniqueSkuAsync();
            }


            // =================================================
            // 4. BARCODE - OPTIONAL / MANUAL
            // =================================================

            string? barcode = null;

            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
                !IsSwaggerPlaceholder(dto.Barcode))
            {
                barcode = dto.Barcode.Trim();

                var existingBarcode =
                    await _unitOfWork.Products
                        .GetByBarcodeAsync(barcode);

                if (existingBarcode != null)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this barcode already exists."
                    });
                }
            }


            // =================================================
            // 5. QR VALUE - OPTIONAL / MANUAL
            // =================================================

            string? qrValue = null;

            if (!string.IsNullOrWhiteSpace(dto.QRValue) &&
                !IsSwaggerPlaceholder(dto.QRValue))
            {
                qrValue = dto.QRValue.Trim();

                var existingQr =
                    await _unitOfWork.Products
                        .GetByQrValueAsync(qrValue);

                if (existingQr != null)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this QR value already exists."
                    });
                }
            }


            // =================================================
            // 6. CREATE PRODUCT
            // =================================================

            var product = new Product
            {
                SKU = sku,

                Barcode = barcode,

                QRValue = qrValue,

                Name = dto.Name,

                CategoryId = dto.CategoryId,

                UnitId = dto.UnitId,

                UnitPrice = dto.UnitPrice,

                MinimumStock = dto.MinimumStock,

                Description = dto.Description,

                ProductStatus = ProductStatus.Active,

                IsActive = true,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow
            };


            // =================================================
            // 7. SAVE
            // =================================================

            await _unitOfWork.Products
                .AddAsync(product);

            await _unitOfWork.SaveAsync();


            // =================================================
            // 8. GET CREATED PRODUCT
            // =================================================

            var result =
                await _unitOfWork.Products
                    .GetByIdAsync(product.ProductId);


            return CreatedAtAction(
                nameof(GetProduct),
                new
                {
                    id = product.ProductId
                },
                result);
        }


        // =====================================================
        // UPDATE PRODUCT
        // PUT /api/products/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            // =================================================
            // FIND PRODUCT
            // =================================================

            var product =
                await _unitOfWork.Products
                    .GetEntityByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }


            // =================================================
            // UPDATE BARCODE
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
                !IsSwaggerPlaceholder(dto.Barcode))
            {
                var barcode = dto.Barcode.Trim();

                var existingBarcode =
                    await _unitOfWork.Products
                        .GetByBarcodeAsync(barcode);

                if (existingBarcode != null &&
                    existingBarcode.ProductId != id)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this barcode already exists."
                    });
                }

                product.Barcode = barcode;
            }


            // =================================================
            // UPDATE QR VALUE
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.QRValue) &&
                !IsSwaggerPlaceholder(dto.QRValue))
            {
                var qrValue = dto.QRValue.Trim();

                var existingQr =
                    await _unitOfWork.Products
                        .GetByQrValueAsync(qrValue);

                if (existingQr != null &&
                    existingQr.ProductId != id)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this QR value already exists."
                    });
                }

                product.QRValue = qrValue;
            }


            // =================================================
            // UPDATE NAME
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.Name) &&
                !IsSwaggerPlaceholder(dto.Name))
            {
                product.Name =
                    dto.Name.Trim();
            }


            // =================================================
            // UPDATE CATEGORY
            // =================================================

            if (dto.CategoryId.HasValue)
            {
                if (dto.CategoryId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid CategoryId."
                    });
                }

                var category =
                    await _unitOfWork.Categories
                        .GetEntityByIdAsync(dto.CategoryId.Value);

                if (category == null)
                {
                    return BadRequest(new
                    {
                        message = "Category not found."
                    });
                }

                product.CategoryId =
                    dto.CategoryId.Value;
            }


            // =================================================
            // UPDATE UNIT
            // =================================================

            if (dto.UnitId.HasValue)
            {
                if (dto.UnitId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid UnitId."
                    });
                }

                var unit =
                    await _unitOfWork.Units
                        .GetEntityByIdAsync(dto.UnitId.Value);

                if (unit == null)
                {
                    return BadRequest(new
                    {
                        message = "Unit not found."
                    });
                }

                product.UnitId =
                    dto.UnitId.Value;
            }


            // =================================================
            // UPDATE UNIT PRICE
            // =================================================

            if (dto.UnitPrice.HasValue)
            {
                product.UnitPrice =
                    dto.UnitPrice.Value;
            }


            // =================================================
            // UPDATE MINIMUM STOCK
            // =================================================

            if (dto.MinimumStock.HasValue)
            {
                product.MinimumStock =
                    dto.MinimumStock.Value;
            }


            // =================================================
            // UPDATE DESCRIPTION
            // =================================================

            if (dto.Description != null &&
                !IsSwaggerPlaceholder(dto.Description))
            {
                product.Description =
                    dto.Description.Trim();
            }


            // =================================================
            // UPDATE PRODUCT STATUS
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.ProductStatus) &&
                !IsSwaggerPlaceholder(dto.ProductStatus) &&
                Enum.TryParse<ProductStatus>(
                    dto.ProductStatus,
                    true,
                    out var status))
            {
                product.ProductStatus = status;

                product.IsActive =
                    status == ProductStatus.Active;
            }


            // =================================================
            // UPDATE IS ACTIVE
            // =================================================

            if (dto.IsActive.HasValue)
            {
                product.IsActive =
                    dto.IsActive.Value;
            }


            // =================================================
            // UPDATED AT
            // =================================================

            product.UpdatedAt =
                DateTimeOffset.UtcNow;


            // =================================================
            // SAVE
            // =================================================

            _unitOfWork.Products
                .Update(product);

            await _unitOfWork.SaveAsync();


            // =================================================
            // RETURN UPDATED PRODUCT
            // =================================================

            var result =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE PRODUCT
        // DELETE /api/products/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetEntityByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            _unitOfWork.Products
                .Delete(product);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Product deleted successfully."
            });
        }


        // =====================================================
        // PRODUCT INVENTORY
        // GET /api/products/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var inventory =
                await _unitOfWork.Products
                    .GetInventoryAsync(id);

            return Ok(inventory);
        }


        // =====================================================
        // PRODUCT STOCK
        // GET /api/products/{id}/stock
        // =====================================================

        [HttpGet("{id:int}/stock")]
        public async Task<IActionResult> GetStock(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var stock =
                await _unitOfWork.Products
                    .GetStockAsync(id);

            return Ok(stock);
        }


        // =====================================================
        // PRODUCT LOCATIONS
        // GET /api/products/{id}/locations
        // =====================================================

        [HttpGet("{id:int}/locations")]
        public async Task<IActionResult> GetLocations(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var locations =
                await _unitOfWork.Products
                    .GetLocationsAsync(id);

            return Ok(locations);
        }


        // =====================================================
        // PRODUCT TRANSACTIONS
        // GET /api/products/{id}/transactions
        // =====================================================

        [HttpGet("{id:int}/transactions")]
        public async Task<IActionResult> GetTransactions(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var transactions =
                await _unitOfWork.Products
                    .GetTransactionsAsync(id);

            return Ok(transactions);
        }


        // =====================================================
        // PRODUCT SUPPLIERS
        // GET /api/products/{id}/suppliers
        // =====================================================

        [HttpGet("{id:int}/suppliers")]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var suppliers =
                await _unitOfWork.Products
                    .GetSuppliersAsync(id);

            return Ok(suppliers);
        }


        // =====================================================
        // PURCHASE HISTORY
        // GET /api/products/{id}/purchase-history
        // =====================================================

        [HttpGet("{id:int}/purchase-history")]
        public async Task<IActionResult> GetPurchaseHistory(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var history =
                await _unitOfWork.Products
                    .GetPurchaseHistoryAsync(id);

            return Ok(history);
        }


        // =====================================================
        // STOCK SUMMARY
        // GET /api/products/{id}/stock-summary
        // =====================================================

        [HttpGet("{id:int}/stock-summary")]
        public async Task<IActionResult> GetStockSummary(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var summary =
                await _unitOfWork.Products
                    .GetStockSummaryAsync(id);

            return Ok(summary);
        }


        // =====================================================
        // STOCK BY WAREHOUSE
        // GET /api/products/{id}/stock-by-warehouse
        // =====================================================

        [HttpGet("{id:int}/stock-by-warehouse")]
        public async Task<IActionResult> GetStockByWarehouse(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var result =
                await _unitOfWork.Products
                    .GetStockByWarehouseAsync(id);

            return Ok(result);
        }


        // =====================================================
        // STOCK BY LOCATION
        // GET /api/products/{id}/stock-by-location
        // =====================================================

        [HttpGet("{id:int}/stock-by-location")]
        public async Task<IActionResult> GetStockByLocation(int id)
        {
            var product =
                await _unitOfWork.Products
                    .GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            var result =
                await _unitOfWork.Products
                    .GetStockByLocationAsync(id);

            return Ok(result);
        }


        // =====================================================
        // GENERATE UNIQUE SKU
        // =====================================================

        private async Task<string> GenerateUniqueSkuAsync()
        {
            string sku;

            do
            {
                sku =
                    $"SKU-{Random.Shared.Next(10000000, 99999999)}";

            } while (
                await _unitOfWork.Products
                    .GetBySkuAsync(sku) != null
            );

            return sku;
        }


        // =====================================================
        // CHECK SWAGGER PLACEHOLDER
        // =====================================================

        private static bool IsSwaggerPlaceholder(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            return value.Trim()
                .Equals(
                    "string",
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}