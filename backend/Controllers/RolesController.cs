using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL ROLES
        // GET: api/Roles
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles =
                await unitOfWork.Roles.GetAllAsync();

            var result = roles.Select(r => new
            {
                roleId = r.Role_Id,

                roleName = r.Role_Name,

                description = r.Role_Description,

                isActive = r.IsActive,

                createdAt = r.CreateAt,

                updatedAt = r.UpdateAt
            });

            return Ok(result);
        }


        // =====================================================
        // GET ROLE BY ID
        // GET: api/Roles/{id}
        // =====================================================

        [HttpGet("GetbyId/{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role =
                await unitOfWork.Roles.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(
                    "Role not found.");
            }

            return Ok(new
            {
                roleId = role.Role_Id,

                roleName = role.Role_Name,

                description = role.Role_Description,

                isActive = role.IsActive,

                createdAt = role.CreateAt,

                updatedAt = role.UpdateAt
            });
        }


        // =====================================================
        // CREATE ROLE
        // POST: api/Roles
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRole(
            CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleName =
                dto.Role_Name.Trim();

            if (await unitOfWork.Roles
                .NameExistsAsync(roleName))
            {
                return Conflict(
                    "Role already exists.");
            }

            var role = new Role
            {
                Role_Name = roleName,

                Role_Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim(),

                IsActive = true,

                CreateAt =
                    DateTimeOffset.UtcNow,

              
            }; 

            await unitOfWork.Roles
                .AddAsync(role);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Role created successfully.",

                roleId =
                    role.Role_Id,

                roleName =
                    role.Role_Name,

                description =
                    role.Role_Description,

                isActive =
                    role.IsActive,

                createdAt =
                    role.CreateAt
            });
        }


        // =====================================================
        // UPDATE ROLE
        // PUT: api/Roles/{id}
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> UpdateRole(
            int id,
            UpdateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role =
                await unitOfWork.Roles
                    .GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(
                    "Role not found.");
            }

            var roleName =
                dto.Role_Name.Trim();

            var existingRole =
                await unitOfWork.Roles
                    .GetByNameAsync(roleName);

            if (existingRole != null &&
                existingRole.Role_Id != id)
            {
                return Conflict(
                    "Role name already exists.");
            }

            role.Role_Name =
                roleName;

            role.Role_Description =
                string.IsNullOrWhiteSpace(dto.Description)
                    ? null
                    : dto.Description.Trim();

            role.IsActive =
                dto.IsActive;

            role.UpdateAt =
                DateTimeOffset.UtcNow;

            unitOfWork.Roles.Update(role);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Role updated successfully.",

                roleId =
                    role.Role_Id,

                roleName =
                    role.Role_Name,

                description =
                    role.Role_Description,

                isActive =
                    role.IsActive,

                updatedAt =
                    role.UpdateAt
            });
        }


        // =====================================================
        // DELETE ROLE
        // DELETE: api/Roles/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role =
                await unitOfWork.Roles
                    .GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(
                    "Role not found.");
            }

            if (role.User.Any())
            {
                return BadRequest(
                    "Role cannot be deleted because it is assigned to users.");
            }

            unitOfWork.Roles.Delete(role);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Role deleted successfully.",

                roleId =
                    id
            });
        }
    }
}