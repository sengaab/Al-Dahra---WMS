using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Stock;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks =
                await unitOfWork.Stocks
                    .GetAllAsync();

            var result =
                stocks.Select(s => new
                {
                    stockId =
                        s.Stock_Id,

                    quantity =
                        s.Quantity,

                    isActive =
                        s.IsActive,

                    createAt =
                        s.CreateAt,

                    lastUpdatedAt =
                        s.LastUpdatedAt,

                    productId =
                        s.ProductId,

                    binId =
                        s.Bin_Id,

                    stockStatue =
                        s.StockStatue
                });

            return Ok(result);
        }


        // =====================================================
        // 2. GET STOCK BY ID
        // GET: api/Stock/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var stock =
                await unitOfWork.Stocks
                    .GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(
                    "Stock not found.");
            }

            return Ok(new
            {
                stockId =
                    stock.Stock_Id,

                quantity =
                    stock.Quantity,

                isActive =
                    stock.IsActive,

                createAt =
                    stock.CreateAt,

                lastUpdatedAt =
                    stock.LastUpdatedAt,

                productId =
                    stock.ProductId,

                binId =
                    stock.Bin_Id,

                stockStatue =
                    stock.StockStatue
            });
        }


        // =====================================================
        // 3. CREATE STOCK
        // POST: api/Stock
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.Quantity < 0)
            {
                return BadRequest(
                    "Quantity cannot be negative.");
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

            var bin =
                await unitOfWork.Bins
                    .GetByIdAsync(dto.Bin_Id);

            if (bin == null)
            {
                return BadRequest(
                    "Bin not found.");
            }


            // =================================================
            // CREATE STOCK
            // =================================================

            var stock =
                new Stock
                {
                    Quantity =
                        dto.Quantity,

                    ProductId =
                        dto.ProductId,

                    Bin_Id =
                        dto.Bin_Id,

                    StockStatue =
                        dto.StockStatue,

                    IsActive =
                        true,

                    CreateAt =
                        DateTime.UtcNow,

                    LastUpdatedAt =
                        DateTime.UtcNow
                };


            await unitOfWork.Stocks
                .AddAsync(stock);

            await unitOfWork
                .SaveAsync();


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

                    quantity =
                        stock.Quantity,

                    productId =
                        stock.ProductId,

                    binId =
                        stock.Bin_Id,

                    stockStatue =
                        stock.StockStatue,

                    isActive =
                        stock.IsActive,

                    createAt =
                        stock.CreateAt,

                    lastUpdatedAt =
                        stock.LastUpdatedAt
                });
        }


        // =====================================================
        // 4. UPDATE STOCK
        // PUT: api/Stock/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.Quantity < 0)
            {
                return BadRequest(
                    "Quantity cannot be negative.");
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
            // CHECK BIN
            // =================================================

            var bin =
                await unitOfWork.Bins
                    .GetByIdAsync(dto.Bin_Id);

            if (bin == null)
            {
                return BadRequest(
                    "Bin not found.");
            }


            // =================================================
            // UPDATE
            // =================================================

            stock.Quantity =
                dto.Quantity;

            stock.Bin_Id =
                dto.Bin_Id;

            stock.IsActive =
                dto.IsActive;

            stock.StockStatue =
                dto.StockStatue;

            stock.LastUpdatedAt =
                DateTime.UtcNow;


            unitOfWork.Stocks
                .Update(stock);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Stock updated successfully.",

                stockId =
                    stock.Stock_Id,

                quantity =
                    stock.Quantity,

                productId =
                    stock.ProductId,

                binId =
                    stock.Bin_Id,

                stockStatue =
                    stock.StockStatue,

                isActive =
                    stock.IsActive,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 5. UPDATE STOCK STATUS
        // PATCH: api/Stock/{id}/status
        // =====================================================

        [HttpPatch("{id:int}/status")]
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


            stock.StockStatue =
                dto.StockStatue;

            stock.LastUpdatedAt =
                DateTime.UtcNow;


            unitOfWork.Stocks
                .Update(stock);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Stock status updated successfully.",

                stockId =
                    stock.Stock_Id,

                status =
                    stock.StockStatue,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }


        // =====================================================
        // 6. DELETE STOCK
        // DELETE: api/Stock/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
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


            unitOfWork.Stocks
                .Update(stock);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Stock deleted successfully.",

                stockId =
                    stock.Stock_Id,

                isActive =
                    stock.IsActive,

                lastUpdatedAt =
                    stock.LastUpdatedAt
            });
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchStock(
    int? siteId,
    int? departmentId)
        {
            if (!siteId.HasValue && !departmentId.HasValue)
            {
                return BadRequest(
                    "Please provide SiteId or DepartmentId.");
            }

            // =====================================================
            // CHECK SITE
            // =====================================================

            if (siteId.HasValue)
            {
                var site = await unitOfWork.Sites
                    .GetByIdAsync(siteId.Value);

                if (site == null)
                {
                    return NotFound("Site not found.");
                }
            }

            // =====================================================
            // CHECK DEPARTMENT
            // =====================================================

            if (departmentId.HasValue)
            {
                var department =
                    await unitOfWork.Departments
                        .GetByIdAsync(departmentId.Value);

                if (department == null)
                {
                    return NotFound("Department not found.");
                }
            }

            // =====================================================
            // SEARCH
            // =====================================================

            var stocks =
                await unitOfWork.Stocks
                    .SearchBySiteAndDepartmentAsync(
                        siteId,
                        departmentId);

            if (!stocks.Any())
            {
                return NotFound(
                    "No stock found matching the specified filters.");
            }

            // =====================================================
            // RESPONSE
            // =====================================================

            var result = stocks.Select(stock => new
            {
                stockId = stock.Stock_Id,

                quantity = stock.Quantity,

                isActive = stock.IsActive,

                stockStatue = stock.StockStatue,

                createAt = stock.CreateAt,

                lastUpdatedAt = stock.LastUpdatedAt,

                // Product
                productId = stock.ProductId,

                productName = stock.Product?.ProductName,

                sku = stock.Product?.SKU,

                // Bin
                binId = stock.Bin_Id,

                binName = stock.Bin?.Bin_Name,

                // Location
                shelfId = stock.Bin?.Shelf_Id,

                shelfName = stock.Bin?.Shelf?.Shelf_Name,

                rowId = stock.Bin?.Shelf?.Row_Id,

                rowName = stock.Bin?.Shelf?.Row?.Row_Name,

                roomId = stock.Bin?.Shelf?.Row?.Room_Id,

                roomName = stock.Bin?.Shelf?.Row?.Room?.Room_Name,

                warehouseId =
                    stock.Bin?.Shelf?.Row?.Room?.Warehouse_Id,

                warehouseName =
                    stock.Bin?.Shelf?.Row?.Room?.Warehouse
                        ?.Warehouse_Name,

                siteId =
                    stock.Bin?.Shelf?.Row?.Room?.Warehouse
                        ?.Site_Id
            });

            return Ok(new
            {
                count = result.Count(),

                filters = new
                {
                    siteId,
                    departmentId
                },

                stocks = result
            });
        }
    }
}