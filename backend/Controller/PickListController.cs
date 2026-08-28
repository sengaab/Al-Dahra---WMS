using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/pick-lists")]
    [Authorize]
    public class PickListController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PickListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET /api/pick-lists
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pickLists = await _unitOfWork.PickLists.GetAllAsync();

            var result = pickLists.Select(MapToResponse).ToList();

            return Ok(result);
        }


        // =====================================================
        // GET BY ID
        // GET /api/pick-lists/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pickList = await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });

            return Ok(MapToResponse(pickList));
        }


        // =====================================================
        // CREATE
        // POST /api/pick-lists
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePickListDTO dto)
        {
            var pickList = new PickList
            {
                RequestId = dto.RequestId,
                WarehouseId = dto.WarehouseId,
                AssignedTo = dto.AssignedTo,

                PickListStatus = PickListStatus.Pending,

                PickNumber = $"PICK-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                CreatedAt = DateTimeOffset.UtcNow
            };

            await _unitOfWork.PickLists.AddAsync(pickList);

            await _unitOfWork.SaveAsync();

            var created =
                await _unitOfWork.PickLists.GetByIdAsync(
                    pickList.PickListId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = pickList.PickListId },
                MapToResponse(created!));
        }


        // =====================================================
        // UPDATE
        // PUT /api/pick-lists/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePickListDTO dto)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            if (Enum.TryParse<PickListStatus>(
                dto.PickListStatus,
                true,
                out var status))
            {
                pickList.PickListStatus = status;
            }
            else
            {
                return BadRequest(new
                {
                    message = "Invalid PickListStatus."
                });
            }


            pickList.AssignedTo = dto.AssignedTo;

            await _unitOfWork.PickLists.UpdateAsync(pickList);

            await _unitOfWork.SaveAsync();

            return Ok(MapToResponse(pickList));
        }


        // =====================================================
        // ASSIGN
        // POST /api/pick-lists/{id}/assign
        // =====================================================

        [HttpPost("{id:int}/assign")]
        public async Task<IActionResult> Assign(
            int id,
            [FromBody] AssignPickListDTO dto)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            pickList.AssignedTo = dto.AssignedTo;

            await _unitOfWork.PickLists.UpdateAsync(pickList);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Pick list assigned successfully.",
                pickListId = pickList.PickListId,
                assignedTo = pickList.AssignedTo
            });
        }


        // =====================================================
        // START
        // POST /api/pick-lists/{id}/start
        // =====================================================

        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            if (pickList.AssignedTo == null)
                return BadRequest(new
                {
                    message = "Pick list must be assigned before starting."
                });


            pickList.PickListStatus = PickListStatus.InProgress;

            await _unitOfWork.PickLists.UpdateAsync(pickList);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Pick list started successfully.",
                pickListId = pickList.PickListId,
                status = pickList.PickListStatus.ToString()
            });
        }


        // =====================================================
        // COMPLETE
        // POST /api/pick-lists/{id}/complete
        // =====================================================

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            pickList.PickListStatus = PickListStatus.Completed;

            await _unitOfWork.PickLists.UpdateAsync(pickList);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Pick list completed successfully.",
                pickListId = pickList.PickListId,
                status = pickList.PickListStatus.ToString()
            });
        }


        // =====================================================
        // CANCEL
        // POST /api/pick-lists/{id}/cancel
        // =====================================================

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            pickList.PickListStatus = PickListStatus.Cancelled;

            await _unitOfWork.PickLists.UpdateAsync(pickList);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Pick list cancelled successfully.",
                pickListId = pickList.PickListId,
                status = pickList.PickListStatus.ToString()
            });
        }


        // =====================================================
        // PICK ITEM
        // POST /api/pick-lists/{id}/items/{itemId}/pick
        // =====================================================

        [HttpPost("{id:int}/items/{itemId:int}/pick")]
        public async Task<IActionResult> PickItem(
            int id,
            int itemId)
        {
            var pickList =
                await _unitOfWork.PickLists.GetByIdAsync(id);

            if (pickList == null)
                return NotFound(new
                {
                    message = "Pick list not found."
                });


            var item =
                await _unitOfWork.PickLists.GetItemAsync(
                    id,
                    itemId);

            if (item == null)
                return NotFound(new
                {
                    message = "Pick item not found."
                });


            if (item.PickedQuantity >= item.RequiredQuantity)
            {
                return BadRequest(new
                {
                    message = "This item has already been fully picked."
                });
            }


            var remaining =
                item.RequiredQuantity -
                item.PickedQuantity;


            item.PickedQuantity += remaining;

            item.pickItemStatus = PickItemStatus.Picked;


            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Item picked successfully.",
                pickListId = id,
                itemId = itemId,
                pickedQuantity = item.PickedQuantity,
                status = item.pickItemStatus.ToString()
            });
        }


        // =====================================================
        // MAPPING
        // =====================================================

        private static PickListResponseDTO MapToResponse(
            PickList pickList)
        {
            return new PickListResponseDTO
            {
                PickListId = pickList.PickListId,

                PickNumber = pickList.PickNumber,

                RequestId = pickList.RequestId,

                RequestNumber =
                    pickList.StockRequest?.RequestNumber,

                WarehouseId = pickList.WarehouseId,

                WarehouseName =
                    pickList.Warehouse?.Name,

                AssignedTo = pickList.AssignedTo,

                AssigneeName =
                    pickList.Assignee?.Name,

                PickListStatus =
                    pickList.PickListStatus.ToString(),

                CreatedAt = pickList.CreatedAt,

              
                CompletedAt = pickList.CompletedAt,

                Items = pickList.Items
                    .Select(x => new PickItemResponseDTO
                    {
                        PickItemId = x.PickItemId,

                        ProductId = x.ProductId,

                        ProductName =
                            x.Product?.Name,

                        StockId = x.StockId,

                        LocationId = x.LocationId,

                        RequestedQuantity =
                            x.RequiredQuantity,

                        PickedQuantity =
                            x.PickedQuantity,

                        PickItemStatus =
                            x.pickItemStatus.ToString()

                    })
                    .ToList()
            };
        }
    }
}