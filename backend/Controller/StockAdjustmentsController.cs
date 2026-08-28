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
    [Route("api/stock-adjustments")]
    [Authorize]
    public class StockAdjustmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataBaseContext _context;

        public StockAdjustmentsController(
            IUnitOfWork unitOfWork,
            DataBaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        // =====================================================
        // GET /api/stock-adjustments
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adjustments =
                await _unitOfWork.StockAdjustments
                    .GetAllAsync();

            var result =
                adjustments
                    .Select(MapToResponse)
                    .ToList();

            return Ok(result);
        }


        // =====================================================
        // GET /api/stock-adjustments/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adjustment =
                await _unitOfWork.StockAdjustments
                    .GetByIdAsync(id);

            if (adjustment == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Adjustment not found."
                });
            }

            return Ok(MapToResponse(adjustment));
        }


        // =====================================================
        // POST /api/stock-adjustments
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockAdjustmentDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =================================================
            // Validate User
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
            // Validate Stock
            // =================================================

            var stock =
                await _context.Stocks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.StockId ==
                        dto.StockId &&
                        x.ProductId ==
                        dto.ProductId);

            if (stock == null)
            {
                return BadRequest(new
                {
                    message =
                        $"Stock {dto.StockId} not found for Product {dto.ProductId}."
                });
            }


            // =================================================
            // Adjustment cannot create negative stock
            // =================================================

            var newQuantity =
                stock.Quantity +
                dto.AdjustmentQuantity;

            if (newQuantity < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Adjustment would result in negative stock.",

                    currentQuantity =
                        stock.Quantity,

                    adjustmentQuantity =
                        dto.AdjustmentQuantity,

                    newQuantity =
                        newQuantity
                });
            }


            // =================================================
            // Generate Number
            // =================================================

            var adjustmentNumber =
                $"ADJ-{DateTime.UtcNow:yyyyMMddHHmmssfff}";


            // =================================================
            // Create
            // =================================================

            var adjustment =
                new StockAdjustment
                {
                    AdjustmentNumber =
                        adjustmentNumber,

                    StockId =
                        dto.StockId,

                    ProductId =
                        dto.ProductId,

                    PreviousQuantity =
                        stock.Quantity,

                    AdjustmentQuantity =
                        dto.AdjustmentQuantity,

                    // This is only the expected quantity
                    // and will be recalculated during APPLY.
                    NewQuantity =
                        newQuantity,

                    Reason =
                        dto.Reason,

                    CreatedBy =
                        dto.CreatedBy,

                    CreatedAt =
                        DateTimeOffset.UtcNow,

                    StockAdjustmentStatus =
                        StockAdjustmentStatus.Pending
                };


            await _unitOfWork.StockAdjustments
                .AddAsync(adjustment);

            await _unitOfWork.SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = adjustment.AdjustmentId
                },
                MapToResponse(adjustment));
        }


        // =====================================================
        // POST /api/stock-adjustments/{id}/submit
        // =====================================================

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var adjustment =
                await _unitOfWork.StockAdjustments
                    .GetByIdForUpdateAsync(id);

            if (adjustment == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Adjustment not found."
                });
            }


            if (adjustment.StockAdjustmentStatus !=
                StockAdjustmentStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Adjustment cannot be submitted from status {adjustment.StockAdjustmentStatus}."
                });
            }


            // =================================================
            // Submit
            // =================================================

            // Pending remains Pending.
            // This endpoint represents the workflow action.
            adjustment.StockAdjustmentStatus =
                StockAdjustmentStatus.Pending;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Adjustment submitted successfully.",

                adjustment =
                    MapToResponse(adjustment)
            });
        }


        // =====================================================
        // POST /api/stock-adjustments/{id}/approve
        // =====================================================

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            [FromBody] StockAdjustmentActionDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var adjustment =
                await _unitOfWork.StockAdjustments
                    .GetByIdForUpdateAsync(id);

            if (adjustment == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Adjustment not found."
                });
            }


            if (adjustment.StockAdjustmentStatus !=
                StockAdjustmentStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Adjustment cannot be approved from status {adjustment.StockAdjustmentStatus}."
                });
            }


            // =================================================
            // Validate Approver
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId ==
                        dto.UserId);

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

            adjustment.ApprovedBy =
                dto.UserId;

            adjustment.StockAdjustmentStatus =
                StockAdjustmentStatus.Approved;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Adjustment approved successfully.",

                adjustment =
                    MapToResponse(adjustment)
            });
        }


        // =====================================================
        // POST /api/stock-adjustments/{id}/reject
        // =====================================================

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(
            int id,
            [FromBody] StockAdjustmentActionDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var adjustment =
                await _unitOfWork.StockAdjustments
                    .GetByIdForUpdateAsync(id);

            if (adjustment == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Adjustment not found."
                });
            }


            if (adjustment.StockAdjustmentStatus !=
                StockAdjustmentStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Adjustment cannot be rejected from status {adjustment.StockAdjustmentStatus}."
                });
            }


            // =================================================
            // Validate User
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId ==
                        dto.UserId);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message =
                        "User not found."
                });
            }


            // =================================================
            // Reject
            // =================================================

            adjustment.ApprovedBy =
                dto.UserId;

            adjustment.StockAdjustmentStatus =
                StockAdjustmentStatus.Rejected;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Adjustment rejected successfully.",

                adjustment =
                    MapToResponse(adjustment)
            });
        }


        // =====================================================
        // POST /api/stock-adjustments/{id}/apply
        // =====================================================

        [HttpPost("{id:int}/apply")]
        public async Task<IActionResult> Apply(int id)
        {
            var adjustment =
                await _unitOfWork.StockAdjustments
                    .GetByIdForUpdateAsync(id);

            if (adjustment == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Adjustment not found."
                });
            }


            if (adjustment.StockAdjustmentStatus !=
                StockAdjustmentStatus.Approved)
            {
                return BadRequest(new
                {
                    message =
                        $"Adjustment cannot be applied from status {adjustment.StockAdjustmentStatus}."
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
                // Get current stock
                // =============================================

                var stock =
                    await _context.Stocks
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            adjustment.StockId &&
                            x.ProductId ==
                            adjustment.ProductId);

                if (stock == null)
                {
                    throw new InvalidOperationException(
                        $"Stock {adjustment.StockId} not found.");
                }


                // =============================================
                // Current quantity
                // =============================================

                var previousQuantity =
                    stock.Quantity;


                // =============================================
                // Calculate new quantity
                // =============================================

                var newQuantity =
                    previousQuantity +
                    adjustment.AdjustmentQuantity;


                // =============================================
                // Prevent negative stock
                // =============================================

                if (newQuantity < 0)
                {
                    throw new InvalidOperationException(
                        "Adjustment would result in negative stock.");
                }


                // =============================================
                // Update Stock
                // =============================================

                stock.Quantity =
                    newQuantity;


                // =============================================
                // Update Adjustment
                // =============================================

                adjustment.PreviousQuantity =
                    previousQuantity;

                adjustment.NewQuantity =
                    newQuantity;

                adjustment.StockAdjustmentStatus =
                    StockAdjustmentStatus.Applied;


                // =============================================
                // Save
                // =============================================

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return Ok(new
                {
                    message =
                        "Stock Adjustment applied successfully.",

                    adjustment =
                        MapToResponse(adjustment)
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message =
                        "Stock Adjustment application failed.",

                    error =
                        ex.Message
                });
            }
        }


        // =====================================================
        // MAPPER
        // =====================================================

        private static StockAdjustmentResponseDTO
            MapToResponse(
                StockAdjustment adjustment)
        {
            return new StockAdjustmentResponseDTO
            {
                AdjustmentId =
                    adjustment.AdjustmentId,

                AdjustmentNumber =
                    adjustment.AdjustmentNumber,

                StockId =
                    adjustment.StockId,

                ProductId =
                    adjustment.ProductId,

                PreviousQuantity =
                    adjustment.PreviousQuantity,

                AdjustmentQuantity =
                    adjustment.AdjustmentQuantity,

                NewQuantity =
                    adjustment.NewQuantity,

                Reason =
                    adjustment.Reason,

                CreatedBy =
                    adjustment.CreatedBy,

                ApprovedBy =
                    adjustment.ApprovedBy,

                CreatedAt =
                    adjustment.CreatedAt,

                StockAdjustmentStatus =
                    adjustment.StockAdjustmentStatus
            };
        }
    }
}