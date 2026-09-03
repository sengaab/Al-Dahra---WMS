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
            [FromQuery] int? parentLocationId = null,
            [FromQuery] int? roomId = null,
            [FromQuery] int? rackId = null,
            [FromQuery] int? shelfId = null,
            [FromQuery] int? binId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var locations = await _unitOfWork.Locations.GetAllAsync(
                warehouseId,
                parentLocationId,
                roomId,
                rackId,
                shelfId,
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
            var location =
                await _unitOfWork.Locations.GetByIdAsync(id);

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
            var structure =
                await _unitOfWork.Locations.GetStructureAsync(id);

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
            // Validate Warehouse
            // =================================================

            if (dto.WarehouseId.HasValue)
            {
                var warehouse =
                    await _unitOfWork.Warehouses.GetEntityByIdAsync(
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
            // Validate Parent Location
            // =================================================

            if (dto.ParentLocationId.HasValue)
            {
                var parent =
                    await _unitOfWork.Locations.GetEntityByIdAsync(
                        dto.ParentLocationId.Value);

                if (parent == null)
                {
                    return BadRequest(new
                    {
                        message = "Parent location not found."
                    });
                }

                if (dto.WarehouseId.HasValue &&
                    parent.WarehouseId != dto.WarehouseId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location must belong to the same warehouse."
                    });
                }
            }

            // =================================================
            // Validate Room
            // =================================================

            if (dto.RoomId.HasValue)
            {
                var room =
                    await _unitOfWork.Rooms.GetEntityByIdAsync(
                        dto.RoomId.Value);

                if (room == null)
                {
                    return BadRequest(new
                    {
                        message = "Room not found."
                    });
                }

                if (dto.WarehouseId.HasValue &&
                    room.Warehouse_Id != dto.WarehouseId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Room must belong to the selected warehouse."
                    });
                }
            }

            // =================================================
            // Validate Rack
            // =================================================

            if (dto.RackId.HasValue)
            {
                var rack =
                    await _unitOfWork.Racks.GetEntityByIdAsync(
                        dto.RackId.Value);

                if (rack == null)
                {
                    return BadRequest(new
                    {
                        message = "Rack not found."
                    });
                }

                if (dto.RoomId.HasValue &&
                    rack.Room_Id != dto.RoomId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Rack must belong to the selected room."
                    });
                }
            }

            // =================================================
            // Validate Shelf
            // =================================================

            if (dto.ShelfId.HasValue)
            {
                var shelf =
                    await _unitOfWork.Shelves.GetEntityByIdAsync(
                        dto.ShelfId.Value);

                if (shelf == null)
                {
                    return BadRequest(new
                    {
                        message = "Shelf not found."
                    });
                }

                if (dto.RackId.HasValue &&
                    shelf.Rack_Id != dto.RackId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Shelf must belong to the selected rack."
                    });
                }
            }

            // =================================================
            // Validate Bin
            // =================================================

            if (dto.BinId.HasValue)
            {
                var bin =
                    await _unitOfWork.Bins.GetEntityByIdAsync(
                        dto.BinId.Value);

                if (bin == null)
                {
                    return BadRequest(new
                    {
                        message = "Bin not found."
                    });
                }

                if (dto.ShelfId.HasValue &&
                    bin.Shelf_Id != dto.ShelfId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Bin must belong to the selected shelf."
                    });
                }

                // Bin -> Location is One-to-One
                var existingLocationId =
                    await _unitOfWork.Locations
                        .GetLocationIdByBinIdAsync(dto.BinId.Value);

                if (existingLocationId > 0)
                {
                    return Conflict(new
                    {
                        message =
                            "This Bin is already linked to another Location.",
                        locationId = existingLocationId
                    });
                }
            }

            // =================================================
            // Create Location
            // =================================================

            var now = DateTimeOffset.UtcNow;

            var location = new Location
            {
                WarehouseId = dto.WarehouseId,
                ParentLocationId = dto.ParentLocationId,

                RoomId = dto.RoomId,
                RackId = dto.RackId,
                ShelfId = dto.ShelfId,
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

            var result =
                await _unitOfWork.Locations.GetByIdAsync(
                    location.LocationId);

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

            var location =
                await _unitOfWork.Locations.GetEntityByIdAsync(id);

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

            var warehouseId =
                dto.WarehouseId ?? location.WarehouseId;

            var parentLocationId =
                dto.ParentLocationId ?? location.ParentLocationId;

            var roomId =
                dto.RoomId ?? location.RoomId;

            var rackId =
                dto.RackId ?? location.RackId;

            var shelfId =
                dto.ShelfId ?? location.ShelfId;

            var binId =
                dto.BinId ?? location.BinId;

            // =================================================
            // Validate Warehouse
            // =================================================

            if (warehouseId.HasValue)
            {
                var warehouse =
                    await _unitOfWork.Warehouses.GetEntityByIdAsync(
                        warehouseId.Value);

                if (warehouse == null)
                {
                    return BadRequest(new
                    {
                        message = "Warehouse not found."
                    });
                }
            }

            // =================================================
            // Validate Parent
            // =================================================

            if (parentLocationId.HasValue)
            {
                if (parentLocationId.Value == id)
                {
                    return BadRequest(new
                    {
                        message =
                            "A location cannot be its own parent."
                    });
                }

                var parent =
                    await _unitOfWork.Locations.GetEntityByIdAsync(
                        parentLocationId.Value);

                if (parent == null)
                {
                    return BadRequest(new
                    {
                        message = "Parent location not found."
                    });
                }

                if (warehouseId.HasValue &&
                    parent.WarehouseId != warehouseId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location must belong to the same warehouse."
                    });
                }
            }

            // =================================================
            // Validate Room
            // =================================================

            if (roomId.HasValue)
            {
                var room =
                    await _unitOfWork.Rooms.GetEntityByIdAsync(
                        roomId.Value);

                if (room == null)
                {
                    return BadRequest(new
                    {
                        message = "Room not found."
                    });
                }

                if (warehouseId.HasValue &&
                    room.Warehouse_Id != warehouseId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Room must belong to the selected warehouse."
                    });
                }
            }

            // =================================================
            // Validate Rack
            // =================================================

            if (rackId.HasValue)
            {
                var rack =
                    await _unitOfWork.Racks.GetEntityByIdAsync(
                        rackId.Value);

                if (rack == null)
                {
                    return BadRequest(new
                    {
                        message = "Rack not found."
                    });
                }

                if (roomId.HasValue &&
                    rack.Room_Id != roomId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Rack must belong to the selected room."
                    });
                }
            }

            // =================================================
            // Validate Shelf
            // =================================================

            if (shelfId.HasValue)
            {
                var shelf =
                    await _unitOfWork.Shelves.GetEntityByIdAsync(
                        shelfId.Value);

                if (shelf == null)
                {
                    return BadRequest(new
                    {
                        message = "Shelf not found."
                    });
                }

                if (rackId.HasValue &&
                    shelf.Rack_Id != rackId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Shelf must belong to the selected rack."
                    });
                }
            }

            // =================================================
            // Validate Bin
            // =================================================

            if (binId.HasValue)
            {
                var bin =
                    await _unitOfWork.Bins.GetEntityByIdAsync(
                        binId.Value);

                if (bin == null)
                {
                    return BadRequest(new
                    {
                        message = "Bin not found."
                    });
                }

                if (shelfId.HasValue &&
                    bin.Shelf_Id != shelfId.Value)
                {
                    return BadRequest(new
                    {
                        message =
                            "Bin must belong to the selected shelf."
                    });
                }

                // Check One-to-One relationship
                var existingLocationId =
                    await _unitOfWork.Locations
                        .GetLocationIdByBinIdAsync(binId.Value);

                if (existingLocationId > 0 &&
                    existingLocationId != id)
                {
                    return Conflict(new
                    {
                        message =
                            "This Bin is already linked to another Location.",
                        locationId = existingLocationId
                    });
                }
            }

            // =================================================
            // Update basic fields
            // =================================================

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

            if (dto.IsActive.HasValue)
            {
                location.IsActive = dto.IsActive.Value;
            }

            // =================================================
            // Assign relationships
            // =================================================

            location.WarehouseId = warehouseId;
            location.ParentLocationId = parentLocationId;

            location.RoomId = roomId;
            location.RackId = rackId;
            location.ShelfId = shelfId;
            location.BinId = binId;

            location.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.Locations.Update(location);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Locations.GetByIdAsync(id);

            return Ok(result);
        }

        // =====================================================
        // DELETE /api/locations/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location =
                await _unitOfWork.Locations.GetEntityByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            // =================================================
            // Check child locations
            // =================================================

            var children =
                await _unitOfWork.Locations.GetChildrenAsync(id);

            if (children.Any())
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this location because it has child locations."
                });
            }

            // =================================================
            // Delete
            // =================================================

            _unitOfWork.Locations.Delete(location);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Location deleted successfully."
            });
        }

        // =====================================================
        // GET /api/locations/{id}/children
        // =====================================================

        [HttpGet("{id:int}/children")]
        public async Task<IActionResult> GetChildren(int id)
        {
            var location =
                await _unitOfWork.Locations.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var children =
                await _unitOfWork.Locations.GetChildrenAsync(id);

            return Ok(children);
        }

        // =====================================================
        // GET /api/locations/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var location =
                await _unitOfWork.Locations.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var inventory =
                await _unitOfWork.Locations.GetInventoryAsync(id);

            return Ok(inventory);
        }

        // =====================================================
        // GET /api/locations/{id}/occupancy
        // =====================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(int id)
        {
            var location =
                await _unitOfWork.Locations.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var occupancy =
                await _unitOfWork.Locations.GetOccupancyAsync(id);

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
            var tree =
                await _unitOfWork.Locations.GetTreeAsync();

            return Ok(tree);
        }
    }
}