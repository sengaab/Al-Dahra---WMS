using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Room;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/rooms
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetRooms(
            [FromQuery] int? warehouseId,
            [FromQuery] int? locationId,
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;


            var rooms =
                await _unitOfWork.Rooms.GetAllAsync(
                    warehouseId,
                    locationId,
                    search,
                    status,
                    page,
                    pageSize);


            return Ok(rooms);
        }


        // =====================================================
        // GET /api/rooms/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room =
                await _unitOfWork.Rooms
                    .GetByIdAsync(id);


            if (room == null)
            {
                return NotFound(new
                {
                    message = "Room not found."
                });
            }


            return Ok(room);
        }


        // =====================================================
        // GET /api/rooms/warehouse/{warehouseId}
        // =====================================================

        [HttpGet("warehouse/{warehouseId:int}")]
        public async Task<IActionResult> GetRoomsByWarehouse(
            int warehouseId)
        {
            if (warehouseId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            var rooms =
                await _unitOfWork.Rooms
                    .GetByWarehouseIdAsync(warehouseId);


            return Ok(rooms);
        }


        // =====================================================
        // GET /api/rooms/location/{locationId}
        // =====================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetRoomsByLocation(
            int locationId)
        {
            if (locationId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            var rooms =
                await _unitOfWork.Rooms
                    .GetByLocationIdAsync(locationId);


            return Ok(rooms);
        }


        // =====================================================
        // POST /api/rooms
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateRoom(
            [FromBody] CreateRoomDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =================================================
            // VALIDATE WAREHOUSE
            // =================================================

            if (dto.WarehouseId.HasValue)
            {
                if (dto.WarehouseId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid WarehouseId."
                    });
                }


                var warehouse =
                    await _unitOfWork.Warehouses
                        .GetEntityByIdAsync(
                            dto.WarehouseId.Value);


                if (warehouse == null)
                {
                    return BadRequest(new
                    {
                        message = "Warehouse not found."
                    });
                }
            }


            // =================================================
            // VALIDATE LOCATION
            // =================================================

            if (dto.LocationId.HasValue)
            {
                if (dto.LocationId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid LocationId."
                    });
                }


                var location =
                    await _unitOfWork.Locations
                        .GetEntityByIdAsync(
                            dto.LocationId.Value);


                if (location == null)
                {
                    return BadRequest(new
                    {
                        message = "Location not found."
                    });
                }
            }


            // =================================================
            // VALIDATE NAME
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Room name is required."
                });
            }


            // =================================================
            // VALIDATE CODE
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Room code is required."
                });
            }


            // =================================================
            // CREATE
            // =================================================

            var room = new Room
            {
                Room_Name = dto.Name.Trim(),

                Room_Code = dto.Code.Trim(),

                Room_Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim(),

                Warehouse_Id = dto.WarehouseId,

                LocationId = dto.LocationId,

                IsActive = true
            };


            await _unitOfWork.Rooms
                .AddAsync(room);


            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Rooms
                    .GetByIdAsync(room.Room_Id);


            return CreatedAtAction(
                nameof(GetRoom),
                new
                {
                    id = room.Room_Id
                },
                result);
        }


        // =====================================================
        // PUT /api/rooms/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRoom(
            int id,
            [FromBody] UpdateRoomDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =================================================
            // GET ROOM
            // =================================================

            var room =
                await _unitOfWork.Rooms
                    .GetEntityByIdAsync(id);


            if (room == null)
            {
                return NotFound(new
                {
                    message = "Room not found."
                });
            }


            // =================================================
            // VALIDATE WAREHOUSE
            // =================================================

            if (dto.WarehouseId.HasValue)
            {
                if (dto.WarehouseId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid WarehouseId."
                    });
                }


                var warehouse =
                    await _unitOfWork.Warehouses
                        .GetEntityByIdAsync(
                            dto.WarehouseId.Value);


                if (warehouse == null)
                {
                    return BadRequest(new
                    {
                        message = "Warehouse not found."
                    });
                }


                room.Warehouse_Id =
                    dto.WarehouseId.Value;
            }
            else
            {
                room.Warehouse_Id = null;
            }


            // =================================================
            // VALIDATE LOCATION
            // =================================================

            if (dto.LocationId.HasValue)
            {
                if (dto.LocationId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid LocationId."
                    });
                }


                var location =
                    await _unitOfWork.Locations
                        .GetEntityByIdAsync(
                            dto.LocationId.Value);


                if (location == null)
                {
                    return BadRequest(new
                    {
                        message = "Location not found."
                    });
                }


                room.LocationId =
                    dto.LocationId.Value;
            }
            else
            {
                room.LocationId = null;
            }


            // =================================================
            // NAME
            // =================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message = "Room name cannot be empty."
                    });
                }


                room.Room_Name =
                    dto.Name.Trim();
            }


            // =================================================
            // CODE
            // =================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Room code cannot be empty."
                    });
                }


                room.Room_Code =
                    dto.Code.Trim();
            }


            // =================================================
            // DESCRIPTION
            // =================================================

            if (dto.Description != null)
            {
                room.Room_Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim();
            }


            // =================================================
            // STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                room.IsActive =
                    dto.IsActive.Value;
            }


            // =================================================
            // UPDATE
            // =================================================

            _unitOfWork.Rooms
                .Update(room);


            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Rooms
                    .GetByIdAsync(id);


            return Ok(result);
        }


        // =====================================================
        // DELETE /api/rooms/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room =
                await _unitOfWork.Rooms
                    .GetEntityByIdAsync(id);


            if (room == null)
            {
                return NotFound(new
                {
                    message = "Room not found."
                });
            }


            _unitOfWork.Rooms
                .Delete(room);


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Room deleted successfully."
            });
        }
    }
}