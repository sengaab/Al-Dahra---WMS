using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Rack;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RacksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RacksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET /api/racks
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetRacks(
            [FromQuery] int? roomId,
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

            var racks = await _unitOfWork.Racks.GetAllAsync(
                roomId,
                locationId,
                search,
                status,
                page,
                pageSize);

            return Ok(racks);
        }

        // =====================================================
        // GET /api/racks/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRack(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid RackId."
                });
            }

            var rack = await _unitOfWork.Racks
                .GetByIdAsync(id);

            if (rack == null)
            {
                return NotFound(new
                {
                    message = "Rack not found."
                });
            }

            return Ok(rack);
        }

        // =====================================================
        // POST /api/racks
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateRack(
            [FromBody] CreateRackDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // =================================================
            // VALIDATE NAME
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Rack name is required."
                });
            }

            // =================================================
            // VALIDATE ROOM
            // =================================================

            if (dto.RoomId.HasValue)
            {
                if (dto.RoomId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid RoomId."
                    });
                }

                var room = await _unitOfWork.Rooms
                    .GetEntityByIdAsync(dto.RoomId.Value);

                if (room == null)
                {
                    return BadRequest(new
                    {
                        message = "Room not found."
                    });
                }
            }

            // =================================================
            // VALIDATE LOCATION
            // =================================================

            Location? location = null;

            if (dto.LocationId.HasValue)
            {
                if (dto.LocationId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid LocationId."
                    });
                }

                location = await _unitOfWork.Locations
                    .GetEntityByIdAsync(dto.LocationId.Value);

                if (location == null)
                {
                    return BadRequest(new
                    {
                        message = "Location not found."
                    });
                }

                // A Location should not already belong to another Rack
                if (location.RackId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a Rack."
                    });
                }

                // If Location already belongs to another physical level,
                // prevent invalid hierarchy.
                if (location.ShelfId.HasValue ||
                    location.BinId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a lower-level location."
                    });
                }
            }

            // =================================================
            // CREATE RACK
            // =================================================

            var rack = new Rack
            {
                Room_Id = dto.RoomId,

                Rack_Code = string.IsNullOrWhiteSpace(dto.Code)
                    ? null
                    : dto.Code.Trim(),

                Rack_Name = dto.Name.Trim(),

                IsActive = true
            };

            await _unitOfWork.Racks.AddAsync(rack);

            await _unitOfWork.SaveAsync();

            // =================================================
            // ASSIGN LOCATION → RACK
            // Location contains RackId
            // =================================================

            if (location != null)
            {
                location.RackId = rack.Rack_Id;

                _unitOfWork.Locations.Update(location);

                await _unitOfWork.SaveAsync();
            }

            // =================================================
            // RETURN CREATED RACK
            // =================================================

            var result = await _unitOfWork.Racks
                .GetByIdAsync(rack.Rack_Id);

            return CreatedAtAction(
                nameof(GetRack),
                new
                {
                    id = rack.Rack_Id
                },
                result);
        }

        // =====================================================
        // PUT /api/racks/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRack(
            int id,
            [FromBody] UpdateRackDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid RackId."
                });
            }

            // =================================================
            // GET RACK
            // =================================================

            var rack = await _unitOfWork.Racks
                .GetEntityByIdAsync(id);

            if (rack == null)
            {
                return NotFound(new
                {
                    message = "Rack not found."
                });
            }

            // =================================================
            // UPDATE ROOM
            // =================================================

            if (dto.RoomId.HasValue)
            {
                if (dto.RoomId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid RoomId."
                    });
                }

                var room = await _unitOfWork.Rooms
                    .GetEntityByIdAsync(dto.RoomId.Value);

                if (room == null)
                {
                    return BadRequest(new
                    {
                        message = "Room not found."
                    });
                }

                rack.Room_Id = dto.RoomId.Value;
            }

            // =================================================
            // UPDATE LOCATION
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

                var newLocation = await _unitOfWork.Locations
                    .GetEntityByIdAsync(dto.LocationId.Value);

                if (newLocation == null)
                {
                    return BadRequest(new
                    {
                        message = "Location not found."
                    });
                }

                // Check if this location belongs to another Rack
                if (newLocation.RackId.HasValue &&
                    newLocation.RackId.Value != rack.Rack_Id)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to another Rack."
                    });
                }

                // A Rack location cannot already be a Shelf/Bin location
                if (newLocation.ShelfId.HasValue ||
                    newLocation.BinId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a lower-level location."
                    });
                }

                // Find current Location assigned to this Rack
                var currentLocation = await _unitOfWork.Locations
                    .GetEntityByIdAsync(rack.Rack_Id);

                // Remove old Rack → Location relation
                if (currentLocation != null &&
                    currentLocation.LocationId != newLocation.LocationId)
                {
                    currentLocation.RackId = null;

                    _unitOfWork.Locations.Update(currentLocation);
                }

                // Assign new Location → Rack
                newLocation.RackId = rack.Rack_Id;

                _unitOfWork.Locations.Update(newLocation);
            }

            // =================================================
            // UPDATE CODE
            // =================================================

            if (dto.Code != null)
            {
                rack.Rack_Code =
                    string.IsNullOrWhiteSpace(dto.Code)
                        ? null
                        : dto.Code.Trim();
            }

            // =================================================
            // UPDATE NAME
            // =================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message = "Rack name cannot be empty."
                    });
                }

                rack.Rack_Name = dto.Name.Trim();
            }

            // =================================================
            // UPDATE STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                rack.IsActive = dto.IsActive.Value;
            }

            // =================================================
            // SAVE RACK
            // =================================================

            _unitOfWork.Racks.Update(rack);

            await _unitOfWork.SaveAsync();

            // =================================================
            // RETURN UPDATED RACK
            // =================================================

            var result = await _unitOfWork.Racks
                .GetByIdAsync(id);

            return Ok(result);
        }

        // =====================================================
        // DELETE /api/racks/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRack(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid RackId."
                });
            }

            var rack = await _unitOfWork.Racks
                .GetEntityByIdAsync(id);

            if (rack == null)
            {
                return NotFound(new
                {
                    message = "Rack not found."
                });
            }

            // =================================================
            // PREVENT DELETE IF RACK HAS SHELVES
            // =================================================

            var shelves = await _unitOfWork.Shelves
                .GetByRackIdAsync(id);

            if (shelves.Any())
            {
                return BadRequest(new
                {
                    message = "Cannot delete rack because it contains shelves."
                });
            }

            // =================================================
            // REMOVE LOCATION → RACK RELATION
            // =================================================

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);

            if (location != null)
            {
                location.RackId = null;

                _unitOfWork.Locations.Update(location);
            }

            // =================================================
            // DELETE RACK
            // =================================================

            _unitOfWork.Racks.Delete(rack);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Rack deleted successfully."
            });
        }

        // =====================================================
        // GET /api/racks/room/{roomId}
        // =====================================================

        [HttpGet("room/{roomId:int}")]
        public async Task<IActionResult> GetByRoom(int roomId)
        {
            if (roomId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid RoomId."
                });
            }

            var room = await _unitOfWork.Rooms
                .GetEntityByIdAsync(roomId);

            if (room == null)
            {
                return NotFound(new
                {
                    message = "Room not found."
                });
            }

            var racks = await _unitOfWork.Racks
                .GetByRoomIdAsync(roomId);

            return Ok(racks);
        }

        // =====================================================
        // GET /api/racks/location/{locationId}
        // =====================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetByLocation(int locationId)
        {
            if (locationId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(locationId);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var racks = await _unitOfWork.Racks
                .GetByLocationIdAsync(locationId);

            return Ok(racks);
        }
    }
}