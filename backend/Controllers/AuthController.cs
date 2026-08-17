using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using whm.DTOs;
using whm.Models;
using whm.Services;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataBaseContext db;
        private readonly IConfiguration configuration;
        private readonly PasswordHasher<Users> passwordHasher;
        private readonly IEmailService emailService;

        public AuthController(
            DataBaseContext db,
            IConfiguration configuration,
            IEmailService emailService)
        {
            this.db = db;
            this.configuration = configuration;
            this.emailService = emailService;

            passwordHasher = new PasswordHasher<Users>();
        }


        // =========================
        // Register
        // =========================

        [HttpPost("Register")]
        public async Task<IActionResult> Register(
            AuthRegisterDTO register)
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
                return BadRequest("Email already exists.");
            }


            // =========================
            // Get Employee Role
            // =========================

            var employeeRole = db.Roles
                .FirstOrDefault(r =>
                    r.Role_Name == "Employee" &&
                    r.IsActive);

            if (employeeRole == null)
            {
                return BadRequest(
                    "Employee role does not exist."
                );
            }


            // =========================
            // Generate Verification Code
            // =========================

            var verificationCode =
                Random.Shared
                    .Next(100000, 1000000)
                    .ToString();


            // =========================
            // Create User
            // =========================

            var user = new Users
            {
                User_Id = Guid.NewGuid(),

                User_Name = register.FullName,

                User_Email = register.Email,

                Role_Id = employeeRole.Role_Id,

                Status = UserStatus.Inactive,

                EmailConfirmed = false,

                EmailVerificationCode = verificationCode,

                EmailVerificationExpiresAt =
                    DateTimeOffset.UtcNow.AddMinutes(10),

                CreateAt = DateTimeOffset.UtcNow,

                UpdateAt = DateTimeOffset.UtcNow
            };


            // =========================
            // Hash Password
            // =========================

            user.User_Password =
                passwordHasher.HashPassword(
                    user,
                    register.Password
                );


            db.Users.Add(user);

            db.SaveChanges();


            // =========================
            // Send Verification Email
            // =========================

            await emailService.SendVerificationCodeAsync(
                user.User_Email,
                verificationCode
            );


            return Ok(new
            {
                message =
                    "Registration successful. Please check your email for the verification code.",

                userId = user.User_Id,

                email = user.User_Email
            });
        }


        // =========================
        // Verify Email
        // =========================

        [HttpPost("VerifyEmail")]
        public IActionResult VerifyEmail(
            VerifyEmailDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = db.Users
                .FirstOrDefault(
                    x => x.User_Email == dto.Email
                );


            if (user == null)
            {
                return NotFound(
                    "User not found."
                );
            }


            // Already verified
            if (user.EmailConfirmed)
            {
                return BadRequest(
                    "Email is already verified."
                );
            }


            // Check Code
            if (user.EmailVerificationCode != dto.Code)
            {
                return BadRequest(
                    "Invalid verification code."
                );
            }


            // Check Expiration
            if (
                user.EmailVerificationExpiresAt == null ||
                user.EmailVerificationExpiresAt
                    < DateTimeOffset.UtcNow
            )
            {
                return BadRequest(
                    "Verification code expired."
                );
            }


            // =========================
            // Verify User
            // =========================

            user.EmailConfirmed = true;

            user.EmailVerificationCode = null;

            user.EmailVerificationExpiresAt = null;

            user.Status = UserStatus.Active;

            user.UpdateAt = DateTimeOffset.UtcNow;


            db.SaveChanges();


            return Ok(new
            {
                message =
                    "Email verified successfully. You can login now."
            });
        }


        // =========================
        // Login
        // =========================

        [HttpPost("Login")]
        public IActionResult Login(
            AuthLoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Get User + Role
            var user = db.Users
                .Include(u => u.role)
                .FirstOrDefault(
                    x => x.User_Email == login.Email
                );


            if (user == null)
            {
                return Unauthorized(
                    "Invalid Email or Password."
                );
            }


            // =========================
            // Check Email Verification
            // =========================

            if (!user.EmailConfirmed)
            {
                return Unauthorized(
                    "Please verify your email first."
                );
            }


            // =========================
            // Check Account Status
            // =========================

            if (user.Status != UserStatus.Active)
            {
                return Unauthorized(
                    "User account is not active."
                );
            }


            // =========================
            // Verify Password
            // =========================

            var passwordResult =
                passwordHasher.VerifyHashedPassword(
                    user,
                    user.User_Password,
                    login.Password
                );


            if (
                passwordResult ==
                PasswordVerificationResult.Failed
            )
            {
                return Unauthorized(
                    "Invalid Email or Password."
                );
            }


            // =========================
            // Update Login Time
            // =========================

            user.LoginAt =
                DateTimeOffset.UtcNow;

            user.UpdateAt =
                DateTimeOffset.UtcNow;

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

            var jwtKey =
                configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                return StatusCode(
                    500,
                    "JWT secret key is not configured."
                );
            }


            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)
                );


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );


            // =========================
            // Create Token
            // =========================

            var token =
                new JwtSecurityToken(
                    claims: claims,

                    expires:
                        DateTime.UtcNow.AddHours(1),

                    signingCredentials:
                        credentials
                );


            var jwt =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);


            // =========================
            // Response
            // =========================

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