using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =====================================================
        // 1. GET ALL USERS
        // GET: api/Users/Getall
        // =====================================================

        [HttpGet("Getall")]
       
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await unitOfWork.User.GetAllAsync();

            var result = users.Select(u => new
            {
                userId = u.User_Id,
                userName = u.User_Name,
                email = u.User_Email,

                roleId = u.Role_Id,
                roleName = u.role != null
                    ? u.role.Role_Name
                    : null,

                createdAt = u.CreateAt,
                updatedAt = u.UpdateAt,
                loginAt = u.LoginAt
            });

            return Ok(result);
        }

        // =====================================================
        // 2. GET USER BY ID
        // GET: api/Users/GetbyId/{id}
        // =====================================================

        [HttpGet("GetbyId/{id:guid}")]
        
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                userId = user.User_Id,
                userName = user.User_Name,
                email = user.User_Email,

                roleId = user.Role_Id,
                roleName = user.role != null
                    ? user.role.Role_Name
                    : null,

                createdAt = user.CreateAt,
                updatedAt = user.UpdateAt,
                loginAt = user.LoginAt
            });
        }

        // =====================================================
        // 3. GET USER BY EMAIL
        // GET: api/Users/Getbyemail/{email}
        // =====================================================

        [HttpGet("Getbyemail/{email}")]
       
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required.");
            }

            var user = await unitOfWork.User
                .GetByEmailAsync(email.Trim());

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                userId = user.User_Id,
                userName = user.User_Name,
                email = user.User_Email,

                roleId = user.Role_Id,
                roleName = user.role != null
                    ? user.role.Role_Name
                    : null,

                createdAt = user.CreateAt,
                updatedAt = user.UpdateAt,
                loginAt = user.LoginAt
            });
        }

        // =====================================================
        // 4. UPDATE USER
        // PUT: api/Users/UpdateUser/{id}
        // =====================================================

        [HttpPut("UpdateUser/{id:guid}")]
       
        public async Task<IActionResult> UpdateUser(
            Guid id,
            UpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var role = await unitOfWork.Roles
                .GetByIdAsync(dto.Role_Id);

            if (role == null || !role.IsActive)
            {
                return BadRequest("Role not found or inactive.");
            }

            user.User_Name = dto.User_Name.Trim();
            user.Role_Id = dto.Role_Id;
            user.UpdateAt = DateTimeOffset.UtcNow;

            unitOfWork.User.Update(user);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "User updated successfully.",

                userId = user.User_Id,
                userName = user.User_Name,
                email = user.User_Email,

                roleId = user.Role_Id,
                roleName = role.Role_Name,

                updatedAt = user.UpdateAt
            });
        }

        // =====================================================
        // 5. CHANGE ROLE
        // PUT: api/Users/ChangeRole
        // =====================================================

        [HttpPut("ChangeRole")]
       
        public async Task<IActionResult> ChangeRole(
            ChangeUserRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await unitOfWork.User
                .GetByIdAsync(dto.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var role = await unitOfWork.Roles
                .GetByIdAsync(dto.Role_Id);

            if (role == null || !role.IsActive)
            {
                return BadRequest("Role not found or inactive.");
            }

            user.Role_Id = role.Role_Id;
            user.UpdateAt = DateTimeOffset.UtcNow;

            unitOfWork.User.Update(user);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "User role changed successfully.",

                userId = user.User_Id,
                userName = user.User_Name,

                roleId = role.Role_Id,
                roleName = role.Role_Name,

                updatedAt = user.UpdateAt
            });
        }

        // =====================================================
        // 6. DELETE USER
        // DELETE: api/Users/{id}
        // =====================================================

        [HttpDelete("{id:guid}")]
       
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            unitOfWork.User.Delete(user);

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest(
                    "User cannot be deleted because it is used in other records.");
            }

            return Ok(new
            {
                message = "User deleted successfully.",
                userId = id
            });
        }
    }
}