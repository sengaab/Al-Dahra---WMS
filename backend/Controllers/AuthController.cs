using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst("sub")?.Value,
                Email = User.FindFirst("email")?.Value
            });
        }
    }
}