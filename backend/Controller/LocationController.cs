using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Location;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET /api/locations
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetLocations(
            [FromQuery] int? warehouseId = null,
            [FromQuery] int? partitionId = null,
            [FromQuery] int? binId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var locations = await _unitOfWork.Locations.GetAllAsync(
                warehouseId,
                partitionId,
                binId,
                search,
                type,
                status,
                page,
                pageSize);

            return Ok(locations);
        }

        // =====================================================
        // GET /api/locations/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            return Ok(location);
        }

        // =====================================================
        // GET /api/locations/{id}/structure
        // =====================================================

        [HttpGet("{id:int}/structure")]
        public async Task<IActionResult> GetStructure(int id)
        {
            var structure = await _unitOfWork.Locations.GetStructureAsync(id);

            if (structure == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            return Ok(structure);
        }

        // =====================================================
        // POST /api/locations
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateLocation(
            [FromBody] CreateLocationDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // =================================================
            // Basic validation
            // =================================================

            if (dto.BinId <= 0)
            {
                return BadRequest(new
                {
                    message = "BinId is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Location code is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Location name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Type))
            {
                return BadRequest(new
                {
                    message = "Location type is required."
                });
            }

            // =================================================
            // Validate Bin
            // =================================================

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(dto.BinId);

            if (bin == null)
            {
                return BadRequest(new
                {
                    message = "Bin not found."
                });
            }

            // =================================================
            // Create Location
            // =================================================

            var now = DateTimeOffset.UtcNow;

            var location = new Location
            {
                BinId = dto.BinId,

                Code = dto.Code.Trim(),
                Name = dto.Name.Trim(),
                Type = dto.Type.Trim(),

                IsActive = true,

                CreatedAt = now,
                UpdatedAt = now
            };

            await _unitOfWork.Locations.AddAsync(location);

            await _unitOfWork.SaveAsync();

            var result = await _unitOfWork.Locations
                .GetByIdAsync(location.LocationId);

            return CreatedAtAction(
                nameof(GetLocation),
                new
                {
                    id = location.LocationId
                },
                result);
        }

        // =====================================================
        // PUT /api/locations/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLocation(
            int id,
            [FromBody] UpdateLocationDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            // =================================================
            // Calculate new values
            // =================================================

            var binId = dto.BinId ?? location.BinId;

            // =================================================
            // Validate Bin
            // =================================================

            if (binId <= 0)
            {
                return BadRequest(new
                {
                    message = "BinId is required."
                });
            }

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(binId);

            if (bin == null)
            {
                return BadRequest(new
                {
                    message = "Bin not found."
                });
            }

            // =================================================
            // Update Code
            // =================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Location code cannot be empty."
                    });
                }

                location.Code = dto.Code.Trim();
            }

            // =================================================
            // Update Name
            // =================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message = "Location name cannot be empty."
                    });
                }

                location.Name = dto.Name.Trim();
            }

            // =================================================
            // Update Type
            // =================================================

            if (dto.Type != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Type))
                {
                    return BadRequest(new
                    {
                        message = "Location type cannot be empty."
                    });
                }

                location.Type = dto.Type.Trim();
            }

            // =================================================
            // Update IsActive
            // =================================================

            if (dto.IsActive.HasValue)
            {
                location.IsActive = dto.IsActive.Value;
            }

            // =================================================
            // Update Relationship
            // =================================================

            location.BinId = binId;

            location.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.Locations.Update(location);

            await _unitOfWork.SaveAsync();

            var result = await _unitOfWork.Locations
                .GetByIdAsync(id);

            return Ok(result);
        }

        // =====================================================
        // DELETE /api/locations/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            // =================================================
            // Check Stock
            // =================================================

            var inventory = await _unitOfWork.Locations
                .GetInventoryAsync(id);

            if (inventory.Any())
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this location because it contains stock."
                });
            }

            // =================================================
            // Delete
            // =================================================

            _unitOfWork.Locations.Delete(location);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Location deleted successfully."
            });
        }

        // =====================================================
        // GET /api/locations/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var location = await _unitOfWork.Locations
                .GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var inventory = await _unitOfWork.Locations
                .GetInventoryAsync(id);

            return Ok(inventory);
        }

        // =====================================================
        // GET /api/locations/{id}/occupancy
        // =====================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(int id)
        {
            var location = await _unitOfWork.Locations
                .GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var occupancy = await _unitOfWork.Locations
                .GetOccupancyAsync(id);

            if (occupancy == null)
            {
                return Ok(new LocationOccupancyDto
                {
                    LocationId = id,
                    LocationName = location.Name,
                    LocationType = location.Type,

                    TotalStockItems = 0,
                    TotalQuantity = 0,
                    TotalReservedQuantity = 0,
                    TotalAvailableQuantity = 0,
                    TotalValue = 0,

                    IsOccupied = false
                });
            }

            return Ok(occupancy);
        }

        // =====================================================
        // GET /api/locations/tree
        // =====================================================

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var tree = await _unitOfWork.Locations
                .GetAllAsync();

            return Ok(tree);
        }

        // =====================================================
        // GET /api/locations/bin/{binId}
        // =====================================================

        [HttpGet("bin/{binId:int}")]
        public async Task<IActionResult> GetByBin(int binId)
        {
            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(binId);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }

            var locations = await _unitOfWork.Locations
                .GetByBinIdAsync(binId);

            return Ok(locations);
        }
    }
}