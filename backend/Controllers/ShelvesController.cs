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
    public class ShelvesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ShelvesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Shelves
        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var shelves =
                await unitOfWork.Shelves.GetAllAsync();

            return Ok(shelves.Select(s => new
            {
                shelfId = s.Shelf_Id,
                shelfName = s.Shelf_Name,
                shelfCode = s.Shelf_Code,
                shelfDescription = s.Shelf_Description,
                isActive = s.IsActive,
                rowId = s.Row_Id,
                rowName = s.Row?.Row_Name
            }));
        }

        // GET: api/Shelves/5
        [HttpGet("Getbyid{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shelf =
                await unitOfWork.Shelves.GetByIdAsync(id);

            if (shelf == null)
                return NotFound("Shelf not found.");

            return Ok(new
            {
                shelfId = shelf.Shelf_Id,
                shelfName = shelf.Shelf_Name,
                shelfCode = shelf.Shelf_Code,
                shelfDescription = shelf.Shelf_Description,
                isActive = shelf.IsActive,
                rowId = shelf.Row_Id,
                rowName = shelf.Row?.Row_Name
            });
        }

        // GET: api/Shelves/row/5
        [HttpGet("Getshelvesbyrow/{rowId:int}")]
        public async Task<IActionResult> GetByRow(int rowId)
        {
            var row =
                await unitOfWork.Rows.GetByIdAsync(rowId);

            if (row == null)
                return NotFound("Row not found.");

            var shelves =
                await unitOfWork.Shelves
                    .GetByRowIdAsync(rowId);

            return Ok(shelves.Select(s => new
            {
                shelfId = s.Shelf_Id,
                shelfName = s.Shelf_Name,
                shelfCode = s.Shelf_Code,
                shelfDescription = s.Shelf_Description,
                isActive = s.IsActive,
                rowId = s.Row_Id,
                rowName = s.Row?.Row_Name
            }));
        }

        // POST: api/Shelves
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateShelfDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var row =
                await unitOfWork.Rows.GetByIdAsync(dto.Row_Id);

            if (row == null)
                return BadRequest("Row not found.");

            var name = dto.Shelf_Name.Trim();

            var exists =
                await unitOfWork.Shelves
                    .NameExistsInRowAsync(
                        name,
                        dto.Row_Id);

            if (exists)
                return Conflict(
                    "A shelf with this name already exists in this row.");

            var shelf = new Shelf
            {
                Shelf_Name = name,

                Shelf_Code =
                    string.IsNullOrWhiteSpace(dto.Shelf_Code)
                        ? null
                        : dto.Shelf_Code.Trim(),

                Shelf_Description =
                    string.IsNullOrWhiteSpace(dto.Shelf_Description)
                        ? null
                        : dto.Shelf_Description.Trim(),

                Row_Id = dto.Row_Id,

                IsActive = true
            };

            await unitOfWork.Shelves.AddAsync(shelf);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Shelf created successfully.",
                shelfId = shelf.Shelf_Id
            });
        }

        // PUT: api/Shelves/5
        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateShelfDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shelf =
                await unitOfWork.Shelves.GetByIdAsync(id);

            if (shelf == null)
                return NotFound("Shelf not found.");

            var name = dto.Shelf_Name.Trim();

            var exists =
                await unitOfWork.Shelves
                    .NameExistsInRowAsync(
                        name,
                        shelf.Row_Id);

            if (exists && shelf.Shelf_Name != name)
                return Conflict(
                    "A shelf with this name already exists in this row.");

            shelf.Shelf_Name = name;

            shelf.Shelf_Code =
                string.IsNullOrWhiteSpace(dto.Shelf_Code)
                    ? null
                    : dto.Shelf_Code.Trim();

            shelf.Shelf_Description =
                string.IsNullOrWhiteSpace(dto.Shelf_Description)
                    ? null
                    : dto.Shelf_Description.Trim();

            shelf.IsActive = dto.IsActive;

            unitOfWork.Shelves.Update(shelf);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Shelf updated successfully."
            });
        }

        // DELETE: api/Shelves/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shelf =
                await unitOfWork.Shelves.GetByIdAsync(id);

            if (shelf == null)
                return NotFound("Shelf not found.");

            if (shelf.Bins.Any())
                return BadRequest(
                    "Cannot delete shelf because it contains bins.");

            unitOfWork.Shelves.Delete(shelf);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Shelf deleted successfully."
            });
        }
    }
}