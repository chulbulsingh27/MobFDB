using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobFDB.Models;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobFDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _userService;
        private readonly MobDbContext _context;

        public LoginController(IConfiguration userService, MobDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        private bool VerifyPassword(Login model, string enteredPassword)
        {
            if (model.Password == enteredPassword)
                return true;

            return false;
        }
        private User AuthenticateCustomer(Login model)
        {
            var user = _context.Users.FirstOrDefault(o => o.EmailAddress == model.EmailAddress);
            if (user == null || !VerifyPassword(model, user.Password))
            {
                return null;
            }
            return user;
        }
        private string GenerateTokens(User user)
        {
            var claims = new[] {
             new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_userService["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_userService["Jwt:Issuer"], _userService["Jwt:Audience"], claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateCustomer(model);
            if (user_ != null)
            {
                var token = GenerateTokens(user_);
                response = Ok(new { EToken = token });

            }
            return response;

        }
    }
}

/*
[HttpPost("register")]
        public async Task<IActionResult> Register(User registerModel)
        {
            if (await _context.Users.AnyAsync(u => u.EmailAddress == registerModel.EmailAddress))
            {
                return BadRequest("Email already exists");
            }

            // Remove password hashing
            // registerModel.Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

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
                new Claim(ClaimTypes.Role, "User"),
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
*/


