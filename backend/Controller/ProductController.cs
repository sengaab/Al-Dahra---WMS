using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Product;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                    message = "Product with this barcode was not found."
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
                    message = "Product with this SKU was not found."
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


            // -------------------------------------------------
            // Check SKU
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.SKU))
            {
                var existingSku =
                    await _unitOfWork.Products
                        .GetBySkuAsync(dto.SKU);

                if (existingSku != null)
                {
                    return Conflict(new
                    {
                        message = "A product with this SKU already exists."
                    });
                }
            }


            // -------------------------------------------------
            // Check Barcode
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var existingBarcode =
                    await _unitOfWork.Products
                        .GetByBarcodeAsync(dto.Barcode);

                if (existingBarcode != null)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this barcode already exists."
                    });
                }
            }


            // -------------------------------------------------
            // Create Product
            // -------------------------------------------------

            var product = new Product
            {
                SKU = dto.SKU,

                Barcode = dto.Barcode,

                QRValue = dto.QRValue,

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


            await _unitOfWork.Products
                .AddAsync(product);

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Get Created Product
            // -------------------------------------------------

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


            // -------------------------------------------------
            // Find Product
            // -------------------------------------------------

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


            // -------------------------------------------------
            // Check Barcode
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var existingBarcode =
                    await _unitOfWork.Products
                        .GetByBarcodeAsync(dto.Barcode);

                if (existingBarcode != null &&
                    existingBarcode.ProductId != id)
                {
                    return Conflict(new
                    {
                        message =
                            "A product with this barcode already exists."
                    });
                }
            }


            // -------------------------------------------------
            // Update Product
            // -------------------------------------------------

            if (dto.Barcode != null)
                product.Barcode = dto.Barcode;

            if (dto.QRValue != null)
                product.QRValue = dto.QRValue;

            if (dto.Name != null)
                product.Name = dto.Name;

            if (dto.CategoryId.HasValue)
                product.CategoryId = dto.CategoryId;

            if (dto.UnitId.HasValue)
                product.UnitId = dto.UnitId;

            if (dto.UnitPrice.HasValue)
                product.UnitPrice = dto.UnitPrice.Value;

            if (dto.MinimumStock.HasValue)
                product.MinimumStock = dto.MinimumStock.Value;

            if (dto.Description != null)
                product.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.ProductStatus) &&
                Enum.TryParse<ProductStatus>(
                    dto.ProductStatus,
                    true,
                    out var status))
            {
                product.ProductStatus = status;

                product.IsActive =
                    status == ProductStatus.Active;
            }

            if (dto.IsActive.HasValue)
                product.IsActive = dto.IsActive.Value;


            product.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Products
                .Update(product);

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Return Updated Product
            // -------------------------------------------------

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
                message = "Product deleted successfully."
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
    }
}