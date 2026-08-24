using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Stock;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public StockController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // 1. GET ALL STOCK
        // GET: api/Stock
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await unitOfWork.Stocks.GetAllAsync();

            var result = stocks.Select(s => new
            {
                stockId = s.Stock_Id,

                stockCode = s.StockCode,

                quantity = s.Quantity,

                reservedQuantity = s.ReservedQuantity,

                availableQuantity =
                    s.Quantity - s.ReservedQuantity,

                unitPrice = s.UnitPrice,

                minimumStock = s.MinimumStock,

                unitId = s.UnitId,

                productId = s.ProductId,

                productName = s.Product?.ProductName,

                sku = s.Product?.SKU,

                binId = s.Bin_Id,

                binName = s.Bin?.Bin_Name,

                expiryDate = s.ExpiryDate,

                stockStatus = s.StockStatus,

                deliveryStatus = s.DeliveryStatus,

                isActive = s.IsActive,

                createAt = s.CreateAt,

                lastUpdatedAt = s.LastUpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 2. GET STOCK BY ID
        // GET: api/Stock/1
        // =====================================================

        [HttpGet("Getbyid/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock =
                await unitOfWork.Stocks.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound("Stock not found.");
            }

            return Ok(new
            {
                stockId = stock.Stock_Id,

                stockCode = stock.StockCode,

                quantity = stock.Quantity,

                reservedQuantity = stock.ReservedQuantity,

                availableQuantity =
                    stock.Quantity -
                    stock.ReservedQuantity,

                unitPrice = stock.UnitPrice,

                minimumStock = stock.MinimumStock,

                unitId = stock.UnitId,

                productId = stock.ProductId,

                productName = stock.Product?.ProductName,

                sku = stock.Product?.SKU,

                binId = stock.Bin_Id,

                binName = stock.Bin?.Bin_Name,

                expiryDate = stock.ExpiryDate,

                stockStatus = stock.StockStatus,

                deliveryStatus = stock.DeliveryStatus,

                isActive = stock.IsActive,

                createAt = stock.CreateAt,

                lastUpdatedAt = stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 3. GET STOCK BY PRODUCT
        // GET: api/Stock/product/1
        // =====================================================

        [HttpGet("GETSTOCKBYPRODUCT/{productId:int}")]
        public async Task<IActionResult> GetByProductId(
            int productId)
        {
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(productId);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var stocks =
                await unitOfWork.Stocks
                    .GetByProductIdAsync(productId);

            if (stocks == null || !stocks.Any())
            {
                return NotFound(
                    "No stock found for this product.");
            }

            var result = stocks.Select(s => new
            {
                stockId = s.Stock_Id,

                stockCode = s.StockCode,

                quantity = s.Quantity,

                reservedQuantity =
                    s.ReservedQuantity,

                availableQuantity =
                    s.Quantity -
                    s.ReservedQuantity,

                unitPrice = s.UnitPrice,

                minimumStock = s.MinimumStock,

                unitId = s.UnitId,

                productId = s.ProductId,

                productName =
                    s.Product?.ProductName,

                sku =
                    s.Product?.SKU,

                binId = s.Bin_Id,

                binName =
                    s.Bin?.Bin_Name,

                expiryDate =
                    s.ExpiryDate,

                stockStatus =
                    s.StockStatus,

                deliveryStatus =
                    s.DeliveryStatus,

                isActive =
                    s.IsActive,

                createAt =
                    s.CreateAt,

                lastUpdatedAt =
                    s.LastUpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 4. GET STOCK BY BIN
        // GET: api/Stock/by-bin/1
        // =====================================================

        [HttpGet("by-bin/{binId:int}")]
        public async Task<IActionResult> GetByBinId(
            int binId)
        {
            var bin =
                await unitOfWork.Bins
                    .GetByIdAsync(binId);

            if (bin == null)
            {
                return NotFound("Bin not found.");
            }

            var stocks =
                await unitOfWork.Stocks
                    .GetByBinIdAsync(binId);

            if (stocks == null || !stocks.Any())
            {
                return NotFound(
                    "No stock found for this bin.");
            }

            var result = stocks.Select(s => new
            {
                stockId = s.Stock_Id,

                stockCode = s.StockCode,

                quantity = s.Quantity,

                reservedQuantity =
                    s.ReservedQuantity,

                availableQuantity =
                    s.Quantity -
                    s.ReservedQuantity,

                unitPrice = s.UnitPrice,

                minimumStock = s.MinimumStock,

                unitId = s.UnitId,

                productId = s.ProductId,

                productName =
                    s.Product?.ProductName,

                sku =
                    s.Product?.SKU,

                binId = s.Bin_Id,

                binName =
                    s.Bin?.Bin_Name,

                expiryDate =
                    s.ExpiryDate,

                stockStatus =
                    s.StockStatus,

                deliveryStatus =
                    s.DeliveryStatus,

                isActive =
                    s.IsActive,

                createAt =
                    s.CreateAt,

                lastUpdatedAt =
                    s.LastUpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 5. INVENTORY STOCK
        // GET: api/Stock/inventory
        // =====================================================

        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryStock(
    int page = 1,
    int pageSize = 10)
        {
            // =================================================
            // VALIDATE PAGINATION
            // =================================================

            if (page < 1)
            {
                return BadRequest("Page must be greater than 0.");
            }

            if (pageSize < 1)
            {
                return BadRequest("PageSize must be greater than 0.");
            }

            // حد أقصى لحماية الـAPI
            if (pageSize > 100)
            {
                return BadRequest(
                    "PageSize cannot be greater than 100.");
            }


            // =================================================
            // GET STOCK
            // =================================================

            var stocks =
                await unitOfWork.Stocks
                    .GetAllAsync();

            if (stocks == null || !stocks.Any())
            {
                return Ok(new
                {
                    page,
                    pageSize,
                    totalItems = 0,
                    totalPages = 0,
                    hasPreviousPage = false,
                    hasNextPage = false,
                    stocks = new List<InventoryStockResponseDto>()
                });
            }


            // =================================================
            // TOTAL ITEMS
            // =================================================

            var totalItems =
                stocks.Count();


            var totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize);


            // =================================================
            // PAGINATION
            // =================================================

            var pagedStocks =
                stocks
                    .OrderByDescending(s => s.Stock_Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);


            // =================================================
            // RESPONSE
            // =================================================

            var result =
                pagedStocks.Select(stock =>
                    new InventoryStockResponseDto
                    {
                        Stock_Id =
                            stock.Stock_Id,

                        SKU =
                            stock.Product?.SKU
                            ?? string.Empty,

                        Product =
                            stock.Product?.ProductName
                            ?? string.Empty,

                        // =================================================
                        // CATEGORY
                        // =================================================

                        Category =
                            stock.Product?.Category?.Category_Name,

                        // =================================================
                        // EXPIRY DATE
                        // =================================================

                        ExpiryDate =
                            stock.ExpiryDate,

                        // =================================================
                        // ALIASES
                        // =================================================

                        Aliases =
                            new List<string>(),

                        // =================================================
                        // LOCATION
                        // =================================================

                        Location =
                            stock.Bin?.Bin_Name,

                        // =================================================
                        // WAREHOUSE
                        // =================================================

                        Warehouse =
                            stock.Bin?
                                .Shelf?
                                .Row?
                                .Room?
                                .Warehouse?
                                .Warehouse_Name,

                        // =================================================
                        // LOT / BATCH
                        // =================================================

                        LotBatch =
                            stock.StockCode,

                        // =================================================
                        // QUANTITY
                        // =================================================

                        Quantity =
                            stock.Quantity,

                        // =================================================
                        // UOM
                        // =================================================

                        UOM =
                            stock.Units?.Unit_Name,

                        // =================================================
                        // UNIT PRICE
                        // =================================================

                        UnitPrice =
                            stock.UnitPrice,

                        // =================================================
                        // AVAILABLE
                        // =================================================

                        Available =
                            stock.Quantity -
                            stock.ReservedQuantity,

                        // =================================================
                        // RESERVED
                        // =================================================

                        Reserved =
                            stock.ReservedQuantity,

                        // =================================================
                        // STATUS
                        // =================================================

                        Status =
                            stock.StockStatus,

                        // =================================================
                        // LAST UPDATED
                        // =================================================

                        LastUpdated =
                            stock.LastUpdatedAt
                    })
                .ToList();


            // =================================================
            // FINAL RESPONSE
            // =================================================

            return Ok(new
            {
                page,

                pageSize,

                totalItems,

                totalPages,

                hasPreviousPage =
                    page > 1,

                hasNextPage =
                    page < totalPages,

                stocks = result
            });
        }
        // =====================================================
        // 6. UPDATE INVENTORY STOCK
        // PUT: api/Stock/inventory/{id}
        // =====================================================

        [HttpPut("inventory/{id:int}")]
        public async Task<IActionResult> UpdateInventoryStock(
            int id,
            UpdateInventoryStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // =================================================
            // GET STOCK
            // =================================================

            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound("Stock not found.");
            }

            // =================================================
            // VALIDATION
            // =================================================

            if (dto.Quantity.HasValue &&
                dto.Quantity.Value < 0)
            {
                return BadRequest(
                    "Quantity cannot be negative.");
            }

            if (dto.ReservedQuantity.HasValue &&
                dto.ReservedQuantity.Value < 0)
            {
                return BadRequest(
                    "Reserved quantity cannot be negative.");
            }

            if (dto.UnitPrice.HasValue &&
                dto.UnitPrice.Value < 0)
            {
                return BadRequest(
                    "Unit price cannot be negative.");
            }

            if (dto.MinimumStock.HasValue &&
                dto.MinimumStock.Value < 0)
            {
                return BadRequest(
                    "Minimum stock cannot be negative.");
            }

            // =================================================
            // CHECK RESERVED QUANTITY
            // =================================================

            var newQuantity =
                dto.Quantity ??
                stock.Quantity;

            var newReservedQuantity =
                dto.ReservedQuantity ??
                stock.ReservedQuantity;

            if (newReservedQuantity > newQuantity)
            {
                return BadRequest(
                    "Reserved quantity cannot be greater than quantity.");
            }

            // =================================================
            // CHECK UNIT
            // =================================================

            if (dto.UnitId.HasValue)
            {
                var unit =
                    await unitOfWork.Units
                        .GetByIdAsync(dto.UnitId.Value);

                if (unit == null)
                {
                    return BadRequest(
                        "Unit not found.");
                }
            }

            // =================================================
            // CHECK BIN
            // =================================================

            if (dto.Bin_Id.HasValue)
            {
                var bin =
                    await unitOfWork.Bins
                        .GetByIdAsync(dto.Bin_Id.Value);

                if (bin == null)
                {
                    return BadRequest(
                        "Bin not found.");
                }
            }

            // =================================================
            // UPDATE PROVIDED VALUES ONLY
            // =================================================

            if (dto.Quantity.HasValue)
            {
                stock.Quantity =
                    dto.Quantity.Value;
            }

            if (dto.ReservedQuantity.HasValue)
            {
                stock.ReservedQuantity =
                    dto.ReservedQuantity.Value;
            }

            if (dto.UnitPrice.HasValue)
            {
                stock.UnitPrice =
                    dto.UnitPrice.Value;
            }

            if (dto.MinimumStock.HasValue)
            {
                stock.MinimumStock =
                    dto.MinimumStock.Value;
            }

            if (dto.UnitId.HasValue)
            {
                stock.UnitId =
                    dto.UnitId.Value;
            }

            if (dto.Bin_Id.HasValue)
            {
                stock.Bin_Id =
                    dto.Bin_Id.Value;
            }

            if (dto.ExpiryDate.HasValue)
            {
                stock.ExpiryDate =
                    dto.ExpiryDate.Value;
            }

            if (dto.StockStatus.HasValue)
            {
                stock.StockStatus =
                    dto.StockStatus.Value;
            }

            if (dto.DeliveryStatus.HasValue)
            {
                stock.DeliveryStatus =
                    dto.DeliveryStatus.Value;
            }

            if (dto.IsActive.HasValue)
            {
                stock.IsActive =
                    dto.IsActive.Value;
            }

            // =================================================
            // UPDATE DATE
            // =================================================

            stock.LastUpdatedAt =
                DateTime.UtcNow;

            // =================================================
            // SAVE
            // =================================================

            unitOfWork.Stocks.Update(stock);

            await unitOfWork.SaveAsync();

            // =================================================
            // RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Inventory stock updated successfully.",

                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                sku =
                    stock.Product?.SKU,

                product =
                    stock.Product?.ProductName,

                category =
                    stock.Product?.Category?.Category_Name,

                expiryDate =
                    stock.ExpiryDate,

                location =
                    stock.Bin?.Bin_Name,

                warehouse =
                    stock.Bin?
                        .Shelf?
                        .Row?
                        .Room?
                        .Warehouse?
                        .Warehouse_Name,

                quantity =
                    stock.Quantity,

                reserved =
                    stock.ReservedQuantity,

                available =
                    stock.Quantity -
                    stock.ReservedQuantity,

                unitPrice =
                    stock.UnitPrice,

                unitId =
                    stock.UnitId,

                binId =
                    stock.Bin_Id,

                status =
                    stock.StockStatus,

                deliveryStatus =
                    stock.DeliveryStatus,

                isActive =
                    stock.IsActive,

                lastUpdated =
                    stock.LastUpdatedAt
            });
        }

        // =====================================================
        // 6. CREATE STOCK
        // POST: api/Stock
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // VALIDATION
            // =================================================

            if (dto.Quantity < 0)
            {
                return BadRequest(
                    "Quantity cannot be negative.");
            }

            if (dto.ReservedQuantity < 0)
            {
                return BadRequest(
                    "Reserved quantity cannot be negative.");
            }

            if (dto.ReservedQuantity > dto.Quantity)
            {
                return BadRequest(
                    "Reserved quantity cannot be greater than quantity.");
            }

            if (dto.UnitPrice < 0)
            {
                return BadRequest(
                    "Unit price cannot be negative.");
            }

            if (dto.MinimumStock < 0)
            {
                return BadRequest(
                    "Minimum stock cannot be negative.");
            }


            // =================================================
            // CHECK PRODUCT
            // =================================================

            var product =
                await unitOfWork.Products
                    .GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                return BadRequest(
                    "Product not found.");
            }


            // =================================================
            // CHECK BIN
            // =================================================

            if (dto.Bin_Id.HasValue)
            {
                var bin =
                    await unitOfWork.Bins
                        .GetByIdAsync(
                            dto.Bin_Id.Value);

                if (bin == null)
                {
                    return BadRequest(
                        "Bin not found.");
                }
            }


            // =================================================
            // CHECK UNIT
            // =================================================

            if (dto.UnitId.HasValue)
            {
                var unit =
                    await unitOfWork.Units
                        .GetByIdAsync(
                            dto.UnitId.Value);

                if (unit == null)
                {
                    return BadRequest(
                        "Unit not found.");
                }
            }


            // =================================================
            // GENERATE STOCK CODE
            // =================================================

            var stockCode =
                $"LOT-{DateTime.UtcNow:yyyyMMddHHmmssfff}";


            // =================================================
            // CREATE STOCK
            // =================================================

            var stock = new Stock
            {
                StockCode =
                    stockCode,

                Quantity =
                    dto.Quantity,

                ReservedQuantity =
                    dto.ReservedQuantity,

                UnitPrice =
                    dto.UnitPrice,

                MinimumStock =
                    dto.MinimumStock,

                UnitId =
                    dto.UnitId,

                ProductId =
                    dto.ProductId,

                Bin_Id =
                    dto.Bin_Id,

                ExpiryDate =
                    dto.ExpiryDate,

                StockStatus =
                    dto.StockStatus,

                DeliveryStatus =
                    dto.DeliveryStatus,

                IsActive =
                    true,

                CreateAt =
                    DateTime.UtcNow,

                LastUpdatedAt =
                    DateTime.UtcNow
            };


            // =================================================
            // SAVE
            // =================================================

            await unitOfWork.Stocks
                .AddAsync(stock);

            await unitOfWork.SaveAsync();


            // =================================================
            // RESPONSE
            // =================================================

            return CreatedAtAction(
                nameof(GetById),

                new
                {
                    id = stock.Stock_Id
                },

                new
                {
                    message =
                        "Stock created successfully.",

                    stockId =
                        stock.Stock_Id,

                    stockCode =
                        stock.StockCode,

                    quantity =
                        stock.Quantity,

                    reservedQuantity =
                        stock.ReservedQuantity,

                    availableQuantity =
                        stock.Quantity -
                        stock.ReservedQuantity,

                    unitPrice =
                        stock.UnitPrice,

                    minimumStock =
                        stock.MinimumStock,

                    unitId =
                        stock.UnitId,

                    productId =
                        stock.ProductId,

                    binId =
                        stock.Bin_Id,

                    expiryDate =
                        stock.ExpiryDate,

                    stockStatus =
                        stock.StockStatus,

                    deliveryStatus =
                        stock.DeliveryStatus,

                    isActive =
                        stock.IsActive,

                    createAt =
                        stock.CreateAt,

                    lastUpdatedAt =
                        stock.LastUpdatedAt
                });
        }


        // =====================================================
        // 7. UPDATE STOCK
        // PUT: api/Stock/1
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // GET STOCK
            // =================================================

            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(
                    "Stock not found.");
            }


            // =================================================
            // VALIDATION
            // =================================================

            if (dto.Quantity.HasValue &&
                dto.Quantity.Value < 0)
            {
                return BadRequest(
                    "Quantity cannot be negative.");
            }

            if (dto.ReservedQuantity.HasValue &&
                dto.ReservedQuantity.Value < 0)
            {
                return BadRequest(
                    "Reserved quantity cannot be negative.");
            }

            if (dto.UnitPrice.HasValue &&
                dto.UnitPrice.Value < 0)
            {
                return BadRequest(
                    "Unit price cannot be negative.");
            }

            if (dto.MinimumStock.HasValue &&
                dto.MinimumStock.Value < 0)
            {
                return BadRequest(
                    "Minimum stock cannot be negative.");
            }


            // =================================================
            // VALIDATE RESERVED QUANTITY
            // =================================================

            var newQuantity =
                dto.Quantity ??
                stock.Quantity;

            var newReservedQuantity =
                dto.ReservedQuantity ??
                stock.ReservedQuantity;

            if (newReservedQuantity > newQuantity)
            {
                return BadRequest(
                    "Reserved quantity cannot be greater than quantity.");
            }


            // =================================================
            // CHECK BIN
            // =================================================

            if (dto.Bin_Id.HasValue)
            {
                var bin =
                    await unitOfWork.Bins
                        .GetByIdAsync(
                            dto.Bin_Id.Value);

                if (bin == null)
                {
                    return BadRequest(
                        "Bin not found.");
                }
            }


            // =================================================
            // CHECK UNIT
            // =================================================

            if (dto.UnitId.HasValue)
            {
                var unit =
                    await unitOfWork.Units
                        .GetByIdAsync(
                            dto.UnitId.Value);

                if (unit == null)
                {
                    return BadRequest(
                        "Unit not found.");
                }
            }


            // =================================================
            // UPDATE ONLY PROVIDED VALUES
            // =================================================

            if (dto.Quantity.HasValue)
            {
                stock.Quantity =
                    dto.Quantity.Value;
            }

            if (dto.ReservedQuantity.HasValue)
            {
                stock.ReservedQuantity =
                    dto.ReservedQuantity.Value;
            }

            if (dto.UnitPrice.HasValue)
            {
                stock.UnitPrice =
                    dto.UnitPrice.Value;
            }

            if (dto.MinimumStock.HasValue)
            {
                stock.MinimumStock =
                    dto.MinimumStock.Value;
            }

            if (dto.UnitId.HasValue)
            {
                stock.UnitId =
                    dto.UnitId.Value;
            }

            if (dto.Bin_Id.HasValue)
            {
                stock.Bin_Id =
                    dto.Bin_Id.Value;
            }

            if (dto.ExpiryDate.HasValue)
            {
                stock.ExpiryDate =
                    dto.ExpiryDate.Value;
            }

            if (dto.IsActive.HasValue)
            {
                stock.IsActive =
                    dto.IsActive.Value;
            }

            if (dto.StockStatus.HasValue)
            {
                stock.StockStatus =
                    dto.StockStatus.Value;
            }

            if (dto.DeliveryStatus.HasValue)
            {
                stock.DeliveryStatus =
                    dto.DeliveryStatus.Value;
            }

            stock.LastUpdatedAt =
                DateTime.UtcNow;


            // =================================================
            // SAVE
            // =================================================

            unitOfWork.Stocks.Update(stock);

            await unitOfWork.SaveAsync();


            // =================================================
            // RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Stock updated successfully.",

                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                quantity =
                    stock.Quantity,

                reservedQuantity =
                    stock.ReservedQuantity,

                availableQuantity =
                    stock.Quantity -
                    stock.ReservedQuantity,

                unitPrice =
                    stock.UnitPrice,

                minimumStock =
                    stock.MinimumStock,

                unitId =
                    stock.UnitId,

                productId =
                    stock.ProductId,

                binId =
                    stock.Bin_Id,

                expiryDate =
                    stock.ExpiryDate,

                stockStatus =
                    stock.StockStatus,

                deliveryStatus =
                    stock.DeliveryStatus,

                isActive =
                    stock.IsActive,

                createAt =
                    stock.CreateAt,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 8. UPDATE STOCK STATUS
        // PATCH: api/Stock/1/status
        // =====================================================

        [HttpPatch("UPDATESTOCKSTATUS{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateStockStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(
                    "Stock not found.");
            }

            stock.StockStatus =
                dto.StockStatus;

            stock.LastUpdatedAt =
                DateTime.UtcNow;

            unitOfWork.Stocks.Update(stock);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Stock status updated successfully.",

                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                stockStatus =
                    stock.StockStatus,

                expiryDate =
                    stock.ExpiryDate,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 9. UPDATE DELIVERY STATUS
        // PATCH: api/Stock/1/delivery-status
        // =====================================================

        [HttpPatch("Update{id:int}/delivery-status")]
        public async Task<IActionResult> UpdateDeliveryStatus(
            int id,
            UpdateDeliveryStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(
                    "Stock not found.");
            }

            stock.DeliveryStatus =
                dto.DeliveryStatus;

            stock.LastUpdatedAt =
                DateTime.UtcNow;

            unitOfWork.Stocks.Update(stock);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Delivery status updated successfully.",

                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                deliveryStatus =
                    stock.DeliveryStatus,

                expiryDate =
                    stock.ExpiryDate,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 10. DELETE STOCK
        // DELETE: api/Stock/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(
                    "Stock not found.");
            }


            // =================================================
            // SOFT DELETE
            // =================================================

            stock.IsActive =
                false;

            stock.LastUpdatedAt =
                DateTime.UtcNow;

            unitOfWork.Stocks.Update(stock);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Stock deleted successfully.",

                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                isActive =
                    stock.IsActive,

                expiryDate =
                    stock.ExpiryDate,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 11. SEARCH STOCK BY SITE / DEPARTMENT
        //
        // GET:
        // api/Stock/Search?siteId=1
        //
        // GET:
        // api/Stock/Search?departmentId=2
        //
        // GET:
        // api/Stock/Search?siteId=1&departmentId=2
        // =====================================================

        [HttpGet("Search")]
        public async Task<IActionResult> SearchStock(
            int? siteId,
            int? departmentId)
        {
            if (!siteId.HasValue &&
                !departmentId.HasValue)
            {
                return BadRequest(
                    "Please provide SiteId or DepartmentId.");
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
                    return NotFound(
                        "Site not found.");
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
                    return NotFound(
                        "Department not found.");
                }
            }


            // =================================================
            // SEARCH
            // =================================================

            var stocks =
                await unitOfWork.Stocks
                    .SearchBySiteAndDepartmentAsync(
                        siteId,
                        departmentId);

            if (stocks == null ||
                !stocks.Any())
            {
                return NotFound(
                    "No stock found matching the specified filters.");
            }


            // =================================================
            // RESPONSE
            // =================================================

            var result = stocks.Select(stock => new
            {
                stockId =
                    stock.Stock_Id,

                stockCode =
                    stock.StockCode,

                quantity =
                    stock.Quantity,

                reservedQuantity =
                    stock.ReservedQuantity,

                availableQuantity =
                    stock.Quantity -
                    stock.ReservedQuantity,

                unitPrice =
                    stock.UnitPrice,

                minimumStock =
                    stock.MinimumStock,

                unitId =
                    stock.UnitId,

                expiryDate =
                    stock.ExpiryDate,

                stockStatus =
                    stock.StockStatus,

                deliveryStatus =
                    stock.DeliveryStatus,

                isActive =
                    stock.IsActive,

                createAt =
                    stock.CreateAt,

                lastUpdatedAt =
                    stock.LastUpdatedAt,


                // =================================================
                // PRODUCT
                // =================================================

                productId =
                    stock.ProductId,

                productName =
                    stock.Product?.ProductName,

                sku =
                    stock.Product?.SKU,


                // =================================================
                // BIN
                // =================================================

                binId =
                    stock.Bin_Id,

                binName =
                    stock.Bin?.Bin_Name,


                // =================================================
                // SHELF
                // =================================================

                shelfId =
                    stock.Bin?.Shelf_Id,

                shelfName =
                    stock.Bin?.Shelf?.Shelf_Name,


                // =================================================
                // ROW
                // =================================================

                rowId =
                    stock.Bin?.Shelf?.Row_Id,

                rowName =
                    stock.Bin?.Shelf?.Row?.Row_Name,


                // =================================================
                // ROOM
                // =================================================

                roomId =
                    stock.Bin?.Shelf?.Row?.Room_Id,

                roomName =
                    stock.Bin?.Shelf?.Row?.Room?.Room_Name,


                // =================================================
                // WAREHOUSE
                // =================================================

                warehouseId =
                    stock.Bin?.Shelf?.Row?.Room
                        ?.Warehouse_Id,

                warehouseName =
                    stock.Bin?.Shelf?.Row?.Room
                        ?.Warehouse?.Warehouse_Name,


                // =================================================
                // SITE
                // =================================================

                siteId =
                    stock.Bin?.Shelf?.Row?.Room
                        ?.Warehouse?.Site_Id
            });


            return Ok(new
            {
                count =
                    result.Count(),

                filters = new
                {
                    siteId,
                    departmentId
                },

                stocks =
                    result
            });
        }
    }
}