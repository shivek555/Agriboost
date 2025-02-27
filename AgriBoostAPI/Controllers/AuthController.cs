using AgriBoostAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgriBoostAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterUser(request.Name, request.Email, request.Password, request.UserType);
            if (user == null) return BadRequest("Email already exists.");
            return Ok(new { message = "User registered successfully!" });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateUser(request.Email, request.Password);
            if (token == null) return Unauthorized("Invalid credentials.");
            return Ok(new { token });
        }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } // "Buyer", "Seller", "Farmer"
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
