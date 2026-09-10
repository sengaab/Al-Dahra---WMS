using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Receipt;
using whm.Models;
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
            // -----------------------------------------------------
            // Validate Purchase Order ID
            // -----------------------------------------------------
            if (dto.PurchaseOrderId <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid PurchaseOrderId is required."
                });
            }

            // -----------------------------------------------------
            // Validate Warehouse ID
            // -----------------------------------------------------
            if (dto.WarehouseId <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid WarehouseId is required."
                });
            }

            // -----------------------------------------------------
            // Validate Purchase Order
            // -----------------------------------------------------
            var purchaseOrder =
                await unitOfWork.PurchaseOrders
                    .GetEntityByIdAsync(dto.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                return BadRequest(new
                {
                    message = "Purchase order not found."
                });
            }

            // -----------------------------------------------------
            // Validate Purchase Order Status
            // -----------------------------------------------------
            if (purchaseOrder.purchaseOrderStatus !=
                    PurchaseOrderStatus.Ordered &&
                purchaseOrder.purchaseOrderStatus !=
                    PurchaseOrderStatus.PartiallyReceived)
            {
                return BadRequest(new
                {
                    message =
                        "A receipt can only be created for an Ordered or PartiallyReceived purchase order."
                });
            }

            // -----------------------------------------------------
            // Validate Warehouse
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate Receiver if supplied
            // -----------------------------------------------------
            if (dto.ReceivedBy.HasValue)
            {
                var receiver =
                    await unitOfWork.User
                        .GetByIdAsync(dto.ReceivedBy.Value);

                if (receiver == null)
                {
                    return BadRequest(new
                    {
                        message = "Receiving user not found."
                    });
                }
            }

            // =====================================================
            // GENERATE RECEIPT NUMBER
            // Format:
            // GRN-2026-0001
            // GRN-2026-0002
            // GRN-2027-0001
            // =====================================================

            var currentYear = DateTime.UtcNow.Year;

            var prefix = $"GRN-{currentYear}-";

            // Get all existing receipts
            var existingReceipts =
                await unitOfWork.ReceiptRepository
                    .GetAllAsync();

            // Get the highest number for the current year
            var lastNumber = existingReceipts
                .Select(x => x.ReceiptNumber)
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x) &&
                    x.StartsWith(prefix))
                .Select(x =>
                {
                    var numberPart =
                        x.Substring(prefix.Length);

                    return int.TryParse(
                        numberPart,
                        out var number)
                        ? number
                        : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            // Increment number
            var nextNumber = lastNumber + 1;

            // Final Receipt Number
            var receiptNumber =
                $"{prefix}{nextNumber:D4}";

            // =====================================================
            // CREATE RECEIPT
            // =====================================================

            var receipt = new Receipt
            {
                ReceiptNumber = receiptNumber,

                PurchaseOrderId =
                    dto.PurchaseOrderId,

                WarehouseId =
                    dto.WarehouseId,

                ReceivedBy =
                    dto.ReceivedBy,

                ReceivedAt =
                    dto.ReceivedAt,

                Notes =
                    dto.Notes,

                receiptStatus =
                    ReceiptStatus.Pending
            };

            await unitOfWork.ReceiptRepository
                .AddAsync(receipt);

            await unitOfWork.SaveAsync();

            // =====================================================
            // RETURN CREATED RECEIPT
            // =====================================================

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = receipt.ReceiptId
                },
                new ReceiptDto
                {
                    ReceiptId =
                        receipt.ReceiptId,

                    ReceiptNumber =
                        receipt.ReceiptNumber,

                    PurchaseOrderId =
                        receipt.PurchaseOrderId,

                    WarehouseId =
                        receipt.WarehouseId,

                    ReceivedBy =
                        receipt.ReceivedBy,

                    ReceivedAt =
                        receipt.ReceivedAt,

                    Notes =
                        receipt.Notes,

                    ReceiptStatus =
                        receipt.receiptStatus.ToString(),

                    Items =
                        new List<ReceiptItemDto>()
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

            // -----------------------------------------------------
            // Check status
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate Warehouse
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate Receiver only if supplied
            // -----------------------------------------------------
            if (dto.ReceivedBy.HasValue)
            {
                var receiver =
                    await unitOfWork.User
                        .GetByIdAsync(dto.ReceivedBy.Value);

                if (receiver == null)
                {
                    return BadRequest(new
                    {
                        message = "Receiving user not found."
                    });
                }
            }

            // -----------------------------------------------------
            // Update
            // -----------------------------------------------------
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
                message =
                    "Receipt updated successfully."
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

            // -----------------------------------------------------
            // Check Receipt status
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate Product ID
            // -----------------------------------------------------
            if (dto.ProductId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Valid ProductId is required."
                });
            }

            // -----------------------------------------------------
            // Validate Purchase Order Item ID
            // -----------------------------------------------------
            if (dto.PurchaseOrderItemId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Valid PurchaseOrderItemId is required."
                });
            }

            // -----------------------------------------------------
            // Validate quantities
            // -----------------------------------------------------
            if (dto.ReceivedQuantity.HasValue &&
                dto.ReceivedQuantity.Value <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Received quantity must be greater than zero."
                });
            }

            if (dto.AcceptedQuantity.HasValue &&
                dto.AcceptedQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted quantity cannot be negative."
                });
            }

            if (dto.QuarantineQuantity.HasValue &&
                dto.QuarantineQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Quarantine quantity cannot be negative."
                });
            }

            if (dto.RejectedQuantity.HasValue &&
                dto.RejectedQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Rejected quantity cannot be negative."
                });
            }

            // -----------------------------------------------------
            // Validate total if all quantities supplied
            // -----------------------------------------------------
            if (dto.ReceivedQuantity.HasValue &&
                dto.AcceptedQuantity.HasValue &&
                dto.QuarantineQuantity.HasValue &&
                dto.RejectedQuantity.HasValue)
            {
                var totalQuantity =
                    dto.AcceptedQuantity.Value +
                    dto.QuarantineQuantity.Value +
                    dto.RejectedQuantity.Value;

                if (totalQuantity !=
                    dto.ReceivedQuantity.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Accepted + Quarantine + Rejected quantities must equal Received quantity."
                    });
                }
            }

            // -----------------------------------------------------
            // Validate Product
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate Purchase Order Item
            // -----------------------------------------------------
            var poItem =
                await unitOfWork.PurchaseOrders
                    .GetItemByIdAsync(
                        dto.PurchaseOrderItemId);

            if (poItem == null)
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order item not found."
                });
            }

            // -----------------------------------------------------
            // Make sure PO Item belongs to Receipt's PO
            // -----------------------------------------------------
            if (poItem.PurchaseOrderId !=
                receipt.PurchaseOrderId)
            {
                return BadRequest(new
                {
                    message =
                        "Purchase order item does not belong to this receipt's purchase order."
                });
            }

            // -----------------------------------------------------
            // Make sure Product belongs to PO Item
            // -----------------------------------------------------
            if (poItem.ProductId !=
                dto.ProductId)
            {
                return BadRequest(new
                {
                    message =
                        "The selected product does not belong to the selected purchase order item."
                });
            }

            // -----------------------------------------------------
            // Create Receipt Item
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Check Receipt status
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Get Receipt Item
            // -----------------------------------------------------
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

            // -----------------------------------------------------
            // Validate quantities
            // -----------------------------------------------------
            if (dto.ReceivedQuantity.HasValue &&
                dto.ReceivedQuantity.Value <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Received quantity must be greater than zero."
                });
            }

            if (dto.AcceptedQuantity.HasValue &&
                dto.AcceptedQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Accepted quantity cannot be negative."
                });
            }

            if (dto.QuarantineQuantity.HasValue &&
                dto.QuarantineQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Quarantine quantity cannot be negative."
                });
            }

            if (dto.RejectedQuantity.HasValue &&
                dto.RejectedQuantity.Value < 0)
            {
                return BadRequest(new
                {
                    message =
                        "Rejected quantity cannot be negative."
                });
            }

            // -----------------------------------------------------
            // Validate total if all quantities exist
            // -----------------------------------------------------
            if (dto.ReceivedQuantity.HasValue &&
                dto.AcceptedQuantity.HasValue &&
                dto.QuarantineQuantity.HasValue &&
                dto.RejectedQuantity.HasValue)
            {
                var totalQuantity =
                    dto.AcceptedQuantity.Value +
                    dto.QuarantineQuantity.Value +
                    dto.RejectedQuantity.Value;

                if (totalQuantity !=
                    dto.ReceivedQuantity.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Accepted + Quarantine + Rejected quantities must equal Received quantity."
                    });
                }
            }

            // -----------------------------------------------------
            // Update fields
            // -----------------------------------------------------
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

            if (receipt.receiptStatus !=
                ReceiptStatus.Pending)
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

            if (receipt.receiptStatus !=
                ReceiptStatus.InProgress)
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
                // -------------------------------------------------
                // Received Quantity
                // -------------------------------------------------
                if (!item.ReceivedQuantity.HasValue ||
                    item.ReceivedQuantity.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Received quantity is required and must be greater than zero for receipt item {item.ReceiptItemId}."
                    });
                }

                // -------------------------------------------------
                // Check negative values
                // -------------------------------------------------
                if (item.AcceptedQuantity.HasValue &&
                    item.AcceptedQuantity.Value < 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Accepted quantity cannot be negative for receipt item {item.ReceiptItemId}."
                    });
                }

                if (item.QuarantineQuantity.HasValue &&
                    item.QuarantineQuantity.Value < 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Quarantine quantity cannot be negative for receipt item {item.ReceiptItemId}."
                    });
                }

                if (item.RejectedQuantity.HasValue &&
                    item.RejectedQuantity.Value < 0)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Rejected quantity cannot be negative for receipt item {item.ReceiptItemId}."
                    });
                }

                // -------------------------------------------------
                // Validate total
                // -------------------------------------------------
                if (item.AcceptedQuantity.HasValue &&
                    item.QuarantineQuantity.HasValue &&
                    item.RejectedQuantity.HasValue)
                {
                    var total =
                        item.AcceptedQuantity.Value +
                        item.QuarantineQuantity.Value +
                        item.RejectedQuantity.Value;

                    if (total !=
                        item.ReceivedQuantity.Value)
                    {
                        return BadRequest(new
                        {
                            message =
                                $"Accepted + Quarantine + Rejected quantities must equal Received quantity for item {item.ReceiptItemId}."
                        });
                    }
                }
            }

            // -----------------------------------------------------
            // Complete Receipt
            // -----------------------------------------------------
            receipt.receiptStatus =
                ReceiptStatus.Completed;

            // If ReceivedAt wasn't supplied before,
            // automatically set it when completing.
            if (!receipt.ReceivedAt.HasValue)
            {
                receipt.ReceivedAt =
                    DateTimeOffset.UtcNow;
            }

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
                    receipt.receiptStatus.ToString(),

                receivedAt =
                    receipt.ReceivedAt
            });
        }
        
// =========================================================
// POST: api/receipts/{id}/partially-received
// =========================================================
[HttpPost("{id:int}/partially-received")]
public async Task<IActionResult> MarkAsPartiallyReceived(int id)
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

            // ---------------------------------------------------------
            // Check current status
            // ---------------------------------------------------------
            if (receipt.receiptStatus != ReceiptStatus.InProgress)
            {
                return BadRequest(new
                {
                    message =
                        $"Receipt cannot be marked as PartiallyReceived because its current status is {receipt.receiptStatus}."
                });
            }

            // ---------------------------------------------------------
            // Check receipt items
            // ---------------------------------------------------------
            if (!receipt.Items.Any())
            {
                return BadRequest(new
                {
                    message = "Cannot mark a receipt as PartiallyReceived without items."
                });
            }

            // ---------------------------------------------------------
            // Check received quantities
            // ---------------------------------------------------------
            var hasReceivedQuantity = receipt.Items.Any(i =>
                i.ReceivedQuantity.HasValue &&
                i.ReceivedQuantity.Value > 0);

            if (!hasReceivedQuantity)
            {
                return BadRequest(new
                {
                    message =
                        "At least one receipt item must have a received quantity greater than zero."
                });
            }

            // ---------------------------------------------------------
            // Mark as Partially Received
            // ---------------------------------------------------------
            receipt.receiptStatus = ReceiptStatus.PartiallyReceived;

            unitOfWork.ReceiptRepository
                .Update(receipt);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Receipt marked as partially received successfully.",

                receiptId = receipt.ReceiptId,

                status = receipt.receiptStatus.ToString()
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

            if (receipt.receiptStatus ==
                ReceiptStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed receipt cannot be cancelled."
                });
            }

            if (receipt.receiptStatus ==
                ReceiptStatus.Cancelled)
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