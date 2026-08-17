using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using whm.Models;
using whm.DTOs;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly PasswordHasher<Users> passwordHasher;

        private readonly DataBaseContext db;
        private readonly IConfiguration configuration;

        public AuthController(
            DataBaseContext db,
            IConfiguration configuration)
        {
            passwordHasher = new PasswordHasher<Users>();
            this.db = db;
            this.configuration = configuration;
        }
        
        


        // =========================
        // Register
        // =========================

        [HttpPost("Register")]
        public IActionResult Register(AuthRegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            var existingUser = db.Users
                .FirstOrDefault(x => x.User_Email == register.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            // Find Employee role
            var employeeRole = db.Roles
                .FirstOrDefault(r => r.Role_Name == "Employee");

            if (employeeRole == null)
            {
                return BadRequest(
                    "Employee role does not exist. Please create it first."
                );
            }

            var user = new Users
            {
                User_Id = Guid.NewGuid(),

                User_Name = register.FullName,

                User_Email = register.Email,

                Status = UserStatus.Active,

                Role_Id = employeeRole.Role_Id,

                CreateAt = DateTimeOffset.UtcNow,

                UpdateAt = DateTimeOffset.UtcNow
            };

            // Hash Password
            user.User_Password = passwordHasher.HashPassword(
                user,
                register.Password
            );

            db.Users.Add(user);

            db.SaveChanges();

            return Ok(new
            {
                message = "User registered successfully",

                userId = user.User_Id,

                userName = user.User_Name,

                email = user.User_Email,

                role = employeeRole.Role_Name
            });
        }


        // =========================
        // Login
        // =========================

        [HttpPost("Login")]
        public IActionResult Login(AuthLoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get user + Role
            var user = db.Users
                .Include(u => u.role)
                .FirstOrDefault(x => x.User_Email == login.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            // Check account status
            if (user.Status != UserStatus.Active)
            {
                return Unauthorized("User account is not active");
            }

            // Verify password
            var passwordResult = passwordHasher.VerifyHashedPassword(
                user,
                user.User_Password,
                login.Password
            );

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid Email or Password");
            }

            // Update last login
            user.LoginAt = DateTimeOffset.UtcNow;
            user.UpdateAt = DateTimeOffset.UtcNow;

            db.SaveChanges();


            // =========================
            // JWT Claims
            // =========================

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.User_Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.User_Name
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.User_Email
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.role.Role_Name
                )
            };


            // =========================
            // JWT Key
            // =========================

            var jwtKey = configuration["Jwt:Key"];

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


            // =========================
            // Create Token
            // =========================

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );


            var jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);


            return Ok(new
            {
                message = "Login successful",

                token = jwt,

                user = new
                {
                    id = user.User_Id,
                    name = user.User_Name,
                    email = user.User_Email,
                    role = user.role.Role_Name
                }
            });
        }
    }
}