using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Receipt;
using whm.Models;
using whm.Repositories;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ReceiptsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =========================================================
        // GET: api/receipts
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var receipts =
                await unitOfWork.ReceiptRepository.GetAllAsync();

            var result = receipts.Select(r => new ReceiptDto
            {
                ReceiptId = r.ReceiptId,
                ReceiptNumber = r.ReceiptNumber,
                PurchaseOrderId = r.PurchaseOrderId,
                WarehouseId = r.WarehouseId,
                ReceivedBy = r.ReceivedBy,
                ReceivedAt = r.ReceivedAt,
                Notes = r.Notes,
                ReceiptStatus = r.receiptStatus.ToString(),

                Items = r.Items.Select(i => new ReceiptItemDto
                {
                    ReceiptItemId = i.ReceiptItemId,
                    ReceiptId = i.ReceiptId,
                    PurchaseOrderItemId = i.PurchaseOrderItemId,
                    ProductId = i.ProductId,
                    ReceivedQuantity = i.ReceivedQuantity,
                    AcceptedQuantity = i.AcceptedQuantity,
                    QuarantineQuantity = i.QuarantineQuantity,
                    RejectedQuantity = i.RejectedQuantity,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // =========================================================
        // GET: api/receipts/{id}
        // =========================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdWithItemsAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            var result = new ReceiptDto
            {
                ReceiptId = receipt.ReceiptId,
                ReceiptNumber = receipt.ReceiptNumber,
                PurchaseOrderId = receipt.PurchaseOrderId,
                WarehouseId = receipt.WarehouseId,
                ReceivedBy = receipt.ReceivedBy,
                ReceivedAt = receipt.ReceivedAt,
                Notes = receipt.Notes,
                ReceiptStatus = receipt.receiptStatus.ToString(),

                Items = receipt.Items.Select(i => new ReceiptItemDto
                {
                    ReceiptItemId = i.ReceiptItemId,
                    ReceiptId = i.ReceiptId,
                    PurchaseOrderItemId = i.PurchaseOrderItemId,
                    ProductId = i.ProductId,
                    ReceivedQuantity = i.ReceivedQuantity,
                    AcceptedQuantity = i.AcceptedQuantity,
                    QuarantineQuantity = i.QuarantineQuantity,
                    RejectedQuantity = i.RejectedQuantity,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate
                }).ToList()
            };

            return Ok(result);
        }

        // =========================================================
        // POST: api/receipts
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateReceiptDto dto)
        {
            // Validate Purchase Order
            var purchaseOrder =
                await unitOfWork.PurchaseOrders
                    .GetByIdAsync(dto.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                return BadRequest(new
                {
                    message = "Purchase order not found."
                });
            }

            // Validate Warehouse
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(dto.WarehouseId);

            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }

            // Validate Receiver/User
            var receiver =
                await unitOfWork.User
                    .GetByIdAsync(dto.ReceivedBy);

            if (receiver == null)
            {
                return BadRequest(new
                {
                    message = "Receiving user not found."
                });
            }

            var receipt = new Receipt
            {
                ReceiptNumber =
                    $"REC-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                PurchaseOrderId =
                    dto.PurchaseOrderId,

                WarehouseId =
                    dto.WarehouseId,

                ReceivedBy =
                    dto.ReceivedBy,

                ReceivedAt =
                    dto.ReceivedAt == default
                        ? DateTimeOffset.UtcNow
                        : dto.ReceivedAt,

                Notes =
                    dto.Notes,

                receiptStatus =
                    ReceiptStatus.Pending
            };

            await unitOfWork.ReceiptRepository
                .AddAsync(receipt);

            await unitOfWork.SaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = receipt.ReceiptId },
                new
                {
                    receipt.ReceiptId,
                    receipt.ReceiptNumber,
                    receipt.PurchaseOrderId,
                    receipt.WarehouseId,
                    receipt.ReceivedBy,
                    receipt.ReceivedAt,
                    receipt.Notes,
                    Status = receipt.receiptStatus.ToString()
                });
        }

        // =========================================================
        // PUT: api/receipts/{id}
        // =========================================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateReceiptDto dto)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdWithItemsAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed receipt cannot be updated."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cancelled receipt cannot be updated."
                });
            }

            // Validate Warehouse
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(dto.WarehouseId);

            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }

            // Validate User
            var receiver =
                await unitOfWork.User
                    .GetByIdAsync(dto.ReceivedBy);

            if (receiver == null)
            {
                return BadRequest(new
                {
                    message = "Receiving user not found."
                });
            }

            receipt.WarehouseId =
                dto.WarehouseId;

            receipt.ReceivedBy =
                dto.ReceivedBy;

            receipt.ReceivedAt =
                dto.ReceivedAt;

            receipt.Notes =
                dto.Notes;

            unitOfWork.ReceiptRepository
                .Update(receipt);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Receipt updated successfully."
            });
        }

        // =========================================================
        // GET: api/receipts/{id}/items
        // =========================================================
        [HttpGet("{id:int}/items")]
        public async Task<IActionResult> GetItems(int id)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            var items =
                await unitOfWork.ReceiptRepository
                    .GetItemsAsync(id);

            var result = items.Select(i => new ReceiptItemDto
            {
                ReceiptItemId =
                    i.ReceiptItemId,

                ReceiptId =
                    i.ReceiptId,

                PurchaseOrderItemId =
                    i.PurchaseOrderItemId,

                ProductId =
                    i.ProductId,

                ReceivedQuantity =
                    i.ReceivedQuantity,

                AcceptedQuantity =
                    i.AcceptedQuantity,

                QuarantineQuantity =
                    i.QuarantineQuantity,

                RejectedQuantity =
                    i.RejectedQuantity,

                BatchNumber =
                    i.BatchNumber,

                ExpiryDate =
                    i.ExpiryDate
            }).ToList();

            return Ok(result);
        }

        // =========================================================
        // POST: api/receipts/{id}/items
        // =========================================================
        [HttpPost("{id:int}/items")]
        public async Task<IActionResult> AddItem(
            int id,
            [FromBody] CreateReceiptItemDto dto)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot add items to a completed receipt."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot add items to a cancelled receipt."
                });
            }

            if (dto.ReceivedQuantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Received quantity must be greater than zero."
                });
            }

            if (dto.AcceptedQuantity < 0 ||
                dto.QuarantineQuantity < 0 ||
                dto.RejectedQuantity < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted, quarantine and rejected quantities cannot be negative."
                });
            }

            var totalQuantity =
                dto.AcceptedQuantity +
                dto.QuarantineQuantity +
                dto.RejectedQuantity;

            if (totalQuantity != dto.ReceivedQuantity)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted + Quarantine + Rejected quantities must equal Received quantity."
                });
            }

            // Validate Product
            var product =
                await unitOfWork.Products
                    .GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                return BadRequest(new
                {
                    message = "Product not found."
                });
            }

            // Validate PO Item
            var poItem =
                await unitOfWork.PurchaseOrders
                    .GetByIdAsync(dto.PurchaseOrderItemId);

            if (poItem == null)
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order item not found."
                });
            }

            // Make sure PO item belongs to receipt's PO
            if (poItem.PurchaseOrderId !=
                receipt.PurchaseOrderId)
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order item does not belong to this receipt's purchase order."
                });
            }

            // Make sure Product belongs to PO item
            

            var item = new ReceiptItem
            {
                ReceiptId =
                    id,

                PurchaseOrderItemId =
                    dto.PurchaseOrderItemId,

                ProductId =
                    dto.ProductId,

                ReceivedQuantity =
                    dto.ReceivedQuantity,

                AcceptedQuantity =
                    dto.AcceptedQuantity,

                QuarantineQuantity =
                    dto.QuarantineQuantity,

                RejectedQuantity =
                    dto.RejectedQuantity,

                BatchNumber =
                    dto.BatchNumber,

                ExpiryDate =
                    dto.ExpiryDate
            };

            await unitOfWork.ReceiptRepository
                .AddItemAsync(item);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt item added successfully.",

                item.ReceiptItemId
            });
        }

        // =========================================================
        // PUT: api/receipts/{id}/items/{itemId}
        // =========================================================
        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateItem(
            int id,
            int itemId,
            [FromBody] UpdateReceiptItemDto dto)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed receipt cannot be updated."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cancelled receipt cannot be updated."
                });
            }

            var item =
                await unitOfWork.ReceiptRepository
                    .GetItemByIdAsync(id, itemId);

            if (item == null)
            {
                return NotFound(new
                {
                    message = "Receipt item not found."
                });
            }

            if (dto.ReceivedQuantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Received quantity must be greater than zero."
                });
            }

            if (dto.AcceptedQuantity < 0 ||
                dto.QuarantineQuantity < 0 ||
                dto.RejectedQuantity < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted, quarantine and rejected quantities cannot be negative."
                });
            }

            var totalQuantity =
                dto.AcceptedQuantity +
                dto.QuarantineQuantity +
                dto.RejectedQuantity;

            if (totalQuantity != dto.ReceivedQuantity)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted + Quarantine + Rejected quantities must equal Received quantity."
                });
            }

            item.ReceivedQuantity =
                dto.ReceivedQuantity;

            item.AcceptedQuantity =
                dto.AcceptedQuantity;

            item.QuarantineQuantity =
                dto.QuarantineQuantity;

            item.RejectedQuantity =
                dto.RejectedQuantity;

            item.BatchNumber =
                dto.BatchNumber;

            item.ExpiryDate =
                dto.ExpiryDate;

            unitOfWork.ReceiptRepository
                .UpdateItem(item);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt item updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/receipts/{id}/items/{itemId}
        // =========================================================
        [HttpDelete("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(
            int id,
            int itemId)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed receipt cannot be modified."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cancelled receipt cannot be modified."
                });
            }

            var item =
                await unitOfWork.ReceiptRepository
                    .GetItemByIdAsync(id, itemId);

            if (item == null)
            {
                return NotFound(new
                {
                    message =
                        "Receipt item not found."
                });
            }

            unitOfWork.ReceiptRepository
                .DeleteItem(item);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt item deleted successfully."
            });
        }

        // =========================================================
        // POST: api/receipts/{id}/start
        // =========================================================
        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdWithItemsAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus != ReceiptStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Receipt cannot be started because its current status is {receipt.receiptStatus}."
                });
            }

            if (!receipt.Items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Cannot start a receipt without items."
                });
            }

            receipt.receiptStatus =
                ReceiptStatus.InProgress;

            unitOfWork.ReceiptRepository
                .Update(receipt);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt started successfully.",

                receiptId =
                    receipt.ReceiptId,

                status =
                    receipt.receiptStatus.ToString()
            });
        }

        // =========================================================
        // POST: api/receipts/{id}/complete
        // =========================================================
        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdWithItemsAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus != ReceiptStatus.InProgress)
            {
                return BadRequest(new
                {
                    message =
                        "Only receipts in progress can be completed."
                });
            }

            if (!receipt.Items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Cannot complete a receipt without items."
                });
            }

            foreach (var item in receipt.Items)
            {
                if (item.ReceivedQuantity <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Invalid received quantity for receipt item {item.ReceiptItemId}."
                    });
                }

                if (item.AcceptedQuantity < 0 ||
                    item.QuarantineQuantity < 0 ||
                    item.RejectedQuantity < 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Invalid quantities for receipt item {item.ReceiptItemId}."
                    });
                }

                var total =
                    item.AcceptedQuantity +
                    item.QuarantineQuantity +
                    item.RejectedQuantity;

                if (total != item.ReceivedQuantity)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Accepted + Quarantine + Rejected quantities must equal Received quantity for item {item.ReceiptItemId}."
                    });
                }
            }

            receipt.receiptStatus =
                ReceiptStatus.Completed;

            unitOfWork.ReceiptRepository
                .Update(receipt);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt completed successfully.",

                receiptId =
                    receipt.ReceiptId,

                status =
                    receipt.receiptStatus.ToString()
            });
        }

        // =========================================================
        // POST: api/receipts/{id}/cancel
        // =========================================================
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var receipt =
                await unitOfWork.ReceiptRepository
                    .GetByIdAsync(id);

            if (receipt == null)
            {
                return NotFound(new
                {
                    message = "Receipt not found."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed receipt cannot be cancelled."
                });
            }

            if (receipt.receiptStatus == ReceiptStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Receipt is already cancelled."
                });
            }

            receipt.receiptStatus =
                ReceiptStatus.Cancelled;

            unitOfWork.ReceiptRepository
                .Update(receipt);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Receipt cancelled successfully.",

                receiptId =
                    receipt.ReceiptId,

                status =
                    receipt.receiptStatus.ToString()
            });
        }
    }
}