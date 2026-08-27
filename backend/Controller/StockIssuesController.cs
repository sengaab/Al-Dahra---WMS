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
    [Route("api/stock-issues")]
    [Authorize]
    public class StockIssuesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataBaseContext _context;

        public StockIssuesController(
            IUnitOfWork unitOfWork,
            DataBaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // =====================================================
        // GET: /api/stock-issues
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = await _context.StockIssues
                .AsNoTracking()
                .Include(x => x.Items)
                .OrderByDescending(x => x.IssueId)
                .ToListAsync();

            var result = issues.Select(MapToResponse).ToList();

            return Ok(result);
        }

        // =====================================================
        // GET: /api/stock-issues/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var issue = await _context.StockIssues
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.IssueId == id);

            if (issue == null)
            {
                return NotFound(new
                {
                    message = "Stock Issue not found."
                });
            }

            return Ok(MapToResponse(issue));
        }

        // =====================================================
        // POST: /api/stock-issues
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockIssueDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (dto.Items == null || dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Stock Issue must contain at least one item."
                });
            }

            // =================================================
            // Validate Request
            // =================================================

            var request = await _context.StockRequests
                .FirstOrDefaultAsync(x =>
                    x.RequestId == dto.RequestId);

            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Stock Request not found."
                });
            }

            // =================================================
            // Validate Warehouse
            // =================================================

            var warehouseExists = await _context.Warehouses
                .AnyAsync(x =>
                    x.WarehouseId == dto.WarehouseId);

            if (!warehouseExists)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }

            // =================================================
            // Validate Department
            // =================================================

            var departmentExists = await _context.Departments
                .AnyAsync(x =>
                    x.DepartmentId == dto.DepartmentId);

            if (!departmentExists)
            {
                return BadRequest(new
                {
                    message = "Department not found."
                });
            }

            // =================================================
            // Validate User
            // =================================================

            var userExists = await _context.Users
                .AnyAsync(x =>
                    x.UserId == dto.IssuedBy);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message = "IssuedBy user not found."
                });
            }

            // =================================================
            // Validate Items
            // =================================================

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Quantity must be greater than zero for ProductId {item.ProductId}."
                    });
                }

                var stock = await _context.Stocks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.StockId == item.StockId &&
                        x.ProductId == item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {item.StockId} does not belong to Product {item.ProductId}, or stock was not found."
                    });
                }
            }

            // =================================================
            // Generate Issue Number
            // =================================================

            var issueNumber =
                $"ISS-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            // =================================================
            // Create Stock Issue
            // =================================================

            var issue = new StockIssue
            {
                IssueNumber = issueNumber,

                RequestId = dto.RequestId,

                PickListId = dto.PickListId,

                WarehouseId = dto.WarehouseId,

                DepartmentId = dto.DepartmentId,

                IssuedBy = dto.IssuedBy,

                IssuedAt = DateTimeOffset.UtcNow,

                StockIssueStatus = StockIssueStatus.Pending
            };

            // =================================================
            // Create Issue Items
            // =================================================

            foreach (var item in dto.Items)
            {
                issue.Items.Add(new StockIssueItem
                {
                    StockId = item.StockId,

                    ProductId = item.ProductId,

                    Quantity = item.Quantity
                });
            }

            // =================================================
            // Save
            // =================================================

            await _context.StockIssues.AddAsync(issue);

            await _unitOfWork.SaveAsync();

            // =================================================
            // Response
            // =================================================

            return CreatedAtAction(
                nameof(GetById),
                new { id = issue.IssueId },
                MapToResponse(issue));
        }

        // =====================================================
        // POST: /api/stock-issues/{id}/verify
        // =====================================================

        [HttpPost("{id:int}/verify")]
        public async Task<IActionResult> Verify(
            int id,
            [FromBody] StockIssueActionDTO dto)
        {
            var issue = await _context.StockIssues
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.IssueId == id);

            if (issue == null)
            {
                return NotFound(new
                {
                    message = "Stock Issue not found."
                });
            }

            // =================================================
            // Status Validation
            // =================================================

            if (issue.StockIssueStatus !=
                StockIssueStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Issue cannot be verified because its current status is {issue.StockIssueStatus}."
                });
            }

            // =================================================
            // Check Items
            // =================================================

            if (issue.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Stock Issue has no items."
                });
            }

            // =================================================
            // Verify User
            // =================================================

            var userExists = await _context.Users
                .AnyAsync(x =>
                    x.UserId == dto.UserId);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message = "User not found."
                });
            }

            // =================================================
            // Verify Stock
            // =================================================

            foreach (var item in issue.Items)
            {
                var stock = await _context.Stocks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.StockId == item.StockId &&
                        x.ProductId == item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Stock {item.StockId} not found for Product {item.ProductId}."
                    });
                }

                var availableQuantity =
                    stock.Quantity - stock.ReservedQuantity;

                if (availableQuantity < item.Quantity)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Insufficient available stock for Product {item.ProductId}. " +
                            $"Available: {availableQuantity}, " +
                            $"Required: {item.Quantity}."
                    });
                }
            }

            // =================================================
            // Change Status
            // =================================================

            issue.StockIssueStatus =
                StockIssueStatus.Ready;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock Issue verified successfully.",

                issue = MapToResponse(issue)
            });
        }

        // =====================================================
        // POST: /api/stock-issues/{id}/issue
        // =====================================================

        [HttpPost("{id:int}/issue")]
        public async Task<IActionResult> Issue(
            int id,
            [FromBody] StockIssueActionDTO dto)
        {
            // =================================================
            // Get Issue
            // =================================================

            var issue = await _context.StockIssues
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.IssueId == id);

            if (issue == null)
            {
                return NotFound(new
                {
                    message = "Stock Issue not found."
                });
            }

            // =================================================
            // Status Validation
            // =================================================

            if (issue.StockIssueStatus !=
                StockIssueStatus.Ready)
            {
                return BadRequest(new
                {
                    message =
                        $"Issue cannot be issued because its current status is {issue.StockIssueStatus}."
                });
            }

            if (issue.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Stock Issue has no items."
                });
            }

            // =================================================
            // Validate User
            // =================================================

            var userExists = await _context.Users
                .AnyAsync(x =>
                    x.UserId == dto.UserId);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message = "User not found."
                });
            }

            // =================================================
            // START TRANSACTION
            // =================================================

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // =================================================
                // 1. ISSUE
                // =================================================

                // The issue is now being processed.
                // We don't change the final status until
                // all operations succeed.


                // =================================================
                // 2. DECREASE STOCK
                // =================================================

                foreach (var item in issue.Items)
                {
                    var stock = await _context.Stocks
                        .FirstOrDefaultAsync(x =>
                            x.StockId == item.StockId &&
                            x.ProductId == item.ProductId);

                    if (stock == null)
                    {
                        throw new InvalidOperationException(
                            $"Stock {item.StockId} not found for Product {item.ProductId}.");
                    }

                    // ---------------------------------------------
                    // Calculate available quantity
                    // ---------------------------------------------

                    var availableQuantity =
                        stock.Quantity -
                        stock.ReservedQuantity;

                    if (availableQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient available stock for StockId {item.StockId}. " +
                            $"Available: {availableQuantity}, " +
                            $"Required: {item.Quantity}.");
                    }

                    // ---------------------------------------------
                    // Decrease Stock
                    // ---------------------------------------------

                    stock.Quantity -= item.Quantity;


                    // =================================================
                    // 3. DECREASE RESERVATION
                    // =================================================

                    if (stock.ReservedQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Reserved quantity is insufficient for StockId {item.StockId}.");
                    }

                    stock.ReservedQuantity -= item.Quantity;
                }


                // =================================================
                // 4. UPDATE REQUEST
                // =================================================

                var request = await _context.StockRequests
                    .FirstOrDefaultAsync(x =>
                        x.RequestId == issue.RequestId);

                if (request == null)
                {
                    throw new InvalidOperationException(
                        $"Stock Request {issue.RequestId} not found.");
                }

                /*
                 * IMPORTANT:
                 *
                 * I am NOT changing request status here yet.
                 *
                 * We need to use the exact properties/status
                 * from your StockRequest model.
                 *
                 * Example:
                 *
                 * request.Status = StockRequestStatus.Completed;
                 */


                // =================================================
                // 5. CREATE STOCK TRANSACTION
                // =================================================

                /*
                 * IMPORTANT:
                 *
                 * Your StockTransaction model is required here.
                 *
                 * We should create one transaction for every
                 * StockIssueItem.
                 *
                 * Example only:
                 *
                 * var stockTransaction = new StockTransaction
                 * {
                 *     StockId = item.StockId,
                 *     ProductId = item.ProductId,
                 *     Quantity = item.Quantity,
                 *     TransactionType = ...,
                 *     ReferenceId = issue.IssueId
                 * };
                 *
                 * But I don't want to invent your actual
                 * StockTransaction properties.
                 */


                // =================================================
                // 6. CREATE AUDIT LOG
                // =================================================

                /*
                 * Same here.
                 *
                 * We need your actual AuditLog model before
                 * creating the record.
                 */


                // =================================================
                // UPDATE ISSUE
                // =================================================

                issue.StockIssueStatus =
                    StockIssueStatus.Issued;

                issue.IssuedBy =
                    dto.UserId;

                issue.IssuedAt =
                    DateTimeOffset.UtcNow;


                // =================================================
                // SAVE EVERYTHING
                // =================================================

                await _context.SaveChangesAsync();


                // =================================================
                // COMMIT
                // =================================================

                await transaction.CommitAsync();


                return Ok(new
                {
                    message =
                        "Stock Issue issued successfully.",

                    issue = MapToResponse(issue)
                });
            }
            catch (Exception ex)
            {
                // =================================================
                // ROLLBACK EVERYTHING
                // =================================================

                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message = "Stock Issue failed.",

                    error = ex.Message
                });
            }
        }

        // =====================================================
        // POST: /api/stock-issues/{id}/cancel
        // =====================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(
            int id,
            [FromBody] StockIssueActionDTO dto)
        {
            var issue = await _context.StockIssues
                .FirstOrDefaultAsync(x =>
                    x.IssueId == id);

            if (issue == null)
            {
                return NotFound(new
                {
                    message = "Stock Issue not found."
                });
            }

            // =================================================
            // Cannot cancel issued issue
            // =================================================

            if (issue.StockIssueStatus ==
                StockIssueStatus.Issued)
            {
                return BadRequest(new
                {
                    message =
                        "Issued Stock Issue cannot be cancelled."
                });
            }

            // =================================================
            // Already cancelled
            // =================================================

            if (issue.StockIssueStatus ==
                StockIssueStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Issue is already cancelled."
                });
            }

            // =================================================
            // Validate User
            // =================================================

            var userExists = await _context.Users
                .AnyAsync(x =>
                    x.UserId == dto.UserId);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message = "User not found."
                });
            }

            // =================================================
            // Cancel
            // =================================================

            issue.StockIssueStatus =
                StockIssueStatus.Cancelled;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Stock Issue cancelled successfully.",

                issue = MapToResponse(issue)
            });
        }

        // =====================================================
        // MAPPER
        // =====================================================

        private static StockIssueResponseDTO MapToResponse(
            StockIssue issue)
        {
            return new StockIssueResponseDTO
            {
                IssueId = issue.IssueId,

                IssueNumber = issue.IssueNumber,

                RequestId = issue.RequestId,

                PickListId = issue.PickListId,

                WarehouseId = issue.WarehouseId,

                DepartmentId = issue.DepartmentId,

                IssuedBy = issue.IssuedBy,

                IssuedAt = issue.IssuedAt,

                StockIssueStatus =
                    issue.StockIssueStatus,

                Items = issue.Items
                    .Select(item =>
                        new StockIssueItemResponseDTO
                        {
                            IssueItemId =
                                item.IssueItemId,

                            StockId =
                                item.StockId,

                            ProductId =
                                item.ProductId,

                            Quantity =
                                item.Quantity
                        })
                    .ToList()
            };
        }
    }
}