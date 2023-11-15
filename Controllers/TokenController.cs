using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobFDB.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobFDB.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MobDbContext _context;

        public TokenController(IConfiguration config, MobDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData != null && !string.IsNullOrEmpty(_userData.EmailAddress) && !string.IsNullOrEmpty(_userData.Password))
            {
                var user = await GetUser(_userData.EmailAddress, _userData.Password);

                if (user != null)
                {
                    // create claims details based on the user information
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName),
                        new Claim("EmailAddress", user.EmailAddress),
                        new Claim("MobileNumber", user.MobileNumber),
                        /*new Claim(ClaimTypes.Role, "User"),*/
                        new Claim(ClaimTypes.Role,  user.Role) // Include the user's role
                        // Add other claims as needed
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid data");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string emailAddress, string password)
        {
            var users = await _context.Users.Where(u => u.EmailAddress == emailAddress).ToListAsync();

            if (!users.Any())
            {
                return BadRequest("Invalid email or password");
            }

            foreach (var user in users)
            {
                if (password == user.Password)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { token });
                }
            }

            return BadRequest("Invalid email or password");
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(User registerModel)
        {
            if (await _context.Users.AnyAsync(u => u.EmailAddress == registerModel.EmailAddress))
            {
                return BadRequest("Email already exists");
            }

            registerModel.Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

            _context.Users.Add(registerModel);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(registerModel);

            return Ok(new { token });
        }

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email && u.Password == password);
        }

        private string GenerateJwtToken(User user)
        {
            // create claims details based on the user information
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("EmailAddress", user.EmailAddress),
                new Claim("MobileNumber", user.MobileNumber),
                /*new Claim(ClaimTypes.Role, "User"),*/
                new Claim(ClaimTypes.Role,  user.Role) // Include the user's role
                // Add other claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}



