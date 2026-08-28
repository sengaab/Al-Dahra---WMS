using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET: api/users
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.User.GetAllAsync();

            var result = users.Select(u => new UserResponseDTO
            {
                UserId = u.UserId,
                EmployeeCode = u.EmployeeCode,
                Name = u.Name,
                Email = u.Email,

                RoleId = u.RoleId,
                RoleName = u.Role?.Name,

                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.Name,

                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt

            }).ToList();

            return Ok(result);
        }


        // =====================================================
        // GET: api/users/{id}
        // =====================================================

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            var result = new UserResponseDTO
            {
                UserId = user.UserId,
                EmployeeCode = user.EmployeeCode,
                Name = user.Name,
                Email = user.Email,

                RoleId = user.RoleId,
                RoleName = user.Role?.Name,

                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name,

                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(result);
        }


        // =====================================================
        // POST: api/users
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Check email
            var emailExists = await _unitOfWork.User
                .EmailExistsAsync(dto.Email);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "A user with this email already exists."
                });
            }


            // Check employee code
            if (!string.IsNullOrWhiteSpace(dto.EmployeeCode))
            {
                var employeeCodeExists =
                    await _unitOfWork.User.EmployeeCodeExistsAsync(
                        dto.EmployeeCode);

                if (employeeCodeExists)
                {
                    return Conflict(new
                    {
                        message = "A user with this employee code already exists."
                    });
                }
            }


            // Check Role
            var role = await _unitOfWork.Roles
                .GetByIdAsync(dto.RoleId);

            if (role == null)
            {
                return BadRequest(new
                {
                    message = "Role not found."
                });
            }


            // Check Department
            if (dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Department
                    .GetByIdAsync(dto.DepartmentId.Value);

                if (department == null)
                {
                    return BadRequest(new
                    {
                        message = "Department not found."
                    });
                }
            }


            // Create User
            var user = new User
            {
                UserId = Guid.NewGuid(),

                EmployeeCode = dto.EmployeeCode,

                Name = dto.Name,

                Email = dto.Email,

                RoleId = dto.RoleId,

                DepartmentId = dto.DepartmentId,

                IsActive = dto.IsActive,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow
            };


            await _unitOfWork.User.AddAsync(user);

            await _unitOfWork.SaveAsync();


            // Load navigation properties
            var createdUser =
                await _unitOfWork.User.GetByIdAsync(user.UserId);


            var result = new UserResponseDTO
            {
                UserId = createdUser!.UserId,

                EmployeeCode = createdUser.EmployeeCode,

                Name = createdUser.Name,

                Email = createdUser.Email,

                RoleId = createdUser.RoleId,

                RoleName = createdUser.Role?.Name,

                DepartmentId = createdUser.DepartmentId,

                DepartmentName = createdUser.Department?.Name,

                IsActive = createdUser.IsActive,

                CreatedAt = createdUser.CreatedAt,

                UpdatedAt = createdUser.UpdatedAt
            };


            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.UserId },
                result);
        }


        // =====================================================
        // PUT: api/users/{id}
        // =====================================================

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(
            Guid id,
            [FromBody] UpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await _unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            // Check email
            var emailExists =
                await _unitOfWork.User.EmailExistsAsync(
                    dto.Email,
                    id);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "A user with this email already exists."
                });
            }


            // Check employee code
            if (!string.IsNullOrWhiteSpace(dto.EmployeeCode))
            {
                var employeeCodeExists =
                    await _unitOfWork.User.EmployeeCodeExistsAsync(
                        dto.EmployeeCode,
                        id);

                if (employeeCodeExists)
                {
                    return Conflict(new
                    {
                        message = "A user with this employee code already exists."
                    });
                }
            }


            // Check Role
            var role = await _unitOfWork.Roles
                .GetByIdAsync(dto.RoleId);

            if (role == null)
            {
                return BadRequest(new
                {
                    message = "Role not found."
                });
            }


            // Check Department
            if (dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Department
                    .GetByIdAsync(dto.DepartmentId.Value);

                if (department == null)
                {
                    return BadRequest(new
                    {
                        message = "Department not found."
                    });
                }
            }


            // Update
            user.EmployeeCode = dto.EmployeeCode;

            user.Name = dto.Name;

            user.Email = dto.Email;

            user.RoleId = dto.RoleId;

            user.DepartmentId = dto.DepartmentId;

            user.IsActive = dto.IsActive;

            user.UpdatedAt = DateTimeOffset.UtcNow;


            _unitOfWork.User.Update(user);

            await _unitOfWork.SaveAsync();


            // Get updated user with navigation properties
            var updatedUser =
                await _unitOfWork.User.GetByIdAsync(id);


            var result = new UserResponseDTO
            {
                UserId = updatedUser!.UserId,

                EmployeeCode = updatedUser.EmployeeCode,

                Name = updatedUser.Name,

                Email = updatedUser.Email,

                RoleId = updatedUser.RoleId,

                RoleName = updatedUser.Role?.Name,

                DepartmentId = updatedUser.DepartmentId,

                DepartmentName = updatedUser.Department?.Name,

                IsActive = updatedUser.IsActive,

                CreatedAt = updatedUser.CreatedAt,

                UpdatedAt = updatedUser.UpdatedAt
            };


            return Ok(result);
        }


        // =====================================================
        // DELETE: api/users/{id}
        // =====================================================

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            _unitOfWork.User.Delete(user);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "User deleted successfully.",
                userId = id
            });
        }


        // =====================================================
        // GET: api/users/{id}/activity
        // =====================================================

        [HttpGet("{id:guid}/activity")]
        public async Task<IActionResult> GetUserActivity(Guid id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            var activity =
                await _unitOfWork.User.GetActivityAsync(id);


            var result = activity.Select(a => new UserActivityDTO
            {
                AuditLogId = a.AuditLogId,

                EntityType = a.EntityType,

                EntityId = a.EntityId,

                Action = a.Action,

                OldValue = a.OldValue,

                NewValue = a.NewValue,

                CreatedAt = a.CreatedAt

            }).ToList();


            return Ok(result);
        }


        // =====================================================
        // GET: api/users/{id}/permissions
        // =====================================================

        [HttpGet("{id:guid}/permissions")]
        public async Task<IActionResult> GetUserPermissions(Guid id)
        {
            var user =
                await _unitOfWork.User
                    .GetUserWithPermissionsAsync(id);


            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            var result = new UserPermissionsDTO
            {
                UserId = user.UserId,

                RoleId = user.RoleId,

                RoleName = user.Role?.Name,

                Permissions = new List<string>()
            };


            /*
             * Your current User/Role models that you provided
             * do not show a Permission collection.
             *
             * Therefore we leave Permissions empty here until
             * the Role -> Permission relationship is defined.
             */


            return Ok(result);
        }
    }
}