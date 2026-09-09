using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/stock-requests")]
    [Authorize]
    public class StockRequestsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockRequestsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/stock-requests
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests =
                await _unitOfWork.StockRequests.GetAllAsync();

            var result = requests.Select(MapToResponse).ToList();

            return Ok(result);
        }


        // =====================================================
        // GET /api/stock-requests/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdWithItemsAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            return Ok(MapToResponse(request));
        }


        // =====================================================
        // POST /api/stock-requests
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateStockRequestDTO dto)
        {
            if (await _unitOfWork.StockRequests
                .RequestNumberExistsAsync(dto.RequestNumber))
            {
                return Conflict(new
                {
                    message = "Request number already exists."
                });
            }

            var requesterExists = await _unitOfWork.User
                .GetByIdAsync(dto.RequestedBy);

            if (requesterExists == null)
            {
                return BadRequest(new
                {
                    message = "RequestedBy user does not exist."
                });
            }

            var request = new StockRequest
            {
                RequestNumber = dto.RequestNumber,
                DepartmentId = dto.DepartmentId,
                SiteId = dto.SiteId,
                RequestedBy = dto.RequestedBy,
                Priority = dto.Priority,
                StockRequestStatus = StockRequestStatus.Draft,
                RequestedAt = DateTimeOffset.UtcNow
            };

            await _unitOfWork.StockRequests.AddAsync(request);

            await _unitOfWork.SaveAsync();

            var created =
                await _unitOfWork.StockRequests.GetByIdWithItemsAsync(
                    request.RequestId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = request.RequestId },
                MapToResponse(created!));
        }


        // =====================================================
        // PUT /api/stock-requests/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStockRequestDTO dto)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (await _unitOfWork.StockRequests
                .RequestNumberExistsAsync(
                    dto.RequestNumber,
                    id))
            {
                return Conflict(new
                {
                    message = "Request number already exists."
                });
            }

            if (request.StockRequestStatus != StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft requests can be updated."
                });
            }

            request.RequestNumber = dto.RequestNumber;
            request.DepartmentId = dto.DepartmentId;
            request.SiteId = dto.SiteId;
            request.Priority = dto.Priority;

            _unitOfWork.StockRequests.Update(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request updated successfully."
            });
        }


        // =====================================================
        // DELETE /api/stock-requests/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus != StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft requests can be deleted."
                });
            }

            _unitOfWork.StockRequests.Delete(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request deleted successfully."
            });
        }


        // =====================================================
        // POST /api/stock-requests/{id}/submit
        // =====================================================

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdWithItemsAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus != StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Only draft requests can be submitted."
                });
            }

            if (!request.Items.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Cannot submit a request without items."
                });
            }

            request.StockRequestStatus =
                StockRequestStatus.Submitted;

            _unitOfWork.StockRequests.Update(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request submitted successfully.",
                status = request.StockRequestStatus
            });
        }


        // =====================================================
        // POST /api/stock-requests/{id}/approve
        // =====================================================

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            [FromQuery] Guid approvedBy)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus !=
                StockRequestStatus.Submitted &&
                request.StockRequestStatus !=
                StockRequestStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        "Only submitted requests can be approved."
                });
            }

            var approver =
                await _unitOfWork.User.GetByIdAsync(approvedBy);

            if (approver == null)
            {
                return BadRequest(new
                {
                    message = "Approver does not exist."
                });
            }

            request.ApprovedBy = approvedBy;
            request.ApprovedAt = DateTimeOffset.UtcNow;

            request.StockRequestStatus =
                StockRequestStatus.Approved;

            _unitOfWork.StockRequests.Update(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request approved successfully.",
                status = request.StockRequestStatus
            });
        }


        // =====================================================
        // POST /api/stock-requests/{id}/reject
        // =====================================================

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus !=
                StockRequestStatus.Submitted &&
                request.StockRequestStatus !=
                StockRequestStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        "Only submitted or pending requests can be rejected."
                });
            }

            request.StockRequestStatus =
                StockRequestStatus.Rejected;

            _unitOfWork.StockRequests.Update(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request rejected successfully.",
                status = request.StockRequestStatus
            });
        }


        // =====================================================
        // POST /api/stock-requests/{id}/cancel
        // =====================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus ==
                    StockRequestStatus.Issued ||
                request.StockRequestStatus ==
                    StockRequestStatus.Completed ||
                request.StockRequestStatus ==
                    StockRequestStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message =
                        "This request cannot be cancelled."
                });
            }

            request.StockRequestStatus =
                StockRequestStatus.Cancelled;

            _unitOfWork.StockRequests.Update(request);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Stock request cancelled successfully.",
                status = request.StockRequestStatus
            });
        }


        // =====================================================
        // GET ITEMS
        // GET /api/stock-requests/{id}/items
        // =====================================================

        [HttpGet("{id:int}/items")]
        public async Task<IActionResult> GetItems(int id)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            var items =
                await _unitOfWork.StockRequests.GetItemsAsync(id);

            var result = items.Select(MapItemToResponse).ToList();

            return Ok(result);
        }


        // =====================================================
        // POST ITEM
        // POST /api/stock-requests/{id}/items
        // =====================================================

        [HttpPost("{id:int}/items")]
        public async Task<IActionResult> AddItem(
            int id,
            [FromBody] CreateStockRequestItemDTO dto)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus !=
                StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be added to draft requests."
                });
            }

            if (dto.RequestedQuantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Requested quantity must be greater than zero."
                });
            }

            if (await _unitOfWork.StockRequests
                .ProductExistsInRequestAsync(
                    id,
                    dto.ProductId))
            {
                return Conflict(new
                {
                    message =
                        "This product already exists in the request."
                });
            }

            var item = new StockRequestItem
            {
                RequestId = id,
                ProductId = dto.ProductId,
                RequestedQuantity = dto.RequestedQuantity,
                ReservedQuantity = 0,
                IssuedQuantity = 0,
                RemainingQuantity = dto.RequestedQuantity
            };

            await _unitOfWork.StockRequests.AddItemAsync(item);

            await _unitOfWork.SaveAsync();

            var created =
                await _unitOfWork.StockRequests.GetItemByIdAsync(
                    id,
                    item.RequestItemId);

            return Ok(MapItemToResponse(created!));
        }


        // =====================================================
        // PUT ITEM
        // PUT /api/stock-requests/{id}/items/{itemId}
        // =====================================================

        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateItem(
            int id,
            int itemId,
            [FromBody] UpdateStockRequestItemDTO dto)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus !=
                StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be updated in draft requests."
                });
            }

            var item =
                await _unitOfWork.StockRequests.GetItemByIdAsync(
                    id,
                    itemId);

            if (item == null)
                return NotFound(new
                {
                    message = "Request item not found."
                });

            if (dto.RequestedQuantity <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "Requested quantity must be greater than zero."
                });
            }

            if (await _unitOfWork.StockRequests
                .ProductExistsInRequestAsync(
                    id,
                    dto.ProductId,
                    itemId))
            {
                return Conflict(new
                {
                    message =
                        "This product already exists in the request."
                });
            }

            item.ProductId = dto.ProductId;
            item.RequestedQuantity = dto.RequestedQuantity;

            item.RemainingQuantity =
                dto.RequestedQuantity -
                item.ReservedQuantity -
                item.IssuedQuantity;

            if (item.RemainingQuantity < 0)
            {
                item.RemainingQuantity = 0;
            }

            _unitOfWork.StockRequests.UpdateItem(item);

            await _unitOfWork.SaveAsync();

            return Ok(MapItemToResponse(item));
        }


        // =====================================================
        // DELETE ITEM
        // DELETE /api/stock-requests/{id}/items/{itemId}
        // =====================================================

        [HttpDelete("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(
            int id,
            int itemId)
        {
            var request =
                await _unitOfWork.StockRequests.GetByIdAsync(id);

            if (request == null)
                return NotFound(new
                {
                    message = "Stock request not found."
                });

            if (request.StockRequestStatus !=
                StockRequestStatus.Draft)
            {
                return BadRequest(new
                {
                    message =
                        "Items can only be deleted from draft requests."
                });
            }

            var item =
                await _unitOfWork.StockRequests.GetItemByIdAsync(
                    id,
                    itemId);

            if (item == null)
                return NotFound(new
                {
                    message = "Request item not found."
                });

            _unitOfWork.StockRequests.DeleteItem(item);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Request item deleted successfully."
            });
        }


        // =====================================================
        // MAPPING
        // =====================================================

        private static StockRequestResponseDTO MapToResponse(
            StockRequest request)
        {
            return new StockRequestResponseDTO
            {
                RequestId = request.RequestId,
                RequestNumber = request.RequestNumber,

                DepartmentId = request.DepartmentId,
                DepartmentName = request.Department?.Name,

                SiteId = request.SiteId,
                SiteName = request.Site?.Name,

                RequestedBy = request.RequestedBy,
                RequesterName = request.Requester?.Name,

                ApprovedBy = request.ApprovedBy,
                ApproverName = request.Approver?.Name,

                Priority = request.Priority,

                RequestedAt = request.RequestedAt,
                ApprovedAt = request.ApprovedAt,

                StockRequestStatus =
                    request.StockRequestStatus,

                Items = request.Items
                    .Select(MapItemToResponse)
                    .ToList()
            };
        }


        private static StockRequestItemResponseDTO
            MapItemToResponse(StockRequestItem item)
        {
            return new StockRequestItemResponseDTO
            {
                RequestItemId = item.RequestItemId,

                RequestId = item.RequestId,

                ProductId = item.ProductId,

                ProductName = item.Product?.Name,

                SKU = item.Product?.SKU,

                RequestedQuantity =
                    item.RequestedQuantity,

                ReservedQuantity =
                    item.ReservedQuantity,

                IssuedQuantity =
                    item.IssuedQuantity,

                RemainingQuantity =
                    item.RemainingQuantity
            };
        }
    }
}