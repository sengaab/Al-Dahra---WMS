using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET: api/roles
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDTO>>>
            GetRoles()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();

            var response = roles.Select(r => new RoleResponseDTO
            {
                RoleId = r.RoleId,
                Name = r.Name,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                UsersCount = r.Users?.Count ?? 0
            }).ToList();

            return Ok(response);
        }


        // =====================================================
        // GET: api/roles/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleResponseDTO>>
            GetRole(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found."
                });
            }

            var response = new RoleResponseDTO
            {
                RoleId = role.RoleId,
                Name = role.Name,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                UsersCount = role.Users?.Count ?? 0
            };

            return Ok(response);
        }


        // =====================================================
        // POST: api/roles
        // =====================================================

        [HttpPost]
        public async Task<ActionResult<RoleResponseDTO>>
            CreateRole(
                [FromBody] CreateRoleDTO dto)
        {
            // -------------------------------------------------
            // Check duplicate name
            // -------------------------------------------------

            var exists =
                await _unitOfWork.Roles.NameExistsAsync(dto.Name);

            if (exists)
            {
                return Conflict(new
                {
                    message = "A role with this name already exists."
                });
            }


            // -------------------------------------------------
            // Create
            // -------------------------------------------------

            var role = new Role
            {
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };


            await _unitOfWork.Roles.AddAsync(role);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Response
            // -------------------------------------------------

            var response = new RoleResponseDTO
            {
                RoleId = role.RoleId,
                Name = role.Name,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                UsersCount = 0
            };

            return CreatedAtAction(
                nameof(GetRole),
                new { id = role.RoleId },
                response);
        }


        // =====================================================
        // PUT: api/roles/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RoleResponseDTO>>
            UpdateRole(
                int id,
                [FromBody] UpdateRoleDTO dto)
        {
            // -------------------------------------------------
            // Get role
            // -------------------------------------------------

            var role =
                await _unitOfWork.Roles.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found."
                });
            }


            // -------------------------------------------------
            // Check duplicate name
            // -------------------------------------------------

            var exists =
                await _unitOfWork.Roles.NameExistsAsync(
                    dto.Name,
                    id);

            if (exists)
            {
                return Conflict(new
                {
                    message = "A role with this name already exists."
                });
            }


            // -------------------------------------------------
            // Update
            // -------------------------------------------------

            role.Name = dto.Name.Trim();
            role.IsActive = dto.IsActive;
            role.UpdatedAt = DateTimeOffset.UtcNow;


            _unitOfWork.Roles.Update(role);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Response
            // -------------------------------------------------

            var updatedRole =
                await _unitOfWork.Roles.GetByIdAsync(id);

            if (updatedRole == null)
            {
                return StatusCode(500, new
                {
                    message =
                        "Role was updated but could not be retrieved."
                });
            }


            var response = new RoleResponseDTO
            {
                RoleId = updatedRole.RoleId,
                Name = updatedRole.Name,
                IsActive = updatedRole.IsActive,
                CreatedAt = updatedRole.CreatedAt,
                UpdatedAt = updatedRole.UpdatedAt,
                UsersCount =
                    updatedRole.Users?.Count ?? 0
            };

            return Ok(response);
        }


        // =====================================================
        // DELETE: api/roles/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            // -------------------------------------------------
            // Get role
            // -------------------------------------------------

            var role =
                await _unitOfWork.Roles.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found."
                });
            }


            // -------------------------------------------------
            // Delete
            // -------------------------------------------------

            _unitOfWork.Roles.Delete(role);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Role deleted successfully.",
                roleId = id
            });
        }


        // =====================================================
        // GET:
        // api/roles/{id}/permissions
        // =====================================================

        [HttpGet("{id:int}/permissions")]
        public async Task<ActionResult<RolePermissionsDTO>>
            GetRolePermissions(int id)
        {
            var role =
                await _unitOfWork.Roles
                    .GetRoleWithPermissionsAsync(id);

            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found."
                });
            }


            var response = new RolePermissionsDTO
            {
                RoleId = role.RoleId,
                RoleName = role.Name,

                // Replace this when Permission entity
                // is added to the project.
                Permissions = new List<string>()
            };

            return Ok(response);
        }


        // =====================================================
        // PUT:
        // api/roles/{id}/permissions
        // =====================================================

        [HttpPut("{id:int}/permissions")]
        public async Task<IActionResult>
            UpdateRolePermissions(
                int id,
                [FromBody] UpdateRolePermissionsDTO dto)
        {
            // -------------------------------------------------
            // Check role
            // -------------------------------------------------

            var role =
                await _unitOfWork.Roles.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found."
                });
            }


            // -------------------------------------------------
            // Update permissions
            // -------------------------------------------------

            await _unitOfWork.Roles.UpdatePermissionsAsync(
                id,
                dto.Permissions);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Role permissions updated successfully.",
                roleId = id,
                permissions = dto.Permissions
            });
        }
    }
}