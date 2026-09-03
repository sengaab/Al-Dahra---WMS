using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.PurchaseOrder;
using whm.Models;
using whm.Repositories;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/purchase-orders")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================================================
        // GET ALL
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] int? siteId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var orders =
                await _unitOfWork.PurchaseOrders.GetAllAsync(
                    search,
                    status,
                    supplierId,
                    siteId,
                    page,
                    pageSize);

            return Ok(orders);
        }

        // =========================================================
        // GET BY ID
        // =========================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders.GetByIdAsync(id);

            if (order == null)
                return NotFound(new
                {
                    message = "Purchase order not found."
                });

            return Ok(order);
        }

        // =========================================================
        // CREATE
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePurchaseOrderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PONumber))
            {
                return BadRequest(new
                {
                    message = "PONumber is required."
                });
            }

            if (dto.SupplierId <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid SupplierId is required."
                });
            }

            if (dto.SiteId <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid SiteId is required."
                });
            }

            var poNumber = dto.PONumber.Trim();

            var exists =
                await _unitOfWork.PurchaseOrders
                    .PONumberExistsAsync(poNumber);

            if (exists)
            {
                return Conflict(new
                {
                    message = "Purchase order number already exists."
                });
            }

            var supplier =
                await _unitOfWork.Suppliers
                    .GetEntityByIdAsync(dto.SupplierId);

            if (supplier == null)
            {
                return BadRequest(new
                {
                    message = "Supplier not found."
                });
            }

            var site =
                await _unitOfWork.Sites
                    .GetEntityByIdAsync(dto.SiteId);

            if (site == null)
            {
                return BadRequest(new
                {
                    message = "Site not found."
                });
            }

            var createdByClaim =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier);

            if (createdByClaim == null)
            {
                return Unauthorized(new
                {
                    message = "User identity was not found."
                });
            }

            if (!Guid.TryParse(
                    createdByClaim.Value,
                    out var createdBy))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var now = DateTimeOffset.UtcNow;

            var order = new PurchaseOrder
            {
                PONumber = poNumber,

                SupplierId = dto.SupplierId,
                SiteId = dto.SiteId,

                OrderDate =
                    dto.OrderDate ?? now,

                ExpectedDate =
                    dto.ExpectedDate,

                purchaseOrderStatus =
                    PurchaseOrderStatus.Draft,

                TotalValue = 0,

                CreatedBy = createdBy,

                CreatedAt = now
            };

            await _unitOfWork.PurchaseOrders
                .AddAsync(order);

            await _unitOfWork.SaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.PurchaseOrderId },
                await _unitOfWork.PurchaseOrders
                    .GetByIdAsync(order.PurchaseOrderId));
        }

        // =========================================================
        // UPDATE
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePurchaseOrderDto dto)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Purchase order not found."
                });
            }

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft purchase orders can be updated."
                });
            }

            if (dto.PONumber != null)
            {
                var poNumber = dto.PONumber.Trim();

                if (string.IsNullOrWhiteSpace(poNumber))
                {
                    return BadRequest(new
                    {
                        message = "PONumber cannot be empty."
                    });
                }

                var exists =
                    await _unitOfWork.PurchaseOrders
                        .PONumberExistsAsync(
                            poNumber,
                            id);

                if (exists)
                {
                    return Conflict(new
                    {
                        message =
                            "Purchase order number already exists."
                    });
                }

                order.PONumber = poNumber;
            }

            if (dto.SupplierId.HasValue)
            {
                if (dto.SupplierId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid SupplierId."
                    });
                }

                var supplier =
                    await _unitOfWork.Suppliers
                        .GetEntityByIdAsync(
                            dto.SupplierId.Value);

                if (supplier == null)
                {
                    return BadRequest(new
                    {
                        message = "Supplier not found."
                    });
                }

                order.SupplierId =
                    dto.SupplierId.Value;
            }

            if (dto.SiteId.HasValue)
            {
                if (dto.SiteId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid SiteId."
                    });
                }

                var site =
                    await _unitOfWork.Sites
                        .GetEntityByIdAsync(
                            dto.SiteId.Value);

                if (site == null)
                {
                    return BadRequest(new
                    {
                        message = "Site not found."
                    });
                }

                order.SiteId =
                    dto.SiteId.Value;
            }

            if (dto.OrderDate.HasValue)
                order.OrderDate =
                    dto.OrderDate.Value;

            if (dto.ExpectedDate.HasValue)
                order.ExpectedDate =
                    dto.ExpectedDate.Value;

            await RecalculateTotalAsync(order);

            order.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.PurchaseOrders.Update(order);

            await _unitOfWork.SaveAsync();

            return Ok(
                await _unitOfWork.PurchaseOrders
                    .GetByIdAsync(id));
        }

        // =========================================================
        // DELETE
        // =========================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Purchase order not found."
                });
            }

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft purchase orders can be deleted."
                });
            }

            var items =
                await _unitOfWork.PurchaseOrders
                    .GetItemsAsync(id);

            if (items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order cannot be deleted while it has items."
                });
            }

            _unitOfWork.PurchaseOrders.Delete(order);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Purchase order deleted successfully."
            });
        }

        // =========================================================
        // SUBMIT
        // =========================================================

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft purchase orders can be submitted."
                });
            }

            var items =
                await _unitOfWork.PurchaseOrders
                    .GetItemsAsync(id);

            if (!items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order must contain at least one item."
                });
            }

            order.purchaseOrderStatus =
                PurchaseOrderStatus.PendingApproval;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Purchase order submitted successfully.",
                status =
                    order.purchaseOrderStatus.ToString()
            });
        }

        // =========================================================
        // APPROVE
        // =========================================================

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        "Only pending approval orders can be approved."
                });
            }

            var userClaim =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userClaim == null ||
                !Guid.TryParse(
                    userClaim.Value,
                    out var approvedBy))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            order.purchaseOrderStatus =
                PurchaseOrderStatus.Approved;

            order.ApprovedBy =
                approvedBy;

            order.ApprovedAt =
                DateTimeOffset.UtcNow;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Purchase order approved successfully.",
                status =
                    order.purchaseOrderStatus.ToString()
            });
        }

        // =========================================================
        // REJECT
        // =========================================================

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        "Only pending approval orders can be rejected."
                });
            }

            order.purchaseOrderStatus =
                PurchaseOrderStatus.Rejected;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Purchase order rejected successfully.",
                status =
                    order.purchaseOrderStatus.ToString()
            });
        }

        // =========================================================
        // CANCEL
        // =========================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus ==
                    PurchaseOrderStatus.Received ||
                order.purchaseOrderStatus ==
                    PurchaseOrderStatus.Closed ||
                order.purchaseOrderStatus ==
                    PurchaseOrderStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "This purchase order cannot be cancelled."
                });
            }

            order.purchaseOrderStatus =
                PurchaseOrderStatus.Cancelled;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Purchase order cancelled successfully.",
                status =
                    order.purchaseOrderStatus.ToString()
            });
        }

        // =========================================================
        // GET ITEMS
        // =========================================================

        [HttpGet("{id:int}/items")]
        public async Task<IActionResult> GetItems(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            var items =
                await _unitOfWork.PurchaseOrders
                    .GetItemsAsync(id);

            return Ok(items);
        }

        // =========================================================
        // ADD ITEM
        // =========================================================

        [HttpPost("{id:int}/items")]
        public async Task<IActionResult> AddItem(
            int id,
            [FromBody] CreatePurchaseOrderItemDto dto)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be added to draft orders."
                });
            }

            if (dto.ProductId <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid ProductId is required."
                });
            }

            if (dto.OrderedQuantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "OrderedQuantity must be greater than zero."
                });
            }

            if (dto.UnitPrice < 0)
            {
                return BadRequest(new
                {
                    message =
                        "UnitPrice cannot be negative."
                });
            }

            var product =
                await _unitOfWork.Products
                    .GetEntityByIdAsync(dto.ProductId);

            if (product == null)
            {
                return BadRequest(new
                {
                    message = "Product not found."
                });
            }

            var item = new PurchaseOrderItem
            {
                PurchaseOrderId = id,

                ProductId = dto.ProductId,

                OrderedQuantity =
                    dto.OrderedQuantity,

                ReceivedQuantity = 0,

                RemainingQuantity =
                    dto.OrderedQuantity,

                UnitPrice =
                    dto.UnitPrice,

                TotalPrice =
                    dto.OrderedQuantity *
                    dto.UnitPrice
            };

            await _unitOfWork.PurchaseOrders
                .AddItemAsync(item);

            order.TotalValue += item.TotalPrice;
            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(
                await _unitOfWork.PurchaseOrders
                    .GetItemByIdAsync(
                        item.PurchaseOrderItemId));
        }

        // =========================================================
        // UPDATE ITEM
        // =========================================================

        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateItem(
            int id,
            int itemId,
            [FromBody] UpdatePurchaseOrderItemDto dto)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be updated in draft orders."
                });
            }

            var item =
                await _unitOfWork.PurchaseOrders
                    .GetItemEntityByIdAsync(itemId);

            if (item == null ||
                item.PurchaseOrderId != id)
            {
                return NotFound(new
                {
                    message =
                        "Purchase order item not found."
                });
            }

            if (dto.ProductId.HasValue)
            {
                if (dto.ProductId.Value <= 0)
                    return BadRequest();

                var product =
                    await _unitOfWork.Products
                        .GetEntityByIdAsync(
                            dto.ProductId.Value);

                if (product == null)
                {
                    return BadRequest(new
                    {
                        message = "Product not found."
                    });
                }

                item.ProductId =
                    dto.ProductId.Value;
            }

            if (dto.OrderedQuantity.HasValue)
            {
                if (dto.OrderedQuantity.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "OrderedQuantity must be greater than zero."
                    });
                }

                if (dto.OrderedQuantity.Value <
                    item.ReceivedQuantity)
                {
                    return BadRequest(new
                    {
                        message =
                            "OrderedQuantity cannot be less than ReceivedQuantity."
                    });
                }

                item.OrderedQuantity =
                    dto.OrderedQuantity.Value;
            }

            if (dto.UnitPrice.HasValue)
            {
                if (dto.UnitPrice.Value < 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "UnitPrice cannot be negative."
                    });
                }

                item.UnitPrice =
                    dto.UnitPrice.Value;
            }

            item.RemainingQuantity =
                item.OrderedQuantity -
                item.ReceivedQuantity;

            item.TotalPrice =
                item.OrderedQuantity *
                item.UnitPrice;

            order.TotalValue =
                await CalculateTotalAsync(id);

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            _unitOfWork.PurchaseOrders
                .UpdateItem(item);

            await _unitOfWork.SaveAsync();

            return Ok(
                await _unitOfWork.PurchaseOrders
                    .GetItemByIdAsync(itemId));
        }

        // =========================================================
        // DELETE ITEM
        // =========================================================

        [HttpDelete("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(
            int id,
            int itemId)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            if (order.purchaseOrderStatus !=
                PurchaseOrderStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be deleted from draft orders."
                });
            }

            var item =
                await _unitOfWork.PurchaseOrders
                    .GetItemEntityByIdAsync(itemId);

            if (item == null ||
                item.PurchaseOrderId != id)
            {
                return NotFound();
            }

            _unitOfWork.PurchaseOrders
                .DeleteItem(item);

            order.TotalValue -= item.TotalPrice;

            if (order.TotalValue < 0)
                order.TotalValue = 0;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Purchase order item deleted successfully."
            });
        }

        // =========================================================
        // RECEIPTS
        // =========================================================

        [HttpGet("{id:int}/receipts")]
        public async Task<IActionResult> GetReceipts(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            var receipts =
                await _unitOfWork.PurchaseOrders
                    .GetReceiptsAsync(id);

            return Ok(receipts);
        }

        // =========================================================
        // HISTORY
        // =========================================================

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var order =
                await _unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(id);

            if (order == null)
                return NotFound();

            var history =
                await _unitOfWork.PurchaseOrders
                    .GetHistoryAsync(id);

            return Ok(history);
        }

        // =========================================================
        // HELPERS
        // =========================================================

        private async Task<decimal> CalculateTotalAsync(
            int purchaseOrderId)
        {
            var items =
                await _unitOfWork.PurchaseOrders
                    .GetItemsAsync(purchaseOrderId);

            return items.Sum(x => x.TotalPrice);
        }

        private async Task RecalculateTotalAsync(
            PurchaseOrder order)
        {
            order.TotalValue =
                await CalculateTotalAsync(
                    order.PurchaseOrderId);
        }
    }
}