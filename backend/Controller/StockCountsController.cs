using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/stock-counts")]
    [Authorize]
    public class StockCountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataBaseContext _context;

        public StockCountsController(
            IUnitOfWork unitOfWork,
            DataBaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        // =====================================================
        // GET /api/stock-counts
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var counts =
                await _unitOfWork.StockCounts.GetAllAsync();

            return Ok(
                counts.Select(MapToResponse).ToList());
        }


        // =====================================================
        // GET /api/stock-counts/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var count =
                await _unitOfWork.StockCounts
                    .GetByIdAsync(id);

            if (count == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }

            return Ok(MapToResponse(count));
        }


        // =====================================================
        // POST /api/stock-counts
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockCountDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Count must contain at least one item."
                });
            }


            // =================================================
            // Warehouse
            // =================================================

            var warehouseExists =
                await _context.Warehouses
                    .AnyAsync(x =>
                        x.WarehouseId ==
                        dto.WarehouseId);

            if (!warehouseExists)
            {
                return BadRequest(new
                {
                    message =
                        "Warehouse not found."
                });
            }


            // =================================================
            // Created By
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId ==
                        dto.CreatedBy);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message =
                        "CreatedBy user not found."
                });
            }


            // =================================================
            // Location
            // =================================================

            if (dto.LocationId.HasValue)
            {
                var locationExists =
                    await _context.Locations
                        .AnyAsync(x =>
                            x.LocationId ==
                            dto.LocationId.Value);

                if (!locationExists)
                {
                    return BadRequest(new
                    {
                        message =
                            "Location not found."
                    });
                }
            }


            // =================================================
            // Validate Items
            // =================================================

            foreach (var item in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            item.StockId &&
                            x.ProductId ==
                            item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {item.StockId} not found for Product {item.ProductId}."
                    });
                }
            }


            // =================================================
            // Generate Count Number
            // =================================================

            var countNumber =
                $"CNT-{DateTime.UtcNow:yyyyMMddHHmmssfff}";


            // =================================================
            // Create Stock Count
            // =================================================

            var stockCount = new StockCount
            {
                CountNumber =
                    countNumber,

                WarehouseId =
                    dto.WarehouseId,

                LocationId =
                    dto.LocationId,

                CreatedBy =
                    dto.CreatedBy,

                CountDate =
                    dto.CountDate == default
                        ? DateTimeOffset.UtcNow
                        : dto.CountDate,

                stockCountStatus =
                    StockCountStatus.Draft
            };


            // =================================================
            // Create Items
            // =================================================

            foreach (var item in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstAsync(x =>
                            x.StockId ==
                            item.StockId &&
                            x.ProductId ==
                            item.ProductId);

                stockCount.Items.Add(
                    new StockCountItem
                    {
                        StockId =
                            item.StockId,

                        ProductId =
                            item.ProductId,

                        ExpectedQuantity =
                            stock.Quantity,

                        CountedQuantity =
                            0,

                        Variance =
                            0
                    });
            }


            await _unitOfWork.StockCounts
                .AddAsync(stockCount);

            await _unitOfWork.SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = stockCount.StockCountId
                },
                MapToResponse(stockCount));
        }


        // =====================================================
        // PUT /api/stock-counts/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStockCountDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var stockCount =
                await _unitOfWork.StockCounts
                    .GetByIdForUpdateAsync(id);

            if (stockCount == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }


            // Only Draft can be edited
            if (stockCount.stockCountStatus !=
                StockCountStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        $"Stock Count cannot be updated from status {stockCount.stockCountStatus}."
                });
            }


            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Count must contain at least one item."
                });
            }


            stockCount.WarehouseId =
                dto.WarehouseId;

            stockCount.LocationId =
                dto.LocationId;

            stockCount.CountDate =
                dto.CountDate == default
                    ? stockCount.CountDate
                    : dto.CountDate;


            // =================================================
            // Remove old items
            // =================================================

            _context.StockCountItems
                .RemoveRange(stockCount.Items);

            stockCount.Items.Clear();


            // =================================================
            // Add new items
            // =================================================

            foreach (var item in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            item.StockId &&
                            x.ProductId ==
                            item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {item.StockId} not found for Product {item.ProductId}."
                    });
                }


                stockCount.Items.Add(
                    new StockCountItem
                    {
                        StockCountId =
                            stockCount.StockCountId,

                        StockId =
                            item.StockId,

                        ProductId =
                            item.ProductId,

                        ExpectedQuantity =
                            stock.Quantity,

                        CountedQuantity =
                            0,

                        Variance =
                            0
                    });
            }


            await _unitOfWork.SaveAsync();


            return Ok(MapToResponse(stockCount));
        }


        // =====================================================
        // POST /api/stock-counts/{id}/start
        // =====================================================

        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var stockCount =
                await _unitOfWork.StockCounts
                    .GetByIdForUpdateAsync(id);

            if (stockCount == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }


            if (stockCount.stockCountStatus !=
                StockCountStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        $"Stock Count cannot be started from status {stockCount.stockCountStatus}."
                });
            }


            if (stockCount.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Count has no items."
                });
            }


            stockCount.stockCountStatus =
                StockCountStatus.InProgress;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Count started successfully.",

                stockCount =
                    MapToResponse(stockCount)
            });
        }


        // =====================================================
        // POST /api/stock-counts/{id}/items/{itemId}/count
        // =====================================================

        [HttpPost("{id:int}/items/{itemId:int}/count")]
        public async Task<IActionResult> CountItem(
            int id,
            int itemId,
            [FromBody] CountStockCountItemDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var stockCount =
                await _unitOfWork.StockCounts
                    .GetByIdForUpdateAsync(id);

            if (stockCount == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }


            if (stockCount.stockCountStatus !=
                    StockCountStatus.InProgress &&
                stockCount.stockCountStatus !=
                    StockCountStatus.PartiallyCounted)
            {
                return BadRequest(new
                {
                    message =
                        $"Items cannot be counted from status {stockCount.stockCountStatus}."
                });
            }


            var item =
                stockCount.Items
                    .FirstOrDefault(x =>
                        x.StockCountItemId ==
                        itemId);

            if (item == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count Item not found."
                });
            }


            // =================================================
            // Count
            // =================================================

            item.CountedQuantity =
                dto.CountedQuantity;

            item.Variance =
                dto.CountedQuantity -
                item.ExpectedQuantity;

            item.Reason =
                dto.Reason;


            // =================================================
            // Determine status
            // =================================================

            var countedItems =
                stockCount.Items.Count(x =>
                    x.CountedQuantity >= 0 &&
                    (
                        x.CountedQuantity != 0 ||
                        x.ExpectedQuantity == 0
                    ));

            if (countedItems ==
                stockCount.Items.Count)
            {
                stockCount.stockCountStatus =
                    StockCountStatus.Counted;
            }
            else
            {
                stockCount.stockCountStatus =
                    StockCountStatus.PartiallyCounted;
            }


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Count Item counted successfully.",

                item = new StockCountItemResponseDTO
                {
                    StockCountItemId =
                        item.StockCountItemId,

                    StockId =
                        item.StockId,

                    ProductId =
                        item.ProductId,

                    ExpectedQuantity =
                        item.ExpectedQuantity,

                    CountedQuantity =
                        item.CountedQuantity,

                    Variance =
                        item.Variance,

                    Reason =
                        item.Reason
                }
            });
        }


        // =====================================================
        // POST /api/stock-counts/{id}/complete
        // =====================================================

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var stockCount =
                await _unitOfWork.StockCounts
                    .GetByIdForUpdateAsync(id);

            if (stockCount == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }


            if (stockCount.stockCountStatus !=
                StockCountStatus.Counted)
            {
                return BadRequest(new
                {
                    message =
                        $"Stock Count cannot be completed from status {stockCount.stockCountStatus}."
                });
            }


            // =================================================
            // Move to Approval
            // =================================================

            stockCount.stockCountStatus =
                StockCountStatus.PendingApproval;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Count submitted for approval.",

                stockCount =
                    MapToResponse(stockCount)
            });
        }


        // =====================================================
        // POST /api/stock-counts/{id}/approve
        // =====================================================

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            [FromBody] ApproveStockCountDTO dto)
        {
            var stockCount =
                await _unitOfWork.StockCounts
                    .GetByIdForUpdateAsync(id);

            if (stockCount == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Count not found."
                });
            }


            if (stockCount.stockCountStatus !=
                StockCountStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        $"Stock Count cannot be approved from status {stockCount.stockCountStatus}."
                });
            }


            // =================================================
            // Validate Approver
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId ==
                        dto.ApprovedBy);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message =
                        "Approver user not found."
                });
            }


            // =================================================
            // Approve
            // =================================================

            stockCount.ApprovedBy =
                dto.ApprovedBy;

            stockCount.stockCountStatus =
                StockCountStatus.Approved;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Count approved successfully.",

                stockCount =
                    MapToResponse(stockCount)
            });
        }


        // =====================================================
        // MAPPER
        // =====================================================

        private static StockCountResponseDTO MapToResponse(
            StockCount stockCount)
        {
            return new StockCountResponseDTO
            {
                StockCountId =
                    stockCount.StockCountId,

                CountNumber =
                    stockCount.CountNumber,

                WarehouseId =
                    stockCount.WarehouseId,

                LocationId =
                    stockCount.LocationId,

                CreatedBy =
                    stockCount.CreatedBy,

                ApprovedBy =
                    stockCount.ApprovedBy,

                StockCountStatus =
                    stockCount.stockCountStatus,

                CountDate =
                    stockCount.CountDate,

                Items =
                    stockCount.Items
                        .Select(item =>
                            new StockCountItemResponseDTO
                            {
                                StockCountItemId =
                                    item.StockCountItemId,

                                StockId =
                                    item.StockId,

                                ProductId =
                                    item.ProductId,

                                ExpectedQuantity =
                                    item.ExpectedQuantity,

                                CountedQuantity =
                                    item.CountedQuantity,

                                Variance =
                                    item.Variance,

                                Reason =
                                    item.Reason
                            })
                        .ToList()
            };
        }
    }
}