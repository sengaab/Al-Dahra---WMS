using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BinsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public BinsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Bins
        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var bins =
                await unitOfWork.Bins.GetAllAsync();

            return Ok(bins.Select(b => new
            {
                binId = b.Bin_Id,
                binName = b.Bin_Name,
                binCode = b.Bin_Code,
                binDescription = b.Bin_Description,
                isActive = b.IsActive,
                shelfId = b.Shelf_Id,
                shelfName = b.Shelf?.Shelf_Name
            }));
        }

        // GET: api/Bins/5
        [HttpGet("GetbyId/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bin =
                await unitOfWork.Bins.GetByIdAsync(id);

            if (bin == null)
                return NotFound("Bin not found.");

            return Ok(new
            {
                binId = bin.Bin_Id,
                binName = bin.Bin_Name,
                binCode = bin.Bin_Code,
                binDescription = bin.Bin_Description,
                isActive = bin.IsActive,
                shelfId = bin.Shelf_Id,
                shelfName = bin.Shelf?.Shelf_Name
            });
        }

        // GET: api/Bins/shelf/5
        [HttpGet("GetBinbyshelfid/{shelfId:int}")]
        public async Task<IActionResult> GetByShelf(
            int shelfId)
        {
            var shelf =
                await unitOfWork.Shelves
                    .GetByIdAsync(shelfId);

            if (shelf == null)
                return NotFound("Shelf not found.");

            var bins =
                await unitOfWork.Bins
                    .GetByShelfIdAsync(shelfId);

            return Ok(bins.Select(b => new
            {
                binId = b.Bin_Id,
                binName = b.Bin_Name,
                binCode = b.Bin_Code,
                binDescription = b.Bin_Description,
                isActive = b.IsActive,
                shelfId = b.Shelf_Id,
                shelfName = b.Shelf?.Shelf_Name
            }));
        }

        // POST: api/Bins
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateBinDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shelf =
                await unitOfWork.Shelves
                    .GetByIdAsync(dto.Shelf_Id);

            if (shelf == null)
                return BadRequest("Shelf not found.");

            var name = dto.Bin_Name.Trim();

            var exists =
                await unitOfWork.Bins
                    .NameExistsInShelfAsync(
                        name,
                        dto.Shelf_Id);

            if (exists)
                return Conflict(
                    "A bin with this name already exists in this shelf.");

            var bin = new Bin
            {
                Bin_Name = name,

                Bin_Code =
                    string.IsNullOrWhiteSpace(dto.Bin_Code)
                        ? null
                        : dto.Bin_Code.Trim(),

                Bin_Description =
                    string.IsNullOrWhiteSpace(dto.Bin_Description)
                        ? null
                        : dto.Bin_Description.Trim(),

                Shelf_Id = dto.Shelf_Id,

                IsActive = true
            };

            await unitOfWork.Bins.AddAsync(bin);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Bin created successfully.",
                binId = bin.Bin_Id
            });
        }

        // PUT: api/Bins/5
        [HttpPut("Updatebyid{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateBinDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bin =
                await unitOfWork.Bins.GetByIdAsync(id);

            if (bin == null)
                return NotFound("Bin not found.");

            var name = dto.Bin_Name.Trim();

            var exists =
                await unitOfWork.Bins
                    .NameExistsInShelfAsync(
                        name,
                        bin.Shelf_Id);

            if (exists && bin.Bin_Name != name)
                return Conflict(
                    "A bin with this name already exists in this shelf.");

            bin.Bin_Name = name;

            bin.Bin_Code =
                string.IsNullOrWhiteSpace(dto.Bin_Code)
                    ? null
                    : dto.Bin_Code.Trim();

            bin.Bin_Description =
                string.IsNullOrWhiteSpace(dto.Bin_Description)
                    ? null
                    : dto.Bin_Description.Trim();

            bin.IsActive = dto.IsActive;

            unitOfWork.Bins.Update(bin);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Bin updated successfully."
            });
        }

        // DELETE: api/Bins/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bin =
                await unitOfWork.Bins.GetByIdAsync(id);

            if (bin == null)
                return NotFound("Bin not found.");

            if (bin.Stocks.Any())
                return BadRequest(
                    "Cannot delete bin because it contains stock.");

            unitOfWork.Bins.Delete(bin);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Bin deleted successfully."
            });
        }
    }
}