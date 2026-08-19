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
    public class RowsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RowsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Rows
        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var rows = await unitOfWork.Rows.GetAllAsync();

            return Ok(rows.Select(r => new
            {
                rowId = r.Row_Id,
                rowName = r.Row_Name,
                rowCode = r.Row_Code,
                rowDescription = r.Row_Description,
                isActive = r.IsActive,
                roomId = r.Room_Id,
                roomName = r.Room?.Room_Name
            }));
        }

        // GET: api/Rows/5
        [HttpGet("Getbyid{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var row = await unitOfWork.Rows.GetByIdAsync(id);

            if (row == null)
                return NotFound("Row not found.");

            return Ok(new
            {
                rowId = row.Row_Id,
                rowName = row.Row_Name,
                rowCode = row.Row_Code,
                rowDescription = row.Row_Description,
                isActive = row.IsActive,
                roomId = row.Room_Id,
                roomName = row.Room?.Room_Name
            });
        }

        // GET: api/Rows/room/5
        [HttpGet("GetRowbyroomid/{roomId:int}")]
        public async Task<IActionResult> GetByRoom(int roomId)
        {
            var room = await unitOfWork.Rooms.GetByIdAsync(roomId);

            if (room == null)
                return NotFound("Room not found.");

            var rows =
                await unitOfWork.Rows.GetByRoomIdAsync(roomId);

            return Ok(rows.Select(row=>
            new
            {
                rowId = row.Row_Id,
                rowName = row.Row_Name,
                rowCode = row.Row_Code,
                rowDescription = row.Row_Description,
                isActive = row.IsActive,
                roomId = row.Room_Id,
                roomName = row.Room?.Room_Name

            }
             ));
        }

        // POST: api/Rows
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRowDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room =
                await unitOfWork.Rooms.GetByIdAsync(dto.Room_Id);

            if (room == null)
                return BadRequest("Room not found.");

            var name = dto.Row_Name.Trim();

            var exists =
                await unitOfWork.Rows
                    .NameExistsInRoomAsync(
                        name,
                        dto.Room_Id);

            if (exists)
                return Conflict(
                    "A row with this name already exists in this room.");

            var row = new Row
            {
                Row_Name = name,

                Row_Code =
                    string.IsNullOrWhiteSpace(dto.Row_Code)
                        ? null
                        : dto.Row_Code.Trim(),

                Row_Description =
                    string.IsNullOrWhiteSpace(dto.Row_Description)
                        ? null
                        : dto.Row_Description.Trim(),

                Room_Id = dto.Room_Id,

                IsActive = true
            };

            await unitOfWork.Rows.AddAsync(row);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Row created successfully.",
                rowId = row.Row_Id
            });
        }

        // PUT: api/Rows/5
        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateRowDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var row =
                await unitOfWork.Rows.GetByIdAsync(id);

            if (row == null)
                return NotFound("Row not found.");

            var name = dto.Row_Name.Trim();

            var exists =
                await unitOfWork.Rows
                    .NameExistsInRoomAsync(
                        name,
                        row.Room_Id);

            if (exists && row.Row_Name != name)
                return Conflict(
                    "A row with this name already exists in this room.");

            row.Row_Name = name;

            row.Row_Code =
                string.IsNullOrWhiteSpace(dto.Row_Code)
                    ? null
                    : dto.Row_Code.Trim();

            row.Row_Description =
                string.IsNullOrWhiteSpace(dto.Row_Description)
                    ? null
                    : dto.Row_Description.Trim();

            row.IsActive = dto.IsActive;

            unitOfWork.Rows.Update(row);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Row updated successfully."
            });
        }

        // DELETE: api/Rows/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var row =
                await unitOfWork.Rows.GetByIdAsync(id);

            if (row == null)
                return NotFound("Row not found.");

            if (row.Shelves.Any())
                return BadRequest(
                    "Cannot delete row because it contains shelves.");

            unitOfWork.Rows.Delete(row);
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Row deleted successfully."
            });
        }
    }
}