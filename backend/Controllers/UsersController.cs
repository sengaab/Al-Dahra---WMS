using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using Microsoft.AspNetCore.Authorization;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataBaseContext db;
        private readonly PasswordHasher<Users> passwordHasher;

        public UsersController(DataBaseContext db)
        {
            this.db = db;
            passwordHasher = new PasswordHasher<Users>();
        }
        [HttpPut("ChangeRole")]
        [Authorize(Roles = "Admin")] 
        public IActionResult ChangeRole(ChangeUserRoleDTO dto)
        {
            // Find user
            var user = db.Users
                .FirstOrDefault(u => u.User_Id == dto.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Find new role
            var role = db.Roles
                .FirstOrDefault(r => r.Role_Id == dto.Role_Id && r.IsActive);

            if (role == null)
            {
                return BadRequest("Role not found.");
            }

            // Change role
            user.Role_Id = role.Role_Id;

            db.SaveChanges();

            return Ok(new
            {
                message = "User role changed successfully.",
                userId = user.User_Id,
                userName = user.User_Name,
                roleId = role.Role_Id,
                roleName = role.Role_Name
            });
        }
    }
}
