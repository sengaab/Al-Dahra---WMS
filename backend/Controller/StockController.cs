using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Stock;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/stock
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            var stocks = await _unitOfWork.Stocks.GetAllAsync();

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await _unitOfWork.Stocks.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(new
                {
                    message = "Stock not found."
                });
            }

            return Ok(stock);
        }


        // =====================================================
        // GET /api/stock/product/{productId}
        // =====================================================

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetStockByProduct(int productId)
        {
            var stocks =
                await _unitOfWork.Stocks.GetByProductAsync(productId);

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/location/{locationId}
        // =====================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetStockByLocation(int locationId)
        {
            var stocks =
                await _unitOfWork.Stocks.GetByLocationAsync(locationId);

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/warehouse/{warehouseId}
        // =====================================================

        [HttpGet("warehouse/{warehouseId:int}")]
        public async Task<IActionResult> GetStockByWarehouse(int warehouseId)
        {
            var stocks =
                await _unitOfWork.Stocks.GetByWarehouseAsync(warehouseId);

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/available
        // =====================================================

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableStock()
        {
            var stocks =
                await _unitOfWork.Stocks.GetAvailableAsync();

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/low
        // =====================================================

        [HttpGet("low")]
        public async Task<IActionResult> GetLowStock()
        {
            var stocks =
                await _unitOfWork.Stocks.GetLowStockAsync();

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/out-of-stock
        // =====================================================

        [HttpGet("out-of-stock")]
        public async Task<IActionResult> GetOutOfStock()
        {
            var stocks =
                await _unitOfWork.Stocks.GetOutOfStockAsync();

            return Ok(stocks);
        }


        // =====================================================
        // GET /api/stock/summary
        // =====================================================

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary =
                await _unitOfWork.Stocks.GetSummaryAsync();

            return Ok(summary);
        }


        // =====================================================
        // GET /api/stock/total-quantity
        // =====================================================

        [HttpGet("total-quantity")]
        public async Task<IActionResult> GetTotalQuantity()
        {
            var total =
                await _unitOfWork.Stocks.GetTotalQuantityAsync();

            return Ok(new
            {
                totalQuantity = total
            });
        }


        // =====================================================
        // GET /api/stock/total-value
        // =====================================================

        [HttpGet("total-value")]
        public async Task<IActionResult> GetTotalValue()
        {
            var total =
                await _unitOfWork.Stocks.GetTotalValueAsync();

            return Ok(new
            {
                totalValue = total
            });
        }


        // =====================================================
        // POST /api/stock
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateStock(
            [FromBody] CreateStockDto dto)
        {
            // =========================
            // Validate Quantity
            // =========================

            if (dto.Quantity < 0)
            {
                return BadRequest(new
                {
                    message = "Quantity cannot be negative."
                });
            }


            // =========================
            // Validate Reserved Quantity
            // =========================

            if (dto.ReservedQuantity < 0)
            {
                return BadRequest(new
                {
                    message = "Reserved quantity cannot be negative."
                });
            }


            if (dto.ReservedQuantity > dto.Quantity)
            {
                return BadRequest(new
                {
                    message =
                        "Reserved quantity cannot be greater than quantity."
                });
            }


            // =========================
            // Validate Supplier
            // =========================

            if (dto.SupplierId.HasValue)
            {
                var supplier = await _unitOfWork.Suppliers
                    .GetEntityByIdAsync(dto.SupplierId.Value);

                if (supplier == null)
                {
                    return BadRequest(new
                    {
                        message = "Supplier not found."
                    });
                }
            }


            // =========================
            // Create Stock
            // =========================

            var stock = new Stock
            {
                ProductId = dto.ProductId,

                WarehouseId = dto.WarehouseId,

                LocationId = dto.LocationId,

                SupplierId = dto.SupplierId,

                BatchNumber = dto.BatchNumber,

                ExpiryDate = dto.ExpiryDate,

                Quantity = dto.Quantity,

                ReservedQuantity = dto.ReservedQuantity,

                AvailableQuantity =
                    dto.Quantity - dto.ReservedQuantity,

                UnitPrice = dto.UnitPrice,

                MinimumStock = dto.MinimumStock,

                stockStatus = StockStatus.Available,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow,

                StockCode = await GenerateStockCode()
            };


            await _unitOfWork.Stocks.AddAsync(stock);

            await _unitOfWork.SaveAsync();


            // =========================
            // Return Created Stock
            // =========================

            var result =
                await _unitOfWork.Stocks.GetByIdAsync(stock.StockId);

            return CreatedAtAction(
                nameof(GetStockById),
                new { id = stock.StockId },
                result);
        }


        // =====================================================
        // PUT /api/stock/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStock(
            int id,
            [FromBody] UpdateStockDto dto)
        {
            // =========================
            // Get Stock
            // =========================

            var stock =
                await _unitOfWork.Stocks.GetEntityByIdAsync(id);

            if (stock == null)
            {
                return NotFound(new
                {
                    message = "Stock not found."
                });
            }


            // =========================
            // Validate Quantity
            // =========================

            if (dto.Quantity < 0)
            {
                return BadRequest(new
                {
                    message = "Quantity cannot be negative."
                });
            }


            // =========================
            // Validate Reserved Quantity
            // =========================

            if (dto.ReservedQuantity < 0)
            {
                return BadRequest(new
                {
                    message = "Reserved quantity cannot be negative."
                });
            }


            if (dto.ReservedQuantity > dto.Quantity)
            {
                return BadRequest(new
                {
                    message =
                        "Reserved quantity cannot be greater than quantity."
                });
            }


            // =========================
            // Validate Supplier
            // =========================

            if (dto.SupplierId.HasValue)
            {
                var supplier = await _unitOfWork.Suppliers
                    .GetEntityByIdAsync(dto.SupplierId.Value);

                if (supplier == null)
                {
                    return BadRequest(new
                    {
                        message = "Supplier not found."
                    });
                }
            }


            // =========================
            // Update Stock
            // =========================

            stock.LocationId = dto.LocationId;

            stock.SupplierId = dto.SupplierId;

            stock.BatchNumber = dto.BatchNumber;

            stock.ExpiryDate = dto.ExpiryDate;

            stock.Quantity = dto.Quantity;

            stock.ReservedQuantity =
                dto.ReservedQuantity;

            stock.AvailableQuantity =
                dto.Quantity - dto.ReservedQuantity;

            stock.UnitPrice = dto.UnitPrice;

            stock.MinimumStock =
                dto.MinimumStock;


            // =========================
            // Update Status
            // =========================

            if (!string.IsNullOrWhiteSpace(dto.StockStatus) &&
                Enum.TryParse<StockStatus>(
                    dto.StockStatus,
                    true,
                    out var status))
            {
                stock.stockStatus = status;
            }


            stock.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Stocks.Update(stock);

            await _unitOfWork.SaveAsync();


            // =========================
            // Return Updated Stock
            // =========================

            var result =
                await _unitOfWork.Stocks.GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE /api/stock/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock =
                await _unitOfWork.Stocks.GetEntityByIdAsync(id);

            if (stock == null)
            {
                return NotFound(new
                {
                    message = "Stock not found."
                });
            }


            _unitOfWork.Stocks.Delete(stock);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Stock deleted successfully."
            });
        }


        // =====================================================
        // GENERATE STOCK CODE
        // =====================================================

        private async Task<string> GenerateStockCode()
        {
            var prefix = "STK";

            var lastStock =
                (await _unitOfWork.Stocks.GetAllAsync())
                .OrderByDescending(x => x.StockId)
                .FirstOrDefault();

            var number = 1;

            if (lastStock != null)
            {
                number = lastStock.StockId + 1;
            }

            return $"{prefix}{number:D6}";
        }
    }
}