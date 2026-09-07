using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Putaways;
using whm.Models;
using whm.UnitOfWork;
using System.Security.Claims;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PutawaysController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PutawaysController(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET: api/putaways
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var putaways =
                await _unitOfWork.Putaways.GetAllAsync();

            var result = putaways.Select(p => new PutawayDto
            {
                PutawayId = p.PutawayId,
                PutawayNumber = p.PutawayNumber,
                ReceiptId = p.ReceiptId,
                WarehouseId = p.WarehouseId,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,

                // Your model currently has StatusStatus
                Status = p.StatusStatus,

                Items = p.Items.Select(i => new PutawayItemDto
                {
                    PutawayItemId = i.PutawayItemId,
                    PutawayId = i.PutawayId,
                    ReceiptItemId = i.ReceiptItemId,
                    ProductId = i.ProductId,
                    LocationId = i.LocationId,
                    Quantity = i.Quantity,
                    StockId = i.StockId
                }).ToList()
            });

            return Ok(result);
        }

        // =====================================================
        // GET: api/putaways/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            var result = new PutawayDto
            {
                PutawayId = putaway.PutawayId,
                PutawayNumber = putaway.PutawayNumber,
                ReceiptId = putaway.ReceiptId,
                WarehouseId = putaway.WarehouseId,
                CreatedBy = putaway.CreatedBy,
                CreatedAt = putaway.CreatedAt,

                Status = putaway.StatusStatus,

                Items = putaway.Items.Select(i => new PutawayItemDto
                {
                    PutawayItemId = i.PutawayItemId,
                    PutawayId = i.PutawayId,
                    ReceiptItemId = i.ReceiptItemId,
                    ProductId = i.ProductId,
                    LocationId = i.LocationId,
                    Quantity = i.Quantity,
                    StockId = i.StockId
                }).ToList()
            };

            return Ok(result);
        }

        // =====================================================
        // POST: api/putaways
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePutawayDto dto)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify the authenticated user."
                });
            }

            var putaway = new Putaway
            {
                PutawayNumber = dto.PutawayNumber,
                ReceiptId = dto.ReceiptId,
                WarehouseId = dto.WarehouseId,

                // Get the creator from JWT
                CreatedBy = userId,

                CreatedAt = DateTimeOffset.UtcNow,

                StatusStatus = PutawayStatus.Pending
            };

            await _unitOfWork.Putaways.AddAsync(putaway);

            await _unitOfWork.SaveAsync();

            var result = new PutawayDto
            {
                PutawayId = putaway.PutawayId,
                PutawayNumber = putaway.PutawayNumber,
                ReceiptId = putaway.ReceiptId,
                WarehouseId = putaway.WarehouseId,
                CreatedBy = putaway.CreatedBy,
                CreatedAt = putaway.CreatedAt,
                Status = putaway.StatusStatus
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = putaway.PutawayId },
                result
            );
        }

        // =====================================================
        // PUT: api/putaways/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePutawayDto dto)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus != PutawayStatus.Pending)
            {
                return BadRequest(new
                {
                    message =
                        "Only pending putaways can be updated.",
                    status = putaway.StatusStatus
                });
            }

            putaway.PutawayNumber = dto.PutawayNumber;
            putaway.WarehouseId = dto.WarehouseId;

            await _unitOfWork.Putaways.UpdateAsync(putaway);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Putaway updated successfully.",

                putawayId = putaway.PutawayId,

                status = putaway.StatusStatus
            });
        }

        // =====================================================
        // POST: api/putaways/{id}/assign
        // =====================================================

        [HttpPost("{id:int}/assign")]
        public async Task<IActionResult> Assign(int id)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cancelled putaway cannot be assigned."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed putaway cannot be assigned."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Assigned)
            {
                return BadRequest(new
                {
                    message =
                        "Putaway is already assigned."
                });
            }

            putaway.StatusStatus = PutawayStatus.Assigned;

            await _unitOfWork.Putaways.UpdateAsync(putaway);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Putaway assigned successfully.",

                putawayId = putaway.PutawayId,

                status = putaway.StatusStatus
            });
        }

        // =====================================================
        // POST: api/putaways/{id}/complete
        // =====================================================

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cancelled putaway cannot be completed."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Putaway is already completed."
                });
            }

            if (!putaway.Items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Putaway cannot be completed because it has no items."
                });
            }

            putaway.StatusStatus = PutawayStatus.Completed;

            await _unitOfWork.Putaways.UpdateAsync(putaway);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Putaway completed successfully.",

                putawayId = putaway.PutawayId,

                status = putaway.StatusStatus
            });
        }

        // =====================================================
        // POST: api/putaways/{id}/cancel
        // =====================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Completed putaway cannot be cancelled."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Putaway is already cancelled."
                });
            }

            putaway.StatusStatus = PutawayStatus.Cancelled;

            await _unitOfWork.Putaways.UpdateAsync(putaway);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Putaway cancelled successfully.",

                putawayId = putaway.PutawayId,

                status = putaway.StatusStatus
            });
        }

        // =====================================================
        // GET: api/putaways/{id}/items
        // =====================================================

        [HttpGet("{id:int}/items")]
        public async Task<IActionResult> GetItems(int id)
        {
            var exists =
                await _unitOfWork.Putaways.ExistsAsync(id);

            if (!exists)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            var items =
                await _unitOfWork.Putaways.GetItemsAsync(id);

            var result = items.Select(i => new PutawayItemDto
            {
                PutawayItemId = i.PutawayItemId,
                PutawayId = i.PutawayId,
                ReceiptItemId = i.ReceiptItemId,
                ProductId = i.ProductId,
                LocationId = i.LocationId,
                Quantity = i.Quantity,
                StockId = i.StockId
            });

            return Ok(result);
        }

        // =====================================================
        // POST: api/putaways/{id}/items
        // =====================================================

        [HttpPost("{id:int}/items")]
        public async Task<IActionResult> AddItem(
            int id,
            [FromBody] CreatePutawayItemDto dto)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot add items to a cancelled putaway."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot add items to a completed putaway."
                });
            }

            if (dto.Quantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Quantity must be greater than zero."
                });
            }

            var item = new PutawayItem
            {
                PutawayId = id,
                ReceiptItemId = dto.ReceiptItemId,
                ProductId = dto.ProductId,
                LocationId = dto.LocationId,
                Quantity = dto.Quantity,
                StockId = dto.StockId
            };

            await _unitOfWork.Putaways.AddItemAsync(item);

            await _unitOfWork.SaveAsync();

            var result = new PutawayItemDto
            {
                PutawayItemId = item.PutawayItemId,
                PutawayId = item.PutawayId,
                ReceiptItemId = item.ReceiptItemId,
                ProductId = item.ProductId,
                LocationId = item.LocationId,
                Quantity = item.Quantity,
                StockId = item.StockId
            };

            return Ok(result);
        }

        // =====================================================
        // PUT: api/putaways/{id}/items/{itemId}
        // =====================================================

        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateItem(
            int id,
            int itemId,
            [FromBody] UpdatePutawayItemDto dto)
        {
            var putaway =
                await _unitOfWork.Putaways.GetByIdAsync(id);

            if (putaway == null)
            {
                return NotFound(new
                {
                    message = "Putaway not found."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot update items in a cancelled putaway."
                });
            }

            if (putaway.StatusStatus == PutawayStatus.Completed)
            {
                return BadRequest(new
                {
                    message =
                        "Cannot update items in a completed putaway."
                });
            }

            if (dto.Quantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Quantity must be greater than zero."
                });
            }

            var item =
                await _unitOfWork.Putaways
                    .GetItemByIdAsync(id, itemId);

            if (item == null)
            {
                return NotFound(new
                {
                    message =
                        "Putaway item not found."
                });
            }

            item.LocationId = dto.LocationId;
            item.Quantity = dto.Quantity;
            item.StockId = dto.StockId;

            await _unitOfWork.Putaways.UpdateItemAsync(item);

            await _unitOfWork.SaveAsync();

            var result = new PutawayItemDto
            {
                PutawayItemId = item.PutawayItemId,
                PutawayId = item.PutawayId,
                ReceiptItemId = item.ReceiptItemId,
                ProductId = item.ProductId,
                LocationId = item.LocationId,
                Quantity = item.Quantity,
                StockId = item.StockId
            };

            return Ok(result);
        }
    }
}