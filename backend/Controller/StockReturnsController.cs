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
    [Route("api/stock-returns")]
    [Authorize]
    public class StockReturnsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataBaseContext _context;

        public StockReturnsController(
            IUnitOfWork unitOfWork,
            DataBaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        // =====================================================
        // GET /api/stock-returns
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var returns =
                await _unitOfWork.StockReturns
                    .GetAllAsync();

            var result = returns
                .Select(MapToResponse)
                .ToList();

            return Ok(result);
        }


        // =====================================================
        // GET /api/stock-returns/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }

            return Ok(MapToResponse(stockReturn));
        }


        // =====================================================
        // POST /api/stock-returns
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockReturnDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "At least one return item is required."
                });
            }


            // =================================================
            // Validate User
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId == dto.ReturnedBy);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message =
                        "ReturnedBy user not found."
                });
            }


            // =================================================
            // Validate Issue
            // =================================================

            var issueExists =
                await _context.StockIssues
                    .AnyAsync(x =>
                        x.IssueId == dto.IssueId);

            if (!issueExists)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Issue not found."
                });
            }


            // =================================================
            // Validate Warehouse
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
            // Validate Department
            // =================================================

            var departmentExists =
                await _context.Departments
                    .AnyAsync(x =>
                        x.DepartmentId ==
                        dto.DepartmentId);

            if (!departmentExists)
            {
                return BadRequest(new
                {
                    message =
                        "Department not found."
                });
            }


            // =================================================
            // Create Return
            // =================================================

            var stockReturn =
                new StockReturn
                {
                    ReturnNumber =
                        $"RET-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                    IssueId =
                        dto.IssueId,

                    WarehouseId =
                        dto.WarehouseId,

                    DepartmentId =
                        dto.DepartmentId,

                    ReturnedBy =
                        dto.ReturnedBy,

                    ReturnedAt =
                        DateTimeOffset.UtcNow,

                    stockReturnStatus =
                        StockReturnStatus.Pending,

                    Reason =
                        dto.Reason
                };


            // =================================================
            // Validate & Add Items
            // =================================================

            foreach (var itemDto in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            itemDto.StockId &&
                            x.ProductId ==
                            itemDto.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {itemDto.StockId} not found for Product {itemDto.ProductId}."
                    });
                }


                if (itemDto.Quantity <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "Return quantity must be greater than zero."
                    });
                }


                var item =
                    new StockReturnItem
                    {
                        ProductId =
                            itemDto.ProductId,

                        StockId =
                            itemDto.StockId,

                        Quantity =
                            itemDto.Quantity,

                        Condition =
                            itemDto.Condition
                    };

                stockReturn.Items.Add(item);
            }


            await _unitOfWork.StockReturns
                .AddAsync(stockReturn);

            await _unitOfWork.SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = stockReturn.ReturnId
                },
                MapToResponse(stockReturn));
        }


        // =====================================================
        // PUT /api/stock-returns/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateStockReturnDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            // =================================================
            // Only Pending can be edited
            // =================================================

            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be updated from status {stockReturn.stockReturnStatus}."
                });
            }


            // =================================================
            // Update Header
            // =================================================

            stockReturn.IssueId =
                dto.IssueId;

            stockReturn.WarehouseId =
                dto.WarehouseId;

            stockReturn.DepartmentId =
                dto.DepartmentId;

            stockReturn.ReturnedBy =
                dto.ReturnedBy;

            stockReturn.Reason =
                dto.Reason;


            // =================================================
            // Replace Items
            // =================================================

            _context.StockReturnItems.RemoveRange(
                stockReturn.Items);

            stockReturn.Items.Clear();


            foreach (var itemDto in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            itemDto.StockId &&
                            x.ProductId ==
                            itemDto.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {itemDto.StockId} not found for Product {itemDto.ProductId}."
                    });
                }


                var item =
                    new StockReturnItem
                    {
                        ReturnId =
                            id,

                        ProductId =
                            itemDto.ProductId,

                        StockId =
                            itemDto.StockId,

                        Quantity =
                            itemDto.Quantity,

                        Condition =
                            itemDto.Condition
                    };

                stockReturn.Items.Add(item);
            }


            await _unitOfWork.SaveAsync();


            return Ok(MapToResponse(stockReturn));
        }


        // =====================================================
        // POST /api/stock-returns/{id}/inspect
        // =====================================================

        [HttpPost("{id:int}/inspect")]
        public async Task<IActionResult> Inspect(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be inspected from status {stockReturn.stockReturnStatus}."
                });
            }


            stockReturn.stockReturnStatus =
                StockReturnStatus.PendingInspection;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Return sent for inspection.",

                stockReturn =
                    MapToResponse(stockReturn)
            });
        }


        // =====================================================
        // POST /api/stock-returns/{id}/accept
        // =====================================================

        [HttpPost("{id:int}/accept")]
        public async Task<IActionResult> Accept(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.PendingInspection)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be accepted from status {stockReturn.stockReturnStatus}."
                });
            }


            stockReturn.stockReturnStatus =
                StockReturnStatus.Accepted;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Return accepted successfully.",

                stockReturn =
                    MapToResponse(stockReturn)
            });
        }


        // =====================================================
        // POST /api/stock-returns/{id}/quarantine
        // =====================================================

        [HttpPost("{id:int}/quarantine")]
        public async Task<IActionResult> Quarantine(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.PendingInspection)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be quarantined from status {stockReturn.stockReturnStatus}."
                });
            }


            stockReturn.stockReturnStatus =
                StockReturnStatus.Quarantined;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Return moved to quarantine.",

                stockReturn =
                    MapToResponse(stockReturn)
            });
        }


        // =====================================================
        // POST /api/stock-returns/{id}/reject
        // =====================================================

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.PendingInspection)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be rejected from status {stockReturn.stockReturnStatus}."
                });
            }


            stockReturn.stockReturnStatus =
                StockReturnStatus.Rejected;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Return rejected successfully.",

                stockReturn =
                    MapToResponse(stockReturn)
            });
        }


        // =====================================================
        // POST /api/stock-returns/{id}/complete
        // =====================================================

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var stockReturn =
                await _unitOfWork.StockReturns
                    .GetByIdForUpdateAsync(id);

            if (stockReturn == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Return not found."
                });
            }


            if (stockReturn.stockReturnStatus !=
                StockReturnStatus.Accepted)
            {
                return BadRequest(new
                {
                    message =
                        $"Return cannot be completed from status {stockReturn.stockReturnStatus}."
                });
            }


            // =================================================
            // Transaction
            // =================================================

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                // =============================================
                // Add returned quantity to Stock
                // =============================================

                foreach (var item in stockReturn.Items)
                {
                    var stock =
                        await _context.Stocks
                            .FirstOrDefaultAsync(x =>
                                x.StockId ==
                                item.StockId &&
                                x.ProductId ==
                                item.ProductId);

                    if (stock == null)
                    {
                        throw new InvalidOperationException(
                            $"Stock {item.StockId} not found.");
                    }


                    stock.Quantity +=
                        item.Quantity;
                }


                // =============================================
                // Update Return
                // =============================================

                stockReturn.stockReturnStatus =
                    StockReturnStatus.Completed;


                // =============================================
                // Save
                // =============================================

                await _context.SaveChangesAsync();


                // =============================================
                // Commit
                // =============================================

                await transaction.CommitAsync();


                return Ok(new
                {
                    message =
                        "Stock Return completed successfully.",

                    stockReturn =
                        MapToResponse(stockReturn)
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message =
                            "Failed to complete stock return.",

                        error =
                            ex.Message
                    });
            }
        }


        // =====================================================
        // MAPPER
        // =====================================================

        private static StockReturnResponseDTO
            MapToResponse(
                StockReturn stockReturn)
        {
            return new StockReturnResponseDTO
            {
                ReturnId =
                    stockReturn.ReturnId,

                ReturnNumber =
                    stockReturn.ReturnNumber,

                IssueId =
                    stockReturn.IssueId,

                WarehouseId =
                    stockReturn.WarehouseId,

                DepartmentId =
                    stockReturn.DepartmentId,

                ReturnedBy =
                    stockReturn.ReturnedBy,

                ReturnedAt =
                    stockReturn.ReturnedAt,

                StockReturnStatus =
                    stockReturn.stockReturnStatus,

                Reason =
                    stockReturn.Reason,

                Items =
                    stockReturn.Items
                        .Select(item =>
                            new StockReturnItemResponseDTO
                            {
                                ReturnItemId =
                                    item.ReturnItemId,

                                ProductId =
                                    item.ProductId,

                                StockId =
                                    item.StockId,

                                Quantity =
                                    item.Quantity,

                                Condition =
                                    item.Condition
                            })
                        .ToList()
            };
        }
    }
}