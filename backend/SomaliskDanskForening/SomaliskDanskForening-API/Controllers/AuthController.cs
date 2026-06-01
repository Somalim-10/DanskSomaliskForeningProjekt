// backend/SomaliskDanskForening/SomaliskDanskForening-API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SomaliskDanskForening_Lib.Services;
using SomaliskDanskForening_Lib.Models;
using SomaliskDanskForening_Lib.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SomaliskDanskForening_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ForeningDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ForeningDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !AuthService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Forkert brugernavn eller adgangskode");

            if (!user.IsActive)
                return Unauthorized("Brugeren er deaktiveret");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                role = user.Role,
                token = token,
                message = "Logget ind succesfuldt"
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Tillad første bruger uden adminKey
            // Hvis der allerede er brugere, kræv adminKey
            bool hasUsers = _context.Users.Any();
            
            if (hasUsers)
            {
                // Efter første bruger: kræv adminKey
                var adminKey = _configuration["Admin:RegistrationKey"];
                if (string.IsNullOrEmpty(adminKey) || request.AdminKey != adminKey)
                    return StatusCode(StatusCodes.Status403Forbidden, "❌ Kun admins kan registrere nye brugere");
            }

            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Brugernavn eksisterer allerede");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = AuthService.HashPassword(request.Password),
                Role = request.Role ?? "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "✅ Bruger oprettet succesfuldt", username = user.Username, role = user.Role });
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
        public string? Role { get; set; } = "User";
        public string? AdminKey { get; set; }
    }
}