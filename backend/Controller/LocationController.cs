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


        // =========================================================
        // GET: api/locations
        // =========================================================

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
            // -----------------------------------------------------
            // Validate pagination
            // -----------------------------------------------------

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;


            // -----------------------------------------------------
            // Validate WarehouseId
            // -----------------------------------------------------

            if (warehouseId.HasValue && warehouseId.Value <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // -----------------------------------------------------
            // Validate PartitionId
            // -----------------------------------------------------

            if (partitionId.HasValue && partitionId.Value <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid PartitionId."
                });
            }


            // -----------------------------------------------------
            // Validate BinId
            // -----------------------------------------------------

            if (binId.HasValue && binId.Value <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // -----------------------------------------------------
            // Get Locations
            // -----------------------------------------------------

            var locations = await _unitOfWork.Locations
                .GetAllAsync(
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


        // =========================================================
        // GET: api/locations/{id}
        // =========================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // -----------------------------------------------------
            // Get Location
            // -----------------------------------------------------

            var location = await _unitOfWork.Locations
                .GetByIdAsync(id);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            return Ok(location);
        }


        // =========================================================
        // GET: api/locations/{id}/structure
        // =========================================================

        [HttpGet("{id:int}/structure")]
        public async Task<IActionResult> GetStructure(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // -----------------------------------------------------
            // Get Structure
            // -----------------------------------------------------

            var structure = await _unitOfWork.Locations
                .GetStructureAsync(id);


            if (structure == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            return Ok(structure);
        }


        // =========================================================
        // POST: api/locations
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateLocation(
            [FromBody] CreateLocationDto dto)
        {
            // -----------------------------------------------------
            // Validate ModelState
            // -----------------------------------------------------

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =====================================================
            // Validate BinId
            // =====================================================

            if (dto.BinId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // =====================================================
            // Validate Code
            // =====================================================

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Location code is required."
                });
            }


            // =====================================================
            // Validate Name
            // =====================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Location name is required."
                });
            }


            // =====================================================
            // Validate Type
            // =====================================================

            if (string.IsNullOrWhiteSpace(dto.Type))
            {
                return BadRequest(new
                {
                    message = "Location type is required."
                });
            }


            // =====================================================
            // Get Bin
            // =====================================================

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(dto.BinId);


            if (bin == null)
            {
                return BadRequest(new
                {
                    message = "Bin not found."
                });
            }


            // =====================================================
            // Check Bin already has Location
            // =====================================================

            if (bin.Location != null)
            {
                return Conflict(new
                {
                    message =
                        "This bin already has a location."
                });
            }


            // =====================================================
            // Get WarehouseId from Bin -> Partition
            // =====================================================

            var warehouseId = bin.Partition.WarehouseId;


            // =====================================================
            // Create Location
            // =====================================================

            var now = DateTimeOffset.UtcNow;

            var location = new Location
            {
                BinId = dto.BinId,

                WarehouseId = warehouseId,

                Code = dto.Code.Trim(),

                Name = dto.Name.Trim(),

                Type = dto.Type.Trim(),

                IsActive = true,

                CreatedAt = now,

                UpdatedAt = now
            };


            // =====================================================
            // Add Location
            // =====================================================

            await _unitOfWork.Locations
                .AddAsync(location);


            // =====================================================
            // Save
            // =====================================================

            await _unitOfWork.SaveAsync();


            // =====================================================
            // Get Created Location
            // =====================================================

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


        // =========================================================
        // PUT: api/locations/{id}
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLocation(
            int id,
            [FromBody] UpdateLocationDto dto)
        {
            // -----------------------------------------------------
            // Validate ModelState
            // -----------------------------------------------------

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // =====================================================
            // Get Location
            // =====================================================

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // =====================================================
            // Determine new BinId
            // =====================================================

            var binId =
                dto.BinId ??
                location.BinId;


            // =====================================================
            // Validate BinId
            // =====================================================

            if (binId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // =====================================================
            // Get Bin
            // =====================================================

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(binId);


            if (bin == null)
            {
                return BadRequest(new
                {
                    message = "Bin not found."
                });
            }


            // =====================================================
            // If changing Bin
            // =====================================================

            if (binId != location.BinId)
            {
                // -------------------------------------------------
                // Make sure new Bin doesn't already have Location
                // -------------------------------------------------

                if (bin.Location != null)
                {
                    return Conflict(new
                    {
                        message =
                            "The selected bin already has a location."
                    });
                }
            }


            // =====================================================
            // Get WarehouseId from Bin
            // =====================================================

            var warehouseId = bin.Partition.WarehouseId;


            // =====================================================
            // Update Code
            // =====================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message =
                            "Location code cannot be empty."
                    });
                }

                location.Code = dto.Code.Trim();
            }


            // =====================================================
            // Update Name
            // =====================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message =
                            "Location name cannot be empty."
                    });
                }

                location.Name = dto.Name.Trim();
            }


            // =====================================================
            // Update Type
            // =====================================================

            if (dto.Type != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Type))
                {
                    return BadRequest(new
                    {
                        message =
                            "Location type cannot be empty."
                    });
                }

                location.Type = dto.Type.Trim();
            }


            // =====================================================
            // Update IsActive
            // =====================================================

            if (dto.IsActive.HasValue)
            {
                location.IsActive = dto.IsActive.Value;
            }


            // =====================================================
            // Update Relationship
            // =====================================================

            location.BinId = binId;

            location.WarehouseId = warehouseId;

            location.UpdatedAt = DateTimeOffset.UtcNow;


            // =====================================================
            // Save
            // =====================================================

            _unitOfWork.Locations
                .Update(location);

            await _unitOfWork.SaveAsync();


            // =====================================================
            // Return Updated Location
            // =====================================================

            var result = await _unitOfWork.Locations
                .GetByIdAsync(id);


            return Ok(result);
        }


        // =========================================================
        // DELETE: api/locations/{id}
        // =========================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // =====================================================
            // Get Location
            // =====================================================

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // =====================================================
            // Check Stock
            // =====================================================

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


            // =====================================================
            // Delete Location
            // =====================================================

            _unitOfWork.Locations
                .Delete(location);


            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Location deleted successfully."
            });
        }


        // =========================================================
        // GET: api/locations/{id}/inventory
        // =========================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // -----------------------------------------------------
            // Check Location
            // -----------------------------------------------------

            var location = await _unitOfWork.Locations
                .GetByIdAsync(id);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // -----------------------------------------------------
            // Get Inventory
            // -----------------------------------------------------

            var inventory = await _unitOfWork.Locations
                .GetInventoryAsync(id);


            return Ok(inventory);
        }


        // =========================================================
        // GET: api/locations/{id}/occupancy
        // =========================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // -----------------------------------------------------
            // Check Location
            // -----------------------------------------------------

            var location = await _unitOfWork.Locations
                .GetByIdAsync(id);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // -----------------------------------------------------
            // Get Occupancy
            // -----------------------------------------------------

            var occupancy = await _unitOfWork.Locations
                .GetOccupancyAsync(id);


            // -----------------------------------------------------
            // Location has no stock
            // -----------------------------------------------------

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


        // =========================================================
        // GET: api/locations/tree
        // =========================================================

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            // -----------------------------------------------------
            // Get all locations
            // -----------------------------------------------------

            var locations = await _unitOfWork.Locations
                .GetAllAsync(
                    page: 1,
                    pageSize: 100);


            // -----------------------------------------------------
            // Build tree
            // -----------------------------------------------------

            var tree = locations
                .GroupBy(x => new
                {
                    x.WarehouseId,
                    x.WarehouseName
                })
                .Select(warehouseGroup => new WarehouseTreeDto
                {
                    WarehouseId = warehouseGroup.Key.WarehouseId,

                    Name = warehouseGroup.Key.WarehouseName ?? string.Empty,

                    Code = string.Empty,

                    IsActive = true,

                    Locations = warehouseGroup
                        .Select(location => new LocationTreeDto
                        {
                            LocationId = location.LocationId,

                            BinId = location.BinId,

                            PartitionId =
                                location.PartitionId,

                            WarehouseId =
                                location.WarehouseId,

                            Code = location.Code,

                            Name = location.Name,

                            Type = location.Type,

                            IsActive = location.IsActive,

                            Children = new List<LocationTreeDto>()
                        })
                        .ToList()
                })
                .ToList();


            return Ok(tree);
        }


        // =========================================================
        // GET: api/locations/bin/{binId}
        // =========================================================

        [HttpGet("bin/{binId:int}")]
        public async Task<IActionResult> GetByBin(int binId)
        {
            // -----------------------------------------------------
            // Validate BinId
            // -----------------------------------------------------

            if (binId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // -----------------------------------------------------
            // Check Bin
            // -----------------------------------------------------

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(binId);


            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            // -----------------------------------------------------
            // Get Location
            // -----------------------------------------------------

            var locations = await _unitOfWork.Locations
                .GetByBinIdAsync(binId);


            return Ok(locations);
        }
    }
}