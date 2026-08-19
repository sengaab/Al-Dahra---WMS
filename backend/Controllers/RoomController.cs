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
    public class RoomsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RoomsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET ALL ROOMS
        // GET: api/Rooms
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms =
                await unitOfWork.Rooms.GetAllAsync();

            var result = rooms.Select(r => new
            {
                roomId = r.Room_Id,

                roomName = r.Room_Name,

                roomCode = r.Room_Code,

                roomDescription = r.Room_Description,

                isActive = r.IsActive,

                warehouseId = r.Warehouse_Id,

                warehouseName =
                    r.Warehouse?.Warehouse_Name
            });

            return Ok(result);
        }


        // =====================================================
        // GET ROOM BY ID
        // GET: api/Rooms/{id}
        // =====================================================

        [HttpGet("Getbyid{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room =
                await unitOfWork.Rooms
                    .GetByIdAsync(id);

            if (room == null)
            {
                return NotFound(
                    "Room not found.");
            }

            return Ok(new
            {
                roomId = room.Room_Id,

                roomName = room.Room_Name,

                roomCode = room.Room_Code,

                roomDescription =
                    room.Room_Description,

                isActive = room.IsActive,

                warehouseId =
                    room.Warehouse_Id,

                warehouseName =
                    room.Warehouse?.Warehouse_Name,

                rowsCount =
                    room.Rows.Count
            });
        }


        // =====================================================
        // GET ROOMS BY WAREHOUSE
        // GET: api/Rooms/warehouse/{warehouseId}
        // =====================================================

        [HttpGet("GETROOMSBYWAREHOUSE/{warehouseId:int}")]
        public async Task<IActionResult> GetRoomsByWarehouse(
            int warehouseId)
        {
            // Check warehouse exists
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(warehouseId);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            var rooms =
                await unitOfWork.Rooms
                    .GetByWarehouseIdAsync(
                        warehouseId);

            var result = rooms.Select(r => new
            {
                roomId = r.Room_Id,

                roomName = r.Room_Name,

                roomCode = r.Room_Code,

                roomDescription =
                    r.Room_Description,

                isActive = r.IsActive,

                warehouseId =
                    r.Warehouse_Id
            });

            return Ok(result);
        }


        // =====================================================
        // CREATE ROOM
        // POST: api/Rooms
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom(
            CreateRoomDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // CHECK WAREHOUSE
            // =================================================

            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(
                        dto.Warehouse_Id);

            if (warehouse == null)
            {
                return BadRequest(
                    "Warehouse not found.");
            }


            // =================================================
            // CHECK DUPLICATE ROOM NAME
            // =================================================

            var roomName =
                dto.Room_Name.Trim();

            var nameExists =
                await unitOfWork.Rooms
                    .NameExistsInWarehouseAsync(
                        roomName,
                        dto.Warehouse_Id);

            if (nameExists)
            {
                return Conflict(
                    "A room with this name already exists in this warehouse.");
            }


            // =================================================
            // CREATE ROOM
            // =================================================

            var room = new Room
            {
                Room_Name =
                    roomName,

                Room_Code =
                    string.IsNullOrWhiteSpace(
                        dto.Room_Code)
                        ? null
                        : dto.Room_Code.Trim(),

                Room_Description =
                    string.IsNullOrWhiteSpace(
                        dto.Room_Description)
                        ? null
                        : dto.Room_Description.Trim(),

                Warehouse_Id =
                    dto.Warehouse_Id,

                IsActive = true
            };


            await unitOfWork.Rooms
                .AddAsync(room);

            await unitOfWork.SaveAsync();


            // =================================================
            // RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Room created successfully.",

                roomId =
                    room.Room_Id,

                roomName =
                    room.Room_Name,

                roomCode =
                    room.Room_Code,

                roomDescription =
                    room.Room_Description,

                warehouseId =
                    room.Warehouse_Id,

                isActive =
                    room.IsActive
            });
        }


        // =====================================================
        // UPDATE ROOM
        // PUT: api/Rooms/{id}
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> UpdateRoom(
            int id,
            UpdateRoomDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // GET ROOM
            // =================================================

            var room =
                await unitOfWork.Rooms
                    .GetByIdAsync(id);

            if (room == null)
            {
                return NotFound(
                    "Room not found.");
            }


            // =================================================
            // CHECK DUPLICATE ROOM NAME
            // =================================================

            var roomName =
                dto.Room_Name.Trim();

            var nameExists =
                await unitOfWork.Rooms
                    .NameExistsInWarehouseAsync(
                        roomName,
                        room.Warehouse_Id);

            if (nameExists &&
                room.Room_Name != roomName)
            {
                return Conflict(
                    "A room with this name already exists in this warehouse.");
            }


            // =================================================
            // UPDATE ROOM
            // =================================================

            room.Room_Name =
                roomName;

            room.Room_Code =
                string.IsNullOrWhiteSpace(
                    dto.Room_Code)
                    ? null
                    : dto.Room_Code.Trim();

            room.Room_Description =
                string.IsNullOrWhiteSpace(
                    dto.Room_Description)
                    ? null
                    : dto.Room_Description.Trim();

            room.IsActive =
                dto.IsActive;


            unitOfWork.Rooms
                .Update(room);

            await unitOfWork.SaveAsync();


            // =================================================
            // RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Room updated successfully.",

                roomId =
                    room.Room_Id,

                roomName =
                    room.Room_Name,

                roomCode =
                    room.Room_Code,

                roomDescription =
                    room.Room_Description,

                warehouseId =
                    room.Warehouse_Id,

                isActive =
                    room.IsActive
            });
        }


        // =====================================================
        // DELETE ROOM
        // DELETE: api/Rooms/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRoom(
            int id)
        {
            var room =
                await unitOfWork.Rooms
                    .GetByIdAsync(id);

            if (room == null)
            {
                return NotFound(
                    "Room not found.");
            }


            // =================================================
            // DON'T DELETE IF ROOM HAS ROWS
            // =================================================

            if (room.Rows.Any())
            {
                return BadRequest(
                    "Room cannot be deleted because it contains rows.");
            }


            // =================================================
            // DELETE
            // =================================================

            unitOfWork.Rooms
                .Delete(room);

            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Room deleted successfully.",

                roomId =
                    id
            });
        }
    }
}