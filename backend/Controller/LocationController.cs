using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Location;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetLocations()
        {
            var locations =
                await _unitOfWork.Locations
                    .GetAllAsync();

            return Ok(locations);
        }


        // =====================================================
        // GET /api/locations/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location =
                await _unitOfWork.Locations
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


        // =====================================================
        // GET /api/locations/{id}/structure
        //
        // Returns:
        // Location
        // ├── Rooms
        // ├── Racks
        // ├── Shelves
        // └── Bins
        // =====================================================

        [HttpGet("{id:int}/structure")]
        public async Task<IActionResult> GetStructure(int id)
        {
            var structure =
                await _unitOfWork.Locations
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
            // VALIDATE WAREHOUSE - OPTIONAL
            // =================================================

            Warehouse? warehouse = null;

            if (dto.WarehouseId.HasValue)
            {
                warehouse =
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
            // VALIDATE PARENT
            // =================================================

            if (dto.ParentLocationId.HasValue)
            {
                var parent =
                    await _unitOfWork.Locations
                        .GetEntityByIdAsync(
                            dto.ParentLocationId.Value);

                if (parent == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location not found."
                    });
                }


                // Parent must belong to same warehouse

                if (parent.WarehouseId !=
                    dto.WarehouseId)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location must belong to the same warehouse."
                    });
                }
            }


            // =================================================
            // VALIDATE TYPE
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Type))
            {
                return BadRequest(new
                {
                    message =
                        "Location type is required."
                });
            }


            // =================================================
            // CREATE
            // =================================================

            var location = new Location
            {
                WarehouseId =
                    dto.WarehouseId,

                ParentLocationId =
                    dto.ParentLocationId,

                Code =
                    dto.Code.Trim(),

                Name =
                    dto.Name.Trim(),

                Type =
                    dto.Type.Trim(),

                IsActive = true,

                CreatedAt =
                    DateTimeOffset.UtcNow,

                UpdatedAt =
                    DateTimeOffset.UtcNow
            };


            await _unitOfWork.Locations
                .AddAsync(location);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Locations
                    .GetByIdAsync(
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
                await _unitOfWork.Locations
                    .GetEntityByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // =================================================
            // WAREHOUSE
            // =================================================

            if (dto.WarehouseId.HasValue)
            {
                var warehouse =
                    await _unitOfWork.Warehouses
                        .GetEntityByIdAsync(
                            dto.WarehouseId.Value);

                if (warehouse == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Warehouse not found."
                    });
                }

                location.WarehouseId =
                    dto.WarehouseId.Value;
            }


            // =================================================
            // PARENT
            // =================================================

            if (dto.ParentLocationId.HasValue)
            {
                if (dto.ParentLocationId.Value == id)
                {
                    return BadRequest(new
                    {
                        message =
                            "A location cannot be its own parent."
                    });
                }


                var parent =
                    await _unitOfWork.Locations
                        .GetEntityByIdAsync(
                            dto.ParentLocationId.Value);

                if (parent == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location not found."
                    });
                }


                if (parent.WarehouseId !=
                    location.WarehouseId)
                {
                    return BadRequest(new
                    {
                        message =
                            "Parent location must belong to the same warehouse."
                    });
                }


                location.ParentLocationId =
                    dto.ParentLocationId.Value;
            }
            else if (dto.WarehouseId.HasValue)
            {
                location.ParentLocationId = null;
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
                        message =
                            "Location code cannot be empty."
                    });
                }

                location.Code =
                    dto.Code.Trim();
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
                        message =
                            "Location name cannot be empty."
                    });
                }

                location.Name =
                    dto.Name.Trim();
            }


            // =================================================
            // TYPE
            // =================================================

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

                location.Type =
                    dto.Type.Trim();
            }


            // =================================================
            // STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                location.IsActive =
                    dto.IsActive.Value;
            }


            location.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Locations
                .Update(location);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Locations
                    .GetByIdAsync(id);


            return Ok(result);
        }


        // =====================================================
        // DELETE /api/locations/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocation(
            int id)
        {
            var location =
                await _unitOfWork.Locations
                    .GetEntityByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // =================================================
            // CHECK CHILD LOCATIONS
            // =================================================

            var hasChildren =
                await _unitOfWork.Locations
                    .GetChildrenAsync(id);


            if (hasChildren.Any())
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this location because it has child locations."
                });
            }


            _unitOfWork.Locations
                .Delete(location);

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
        public async Task<IActionResult> GetChildren(
            int id)
        {
            var location =
                await _unitOfWork.Locations
                    .GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message =
                        "Location not found."
                });
            }


            var children =
                await _unitOfWork.Locations
                    .GetChildrenAsync(id);

            return Ok(children);
        }


        // =====================================================
        // GET /api/locations/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(
            int id)
        {
            var location =
                await _unitOfWork.Locations
                    .GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(new
                {
                    message =
                        "Location not found."
                });
            }


            var inventory =
                await _unitOfWork.Locations
                    .GetInventoryAsync(id);

            return Ok(inventory);
        }


        // =====================================================
        // GET /api/locations/{id}/occupancy
        // =====================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(
            int id)
        {
            var occupancy =
                await _unitOfWork.Locations
                    .GetOccupancyAsync(id);

            if (occupancy == null)
            {
                return NotFound(new
                {
                    message =
                        "Location not found."
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
                await _unitOfWork.Locations
                    .GetTreeAsync();

            return Ok(tree);
        }
    }
}