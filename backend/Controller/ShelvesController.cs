using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Shelf;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

            var shelves =
                await _unitOfWork.Shelves.GetAllAsync(
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
            var shelf =
                await _unitOfWork.Shelves
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
            // VALIDATE RACK
            // =================================================

            if (dto.RackId.HasValue)
            {
                var rack =
                    await _unitOfWork.Racks
                        .GetEntityByIdAsync(
                            dto.RackId.Value);

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

            if (dto.LocationId.HasValue)
            {
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
            // CREATE
            // =================================================

            var shelf = new Shelf
            {
                Row_Id = dto.RackId,

                LocationId = dto.LocationId,

                Shelf_Code = dto.Code.Trim(),

                Shelf_Name = dto.Name.Trim(),

                IsActive = true
            };

            await _unitOfWork.Shelves
                .AddAsync(shelf);

            await _unitOfWork.SaveAsync();


            // =================================================
            // RESULT
            // =================================================

            var result =
                await _unitOfWork.Shelves
                    .GetByIdAsync(shelf.Shelf_Id);

            return CreatedAtAction(
                nameof(GetShelf),
                new { id = shelf.Shelf_Id },
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


            // =================================================
            // GET SHELF
            // =================================================

            var shelf =
                await _unitOfWork.Shelves
                    .GetEntityByIdAsync(id);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }


            // =================================================
            // VALIDATE RACK
            // =================================================

            if (dto.RackId.HasValue)
            {
                var rack =
                    await _unitOfWork.Racks
                        .GetEntityByIdAsync(
                            dto.RackId.Value);

                if (rack == null)
                {
                    return BadRequest(new
                    {
                        message = "Rack not found."
                    });
                }

                shelf.Row_Id = dto.RackId.Value;
            }
            else
            {
                shelf.Row_Id = null;
            }


            // =================================================
            // VALIDATE LOCATION
            // =================================================

            if (dto.LocationId.HasValue)
            {
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

                shelf.LocationId = dto.LocationId.Value;
            }
            else
            {
                shelf.LocationId = null;
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
                        message = "Shelf code cannot be empty."
                    });
                }

                shelf.Shelf_Code =
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
                        message = "Shelf name cannot be empty."
                    });
                }

                shelf.Shelf_Name =
                    dto.Name.Trim();
            }


            // =================================================
            // STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                shelf.IsActive =
                    dto.IsActive.Value;
            }


            // =================================================
            // UPDATE
            // =================================================

            _unitOfWork.Shelves
                .Update(shelf);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Shelves
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE /api/shelves/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShelf(int id)
        {
            var shelf =
                await _unitOfWork.Shelves
                    .GetEntityByIdAsync(id);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }


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
        public async Task<IActionResult> GetByRack(
            int rackId)
        {
            var rack =
                await _unitOfWork.Racks
                    .GetEntityByIdAsync(rackId);

            if (rack == null)
            {
                return NotFound(new
                {
                    message = "Rack not found."
                });
            }

            var shelves =
                await _unitOfWork.Shelves
                    .GetByRackIdAsync(rackId);

            return Ok(shelves);
        }


        // =====================================================
        // GET /api/shelves/location/{locationId}
        // =====================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetByLocation(
            int locationId)
        {
            var location =
                await _unitOfWork.Locations
                    .GetEntityByIdAsync(locationId);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }

            var shelves =
                await _unitOfWork.Shelves
                    .GetByLocationIdAsync(locationId);

            return Ok(shelves);
        }
    }
}