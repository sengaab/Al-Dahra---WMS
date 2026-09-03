using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Shelf;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShelvesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShelvesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET /api/shelves
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetShelves(
            [FromQuery] int? rackId,
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

            var shelves = await _unitOfWork.Shelves.GetAllAsync(
                rackId,
                locationId,
                search,
                status,
                page,
                pageSize);

            return Ok(shelves);
        }

        // =====================================================
        // GET /api/shelves/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShelf(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid ShelfId."
                });
            }

            var shelf = await _unitOfWork.Shelves
                .GetByIdAsync(id);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }

            return Ok(shelf);
        }

        // =====================================================
        // POST /api/shelves
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateShelf(
            [FromBody] CreateShelfDto dto)
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
                    message = "Shelf name is required."
                });
            }

            // =================================================
            // VALIDATE CODE
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Shelf code is required."
                });
            }

            // =================================================
            // VALIDATE RACK
            // =================================================

            if (dto.RackId.HasValue)
            {
                if (dto.RackId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid RackId."
                    });
                }

                var rack = await _unitOfWork.Racks
                    .GetEntityByIdAsync(dto.RackId.Value);

                if (rack == null)
                {
                    return BadRequest(new
                    {
                        message = "Rack not found."
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

                // Location already assigned to another Shelf
                if (location.ShelfId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a Shelf."
                    });
                }

                // A Shelf Location cannot already belong
                // to a Bin.
                if (location.BinId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a Bin."
                    });
                }
            }

            // =================================================
            // CREATE SHELF
            // =================================================

            var shelf = new Shelf
            {
                Rack_Id = dto.RackId,

                Shelf_Code = dto.Code.Trim(),

                Shelf_Name = dto.Name.Trim(),

                IsActive = true
            };

            await _unitOfWork.Shelves
                .AddAsync(shelf);

            await _unitOfWork.SaveAsync();

            // =================================================
            // ASSIGN LOCATION → SHELF
            //
            // Location contains ShelfId
            // =================================================

            if (location != null)
            {
                location.ShelfId = shelf.Shelf_Id;

                _unitOfWork.Locations
                    .Update(location);

                await _unitOfWork.SaveAsync();
            }

            // =================================================
            // RESULT
            // =================================================

            var result = await _unitOfWork.Shelves
                .GetByIdAsync(shelf.Shelf_Id);

            return CreatedAtAction(
                nameof(GetShelf),
                new
                {
                    id = shelf.Shelf_Id
                },
                result);
        }

        // =====================================================
        // PUT /api/shelves/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShelf(
            int id,
            [FromBody] UpdateShelfDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid ShelfId."
                });
            }

            // =================================================
            // GET SHELF
            // =================================================

            var shelf = await _unitOfWork.Shelves
                .GetEntityByIdAsync(id);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }

            // =================================================
            // UPDATE RACK
            // =================================================

            if (dto.RackId.HasValue)
            {
                if (dto.RackId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid RackId."
                    });
                }

                var rack = await _unitOfWork.Racks
                    .GetEntityByIdAsync(dto.RackId.Value);

                if (rack == null)
                {
                    return BadRequest(new
                    {
                        message = "Rack not found."
                    });
                }

                shelf.Rack_Id = dto.RackId.Value;
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

                // Location belongs to another Shelf
                if (newLocation.ShelfId.HasValue &&
                    newLocation.ShelfId.Value != shelf.Shelf_Id)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to another Shelf."
                    });
                }

                // Location belongs to a Bin
                if (newLocation.BinId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "This Location is already assigned to a Bin."
                    });
                }

                // Find current Location assigned to this Shelf
                var currentLocation = await _unitOfWork.Locations
                    .GetEntityByIdAsync(shelf.Shelf_Id);

                // Remove old relationship
                if (currentLocation != null &&
                    currentLocation.LocationId != newLocation.LocationId)
                {
                    currentLocation.ShelfId = null;

                    _unitOfWork.Locations
                        .Update(currentLocation);
                }

                // Assign new Location → Shelf
                newLocation.ShelfId = shelf.Shelf_Id;

                _unitOfWork.Locations
                    .Update(newLocation);
            }

            // =================================================
            // UPDATE CODE
            // =================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Shelf code cannot be empty."
                    });
                }

                shelf.Shelf_Code = dto.Code.Trim();
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
                        message = "Shelf name cannot be empty."
                    });
                }

                shelf.Shelf_Name = dto.Name.Trim();
            }

            // =================================================
            // UPDATE STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                shelf.IsActive = dto.IsActive.Value;
            }

            // =================================================
            // SAVE
            // =================================================

            _unitOfWork.Shelves
                .Update(shelf);

            await _unitOfWork.SaveAsync();

            // =================================================
            // RESULT
            // =================================================

            var result = await _unitOfWork.Shelves
                .GetByIdAsync(id);

            return Ok(result);
        }

        // =====================================================
        // DELETE /api/shelves/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShelf(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid ShelfId."
                });
            }

            var shelf = await _unitOfWork.Shelves
                .GetEntityByIdAsync(id);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }

            // =================================================
            // PREVENT DELETE IF SHELF HAS BINS
            // =================================================

            var bins = await _unitOfWork.Bins
                .GetByShelfIdAsync(id);

            if (bins.Any())
            {
                return BadRequest(new
                {
                    message = "Cannot delete shelf because it contains bins."
                });
            }

            // =================================================
            // REMOVE LOCATION → SHELF RELATION
            // =================================================

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);

            if (location != null)
            {
                location.ShelfId = null;

                _unitOfWork.Locations
                    .Update(location);
            }

            // =================================================
            // DELETE SHELF
            // =================================================

            _unitOfWork.Shelves
                .Delete(shelf);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Shelf deleted successfully."
            });
        }

        // =====================================================
        // GET /api/shelves/rack/{rackId}
        // =====================================================

        [HttpGet("rack/{rackId:int}")]
        public async Task<IActionResult> GetByRack(int rackId)
        {
            if (rackId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid RackId."
                });
            }

            var rack = await _unitOfWork.Racks
                .GetEntityByIdAsync(rackId);

            if (rack == null)
            {
                return NotFound(new
                {
                    message = "Rack not found."
                });
            }

            var shelves = await _unitOfWork.Shelves
                .GetByRackIdAsync(rackId);

            return Ok(shelves);
        }

        // =====================================================
        // GET /api/shelves/location/{locationId}
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

            var shelves = await _unitOfWork.Shelves
                .GetByLocationIdAsync(locationId);

            return Ok(shelves);
        }
    }
}