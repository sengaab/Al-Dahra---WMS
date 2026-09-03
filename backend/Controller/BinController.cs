using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Bin;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BinsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BinsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================================================
        // GET: api/bins
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetBins(
            [FromQuery] int? shelfId,
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

            var bins = await _unitOfWork.Bins.GetAllAsync(
                shelfId,
                locationId,
                search,
                status,
                page,
                pageSize);

            return Ok(bins);
        }

        // =========================================================
        // GET: api/bins/{id}
        // =========================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBin(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }

            var bin = await _unitOfWork.Bins.GetByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }

            return Ok(bin);
        }

        // =========================================================
        // GET: api/bins/shelf/{shelfId}
        // =========================================================

        [HttpGet("shelf/{shelfId:int}")]
        public async Task<IActionResult> GetBinsByShelf(int shelfId)
        {
            if (shelfId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid ShelfId."
                });
            }

            var shelf = await _unitOfWork.Shelves
                .GetEntityByIdAsync(shelfId);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }

            var bins = await _unitOfWork.Bins
                .GetByShelfIdAsync(shelfId);

            return Ok(bins);
        }

        // =========================================================
        // GET: api/bins/location/{locationId}
        // =========================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetBinsByLocation(int locationId)
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

            var bins = await _unitOfWork.Bins
                .GetByLocationIdAsync(locationId);

            return Ok(bins);
        }

        // =========================================================
        // POST: api/bins
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateBin(
            [FromBody] CreateBinDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Bin name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Bin code is required."
                });
            }

            // =====================================================
            // Validate Shelf
            // =====================================================

            if (dto.ShelfId.HasValue)
            {
                if (dto.ShelfId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid ShelfId."
                    });
                }

                var shelf = await _unitOfWork.Shelves
                    .GetEntityByIdAsync(dto.ShelfId.Value);

                if (shelf == null)
                {
                    return BadRequest(new
                    {
                        message = "Shelf not found."
                    });
                }
            }

            // =====================================================
            // Validate Location
            // =====================================================

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

                // Location already belongs to another Bin
                if (location.BinId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to a Bin."
                    });
                }

                // Location already belongs to Rack/Shelf
                if (location.RackId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to a Rack."
                    });
                }

                if (location.ShelfId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to a Shelf."
                    });
                }
            }

            // =====================================================
            // Create Bin
            // =====================================================

            var bin = new Bin
            {
                Bin_Name = dto.Name.Trim(),

                Bin_Code = dto.Code.Trim(),

                Shelf_Id = dto.ShelfId,

                IsActive = true
            };

            await _unitOfWork.Bins.AddAsync(bin);

            await _unitOfWork.SaveAsync();

            // =====================================================
            // Assign Location -> Bin
            // Location contains the FK: BinId
            // =====================================================

            if (location != null)
            {
                location.BinId = bin.Bin_Id;

                _unitOfWork.Locations.Update(location);

                await _unitOfWork.SaveAsync();
            }

            var result = await _unitOfWork.Bins
                .GetByIdAsync(bin.Bin_Id);

            return CreatedAtAction(
                nameof(GetBin),
                new { id = bin.Bin_Id },
                result);
        }

        // =========================================================
        // PUT: api/bins/{id}
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBin(
            int id,
            [FromBody] UpdateBinDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }

            // =====================================================
            // Update Shelf
            // =====================================================

            if (dto.ShelfId.HasValue)
            {
                if (dto.ShelfId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid ShelfId."
                    });
                }

                var shelf = await _unitOfWork.Shelves
                    .GetEntityByIdAsync(dto.ShelfId.Value);

                if (shelf == null)
                {
                    return BadRequest(new
                    {
                        message = "Shelf not found."
                    });
                }

                bin.Shelf_Id = dto.ShelfId.Value;
            }

            // =====================================================
            // Update Location
            // =====================================================

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

                // Location belongs to another Bin
                if (newLocation.BinId.HasValue &&
                    newLocation.BinId.Value != bin.Bin_Id)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to another Bin."
                    });
                }

                // Location belongs to Rack
                if (newLocation.RackId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to a Rack."
                    });
                }

                // Location belongs to Shelf
                if (newLocation.ShelfId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "This Location is already assigned to a Shelf."
                    });
                }

                // =================================================
                // Find current Location assigned to this Bin
                // =================================================

                var currentLocation = await _unitOfWork.Locations
                    .GetEntityByIdAsync(bin.Bin_Id);

                if (currentLocation != null &&
                    currentLocation.LocationId != newLocation.LocationId)
                {
                    currentLocation.BinId = null;

                    _unitOfWork.Locations.Update(currentLocation);
                }

                // Assign new Location
                newLocation.BinId = bin.Bin_Id;

                _unitOfWork.Locations.Update(newLocation);
            }

            // =====================================================
            // Update Code
            // =====================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Bin code cannot be empty."
                    });
                }

                bin.Bin_Code = dto.Code.Trim();
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
                        message = "Bin name cannot be empty."
                    });
                }

                bin.Bin_Name = dto.Name.Trim();
            }

            // =====================================================
            // Update Active Status
            // =====================================================

            if (dto.IsActive.HasValue)
            {
                bin.IsActive = dto.IsActive.Value;
            }

            _unitOfWork.Bins.Update(bin);

            await _unitOfWork.SaveAsync();

            var result = await _unitOfWork.Bins
                .GetByIdAsync(id);

            return Ok(result);
        }

        // =========================================================
        // DELETE: api/bins/{id}
        // =========================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBin(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }

            // =====================================================
            // Prevent deleting Bin if it contains Stock
            // =====================================================

            if (bin.Stocks.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Cannot delete bin because it contains stock."
                });
            }

            // =====================================================
            // Clear Location -> Bin relation
            // =====================================================

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(id);

            if (location != null)
            {
                location.BinId = null;

                _unitOfWork.Locations.Update(location);
            }

            // =====================================================
            // Delete Bin
            // =====================================================

            _unitOfWork.Bins.Delete(bin);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Bin deleted successfully."
            });
        }
    }
}