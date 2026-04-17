// backend/SomaliskDanskForening/SomaliskDanskForening-API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using SomaliskDanskForening_Lib.Services;
using SomaliskDanskForening_Lib.Models;
using SomaliskDanskForening_Lib.Data;

namespace SomaliskDanskForening_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ForeningDbContext _context;

        public AuthController(ForeningDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !AuthService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Forkert brugernavn eller adgangskode");

            if (!user.IsActive)
                return Unauthorized("Brugeren er deaktiveret");

            // Returner user info + token (JWT)
            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                role = user.Role,
                message = "Logget ind succesfuldt"
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Brugernavn eksisterer allerede");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = AuthService.HashPassword(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Bruger oprettet succesfuldt");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}