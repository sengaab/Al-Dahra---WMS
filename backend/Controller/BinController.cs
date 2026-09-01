using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Bin;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BinsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/bins
        // =====================================================

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


            var bins =
                await _unitOfWork.Bins.GetAllAsync(
                    shelfId,
                    locationId,
                    search,
                    status,
                    page,
                    pageSize);

            return Ok(bins);
        }


        // =====================================================
        // GET /api/bins/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBin(int id)
        {
            var bin =
                await _unitOfWork.Bins
                    .GetByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }

            return Ok(bin);
        }


        // =====================================================
        // POST /api/bins
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateBin(
            [FromBody] CreateBinDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =================================================
            // VALIDATE SHELF
            // =================================================

            if (dto.ShelfId.HasValue)
            {
                if (dto.ShelfId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid ShelfId."
                    });
                }

                var shelf =
                    await _unitOfWork.Shelves
                        .GetEntityByIdAsync(
                            dto.ShelfId.Value);

                if (shelf == null)
                {
                    return NotFound(new
                    {
                        message = "Shelf not found."
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
                    return NotFound(new
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
                    message = "Bin code is required."
                });
            }


            // =================================================
            // VALIDATE NAME
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Bin name is required."
                });
            }


            // =================================================
            // CREATE
            // =================================================

            var bin = new Bin
            {
                Shelf_Id = dto.ShelfId,

                LocationId = dto.LocationId,

                Bin_Code = dto.Code.Trim(),

                Bin_Name = dto.Name.Trim(),

                IsActive = true
            };


            await _unitOfWork.Bins.AddAsync(bin);

            await _unitOfWork.SaveAsync();


            // =================================================
            // RETURN CREATED
            // =================================================

            var result =
                await _unitOfWork.Bins
                    .GetByIdAsync(bin.Bin_Id);

            return CreatedAtAction(
                nameof(GetBin),
                new { id = bin.Bin_Id },
                result);
        }


        // =====================================================
        // PUT /api/bins/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBin(
            int id,
            [FromBody] UpdateBinDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            var bin =
                await _unitOfWork.Bins
                    .GetEntityByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            // =================================================
            // UPDATE SHELF
            // =================================================

            if (dto.ShelfId.HasValue)
            {
                if (dto.ShelfId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid ShelfId."
                    });
                }

                var shelf =
                    await _unitOfWork.Shelves
                        .GetEntityByIdAsync(
                            dto.ShelfId.Value);

                if (shelf == null)
                {
                    return NotFound(new
                    {
                        message = "Shelf not found."
                    });
                }

                bin.Shelf_Id = dto.ShelfId.Value;
            }
            else
            {
                bin.Shelf_Id = null;
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

                var location =
                    await _unitOfWork.Locations
                        .GetEntityByIdAsync(
                            dto.LocationId.Value);

                if (location == null)
                {
                    return NotFound(new
                    {
                        message = "Location not found."
                    });
                }

                bin.LocationId = dto.LocationId.Value;
            }
            else
            {
                bin.LocationId = null;
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
                        message = "Bin code cannot be empty."
                    });
                }

                bin.Bin_Code = dto.Code.Trim();
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
                        message = "Bin name cannot be empty."
                    });
                }

                bin.Bin_Name = dto.Name.Trim();
            }


            // =================================================
            // UPDATE STATUS
            // =================================================

            if (dto.IsActive.HasValue)
            {
                bin.IsActive = dto.IsActive.Value;
            }


            // =================================================
            // SAVE
            // =================================================

            _unitOfWork.Bins.Update(bin);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Bins
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE /api/bins/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBin(int id)
        {
            var bin =
                await _unitOfWork.Bins
                    .GetEntityByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            _unitOfWork.Bins.Delete(bin);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Bin deleted successfully."
            });
        }


        // =====================================================
        // GET /api/bins/{id}/stocks
        // =====================================================

        [HttpGet("{id:int}/stocks")]
        public async Task<IActionResult> GetStocks(int id)
        {
            var bin =
                await _unitOfWork.Bins
                    .GetByIdAsync(id);

            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            // لو عندك StockRepository فيه GetByBinIdAsync
            // نقدر نستخدمه هنا.
            return Ok(new
            {
                binId = bin.BinId,
                binName = bin.Name,
                stockCount = bin.StockCount
            });
        }


        // =====================================================
        // GET /api/bins/by-shelf/{shelfId}
        // =====================================================

        [HttpGet("by-shelf/{shelfId:int}")]
        public async Task<IActionResult> GetByShelf(
            int shelfId)
        {
            var shelf =
                await _unitOfWork.Shelves
                    .GetEntityByIdAsync(shelfId);

            if (shelf == null)
            {
                return NotFound(new
                {
                    message = "Shelf not found."
                });
            }


            var bins =
                await _unitOfWork.Bins
                    .GetByShelfIdAsync(shelfId);

            return Ok(bins);
        }


        // =====================================================
        // GET /api/bins/by-location/{locationId}
        // =====================================================

        [HttpGet("by-location/{locationId:int}")]
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


            var bins =
                await _unitOfWork.Bins
                    .GetByLocationIdAsync(locationId);

            return Ok(bins);
        }
    }
}