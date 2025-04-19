using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;
using TaskFlow.Requests;
using TaskFlow.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace TaskFlow.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {
            // Check if user already exist
            if (_context.Users.Any(u => u.Email == request.Email)) return BadRequest("User already exists");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
            };

            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new StatusCodeResult(200);
        }

        [HttpPost]
        public async Task<ActionResult<String>> Login([FromBody] Login request)
        {
            Console.WriteLine("Request Received");
            if (!_context.Users.Any(u => u.Email == request.Email)) return Unauthorized("Bad Credentials");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null) return BadRequest(string.Empty);

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Bad Credentials");
            }

            var token = GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                token = token
            });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            Console.WriteLine(id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) { return NotFound(); }

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Projects = u.Projects
                  
                })
                .ToListAsync();

            return Ok(users);
        }

        private string GenerateJwtToken(User user)
        {

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
