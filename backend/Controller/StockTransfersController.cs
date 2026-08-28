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
    [Route("api/stock-transfers")]
    [Authorize]
    public class StockTransfersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataBaseContext _context;

        public StockTransfersController(
            IUnitOfWork unitOfWork,
            DataBaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        // =====================================================
        // GET /api/stock-transfers
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transfers =
                await _unitOfWork.StockTransfers.GetAllAsync();

            var result =
                transfers.Select(MapToResponse).ToList();

            return Ok(result);
        }


        // =====================================================
        // GET /api/stock-transfers/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdAsync(id);

            if (transfer == null)
            {
                return NotFound(new
                {
                    message = "Stock Transfer not found."
                });
            }

            return Ok(MapToResponse(transfer));
        }


        // =====================================================
        // POST /api/stock-transfers
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockTransferDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Stock Transfer must contain at least one item."
                });
            }


            // =================================================
            // Source Warehouse
            // =================================================

            var sourceWarehouseExists =
                await _context.Warehouses
                    .AnyAsync(x =>
                        x.WarehouseId ==
                        dto.SourceWarehouseId);

            if (!sourceWarehouseExists)
            {
                return BadRequest(new
                {
                    message =
                        "Source warehouse not found."
                });
            }


            // =================================================
            // Destination Warehouse
            // =================================================

            var destinationWarehouseExists =
                await _context.Warehouses
                    .AnyAsync(x =>
                        x.WarehouseId ==
                        dto.DestinationWarehouseId);

            if (!destinationWarehouseExists)
            {
                return BadRequest(new
                {
                    message =
                        "Destination warehouse not found."
                });
            }


            if (dto.SourceWarehouseId ==
                dto.DestinationWarehouseId)
            {
                return BadRequest(new
                {
                    message =
                        "Source and destination warehouses must be different."
                });
            }


            // =================================================
            // Requested User
            // =================================================

            var userExists =
                await _context.Users
                    .AnyAsync(x =>
                        x.UserId ==
                        dto.RequestedBy);

            if (!userExists)
            {
                return BadRequest(new
                {
                    message =
                        "RequestedBy user not found."
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


                var productExists =
                    await _context.Products
                        .AnyAsync(x =>
                            x.ProductId ==
                            item.ProductId);

                if (!productExists)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Product {item.ProductId} not found."
                    });
                }


                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            item.SourceStockId &&
                            x.ProductId ==
                            item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Source Stock {item.SourceStockId} not found for Product {item.ProductId}."
                    });
                }


                // =================================================
                // Make sure source stock belongs to source warehouse
                // =================================================

                // If your Stock model has WarehouseId directly,
                // validate it here.
                //
                // This depends on your current Stock/Location model.
            }


            // =================================================
            // Generate Transfer Number
            // =================================================

            var transferNumber =
                $"TRF-{DateTime.UtcNow:yyyyMMddHHmmssfff}";


            // =================================================
            // Create Transfer
            // =================================================

            var transfer = new StockTransfer
            {
                TransferNumber =
                    transferNumber,

                SourceWarehouseId =
                    dto.SourceWarehouseId,

                DestinationWarehouseId =
                    dto.DestinationWarehouseId,

                RequestedBy =
                    dto.RequestedBy,

                TransferStatus =
                    StockTransferStatus.Pending,

                CreatedAt =
                    DateTimeOffset.UtcNow
            };


            // =================================================
            // Create Items
            // =================================================

            foreach (var item in dto.Items)
            {
                transfer.Items.Add(
                    new StockTransferItem
                    {
                        ProductId =
                            item.ProductId,

                        SourceStockId =
                            item.SourceStockId,

                        SourceLocationId =
                            item.SourceLocationId,

                        DestinationLocationId =
                            item.DestinationLocationId,

                        Quantity =
                            item.Quantity,

                        ReceivedQuantity = 0
                    });
            }


            await _unitOfWork.StockTransfers
                .AddAsync(transfer);

            await _unitOfWork.SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new { id = transfer.TransferId },
                MapToResponse(transfer));
        }


        // =====================================================
        // PUT /api/stock-transfers/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStockTransferDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound(new
                {
                    message =
                        "Stock Transfer not found."
                });
            }


            // =================================================
            // Only Pending can be updated
            // =================================================

            if (transfer.TransferStatus !=
                StockTransferStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Transfer cannot be updated because its current status is {transfer.TransferStatus}."
                });
            }


            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Transfer must contain at least one item."
                });
            }


            if (dto.SourceWarehouseId ==
                dto.DestinationWarehouseId)
            {
                return BadRequest(new
                {
                    message =
                        "Source and destination warehouses must be different."
                });
            }


            // =================================================
            // Update Header
            // =================================================

            transfer.SourceWarehouseId =
                dto.SourceWarehouseId;

            transfer.DestinationWarehouseId =
                dto.DestinationWarehouseId;


            // =================================================
            // Replace Items
            // =================================================

            _context.StockTransferItems
                .RemoveRange(transfer.Items);


            transfer.Items.Clear();


            foreach (var item in dto.Items)
            {
                var stock =
                    await _context.Stocks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            item.SourceStockId &&
                            x.ProductId ==
                            item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Source Stock {item.SourceStockId} not found for Product {item.ProductId}."
                    });
                }


                transfer.Items.Add(
                    new StockTransferItem
                    {
                        TransferId =
                            transfer.TransferId,

                        ProductId =
                            item.ProductId,

                        SourceStockId =
                            item.SourceStockId,

                        SourceLocationId =
                            item.SourceLocationId,

                        DestinationLocationId =
                            item.DestinationLocationId,

                        Quantity =
                            item.Quantity,

                        ReceivedQuantity = 0
                    });
            }


            await _unitOfWork.SaveAsync();


            return Ok(MapToResponse(transfer));
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/submit
        // =====================================================

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus !=
                StockTransferStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot submit transfer with status {transfer.TransferStatus}."
                });
            }


            if (transfer.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Transfer must contain at least one item."
                });
            }


            transfer.TransferStatus =
                StockTransferStatus.Pending;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Transfer submitted successfully.",

                transfer =
                    MapToResponse(transfer)
            });
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/approve
        // =====================================================

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            [FromBody] StockTransferActionDTO dto)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus !=
                StockTransferStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot approve transfer with status {transfer.TransferStatus}."
                });
            }


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


            transfer.ApprovedBy =
                dto.UserId;

            transfer.TransferStatus =
                StockTransferStatus.Approved;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Transfer approved successfully.",

                transfer =
                    MapToResponse(transfer)
            });
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/start
        // =====================================================

        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus !=
                StockTransferStatus.Approved)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot start transfer with status {transfer.TransferStatus}."
                });
            }


            transfer.TransferStatus =
                StockTransferStatus.Picking;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Transfer picking started.",

                transfer =
                    MapToResponse(transfer)
            });
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/ship
        // =====================================================

        [HttpPost("{id:int}/ship")]
        public async Task<IActionResult> Ship(int id)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus !=
                StockTransferStatus.Picking)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot ship transfer with status {transfer.TransferStatus}."
                });
            }


            if (transfer.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Transfer has no items."
                });
            }


            // =================================================
            // Validate source stock
            // =================================================

            foreach (var item in transfer.Items)
            {
                var stock =
                    await _context.Stocks
                        .FirstOrDefaultAsync(x =>
                            x.StockId ==
                            item.SourceStockId &&
                            x.ProductId ==
                            item.ProductId);

                if (stock == null)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Source Stock {item.SourceStockId} not found."
                    });
                }


                var availableQuantity =
                    stock.Quantity -
                    stock.ReservedQuantity;


                if (availableQuantity <
                    item.Quantity)
                {
                    return BadRequest(new
                    {
                        message =
                            $"Insufficient available stock for StockId {item.SourceStockId}. " +
                            $"Available: {availableQuantity}, " +
                            $"Required: {item.Quantity}."
                    });
                }
            }


            // =================================================
            // Transaction
            // =================================================

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                foreach (var item in transfer.Items)
                {
                    var stock =
                        await _context.Stocks
                            .FirstAsync(x =>
                                x.StockId ==
                                item.SourceStockId &&
                                x.ProductId ==
                                item.ProductId);


                    // Decrease source stock
                    stock.Quantity -=
                        item.Quantity;


                    // Release reservation if exists
                    if (stock.ReservedQuantity >
                        0)
                    {
                        var reservation =
                            Math.Min(
                                stock.ReservedQuantity,
                                item.Quantity);

                        stock.ReservedQuantity -=
                            reservation;
                    }
                }


                transfer.TransferStatus =
                    StockTransferStatus.InTransit;


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return Ok(new
                {
                    message =
                        "Stock Transfer shipped successfully.",

                    transfer =
                        MapToResponse(transfer)
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message =
                        "Stock Transfer shipping failed.",

                    error = ex.Message
                });
            }
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/receive
        // =====================================================

        [HttpPost("{id:int}/receive")]
        public async Task<IActionResult> Receive(
            int id,
            [FromBody] ReceiveStockTransferDTO dto)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus !=
                    StockTransferStatus.InTransit &&
                transfer.TransferStatus !=
                    StockTransferStatus.PartiallyReceived)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot receive transfer with status {transfer.TransferStatus}."
                });
            }


            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Receive request must contain items."
                });
            }


            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                foreach (var receivedItem in dto.Items)
                {
                    var transferItem =
                        transfer.Items
                            .FirstOrDefault(x =>
                                x.TransferItemId ==
                                receivedItem.TransferItemId);

                    if (transferItem == null)
                    {
                        throw new InvalidOperationException(
                            $"Transfer Item {receivedItem.TransferItemId} not found.");
                    }


                    var remainingQuantity =
                        transferItem.Quantity -
                        transferItem.ReceivedQuantity;


                    if (receivedItem.ReceivedQuantity >
                        remainingQuantity)
                    {
                        throw new InvalidOperationException(
                            $"Received quantity cannot exceed remaining quantity for TransferItem {transferItem.TransferItemId}.");
                    }


                    transferItem.ReceivedQuantity +=
                        receivedItem.ReceivedQuantity;
                }


                // =================================================
                // Determine transfer status
                // =================================================

                var totalQuantity =
                    transfer.Items.Sum(x =>
                        x.Quantity);

                var totalReceived =
                    transfer.Items.Sum(x =>
                        x.ReceivedQuantity);


                if (totalReceived == 0)
                {
                    throw new InvalidOperationException(
                        "No quantity received.");
                }


                if (totalReceived < totalQuantity)
                {
                    transfer.TransferStatus =
                        StockTransferStatus.PartiallyReceived;
                }
                else
                {
                    transfer.TransferStatus =
                        StockTransferStatus.Received;

                    transfer.CompletedAt =
                        DateTimeOffset.UtcNow;
                }


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return Ok(new
                {
                    message =
                        "Stock Transfer received successfully.",

                    transfer =
                        MapToResponse(transfer)
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message =
                        "Stock Transfer receiving failed.",

                    error = ex.Message
                });
            }
        }


        // =====================================================
        // POST /api/stock-transfers/{id}/cancel
        // =====================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var transfer =
                await _unitOfWork.StockTransfers
                    .GetByIdForUpdateAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }


            if (transfer.TransferStatus ==
                    StockTransferStatus.InTransit ||
                transfer.TransferStatus ==
                    StockTransferStatus.Received ||
                transfer.TransferStatus ==
                    StockTransferStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        $"Transfer cannot be cancelled from status {transfer.TransferStatus}."
                });
            }


            if (transfer.TransferStatus ==
                StockTransferStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Transfer is already cancelled."
                });
            }


            transfer.TransferStatus =
                StockTransferStatus.Cancelled;


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Stock Transfer cancelled successfully.",

                transfer =
                    MapToResponse(transfer)
            });
        }


        // =====================================================
        // MAPPER
        // =====================================================

        private static StockTransferResponseDTO MapToResponse(
            StockTransfer transfer)
        {
            return new StockTransferResponseDTO
            {
                TransferId =
                    transfer.TransferId,

                TransferNumber =
                    transfer.TransferNumber,

                SourceWarehouseId =
                    transfer.SourceWarehouseId,

                DestinationWarehouseId =
                    transfer.DestinationWarehouseId,

                RequestedBy =
                    transfer.RequestedBy,

                ApprovedBy =
                    transfer.ApprovedBy,

                TransferStatus =
                    transfer.TransferStatus,

                CreatedAt =
                    transfer.CreatedAt,

                CompletedAt =
                    transfer.CompletedAt,

                Items =
                    transfer.Items
                        .Select(item =>
                            new StockTransferItemResponseDTO
                            {
                                TransferItemId =
                                    item.TransferItemId,

                                ProductId =
                                    item.ProductId,

                                SourceStockId =
                                    item.SourceStockId,

                                SourceLocationId =
                                    item.SourceLocationId,

                                DestinationLocationId =
                                    item.DestinationLocationId,

                                Quantity =
                                    item.Quantity,

                                ReceivedQuantity =
                                    item.ReceivedQuantity
                            })
                        .ToList()
            };
        }
    }
}