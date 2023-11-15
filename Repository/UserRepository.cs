using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobFDB.Interface;
using MobFDB.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MobFDB.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MobDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(MobDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task PutUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

    
        /*public async Task<ActionResult<User>> Login(string EmailAddress, string password)
        {
            if (string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(password))
            {
                return new BadRequestObjectResult("EmailAddress and password are required.");
            }

            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == EmailAddress && u.Password == password);
            if (user1 == null)
            {
                return new NotFoundObjectResult("Incorrect EmailAddress or password.");
            }

            if (string.IsNullOrEmpty(user1.EmailAddress) || string.IsNullOrEmpty(user1.Password))
            {
                return new BadRequestObjectResult("User data is invalid.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user1.EmailAddress),
            new Claim(ClaimTypes.Role, user1.Role),
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Set token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new JsonResult(new { Token = tokenString, Role = user1.Role }) { StatusCode = 200 };
        }

        public async Task<ActionResult<User>> CheckAdminLogin(string EmailAddress, string password)
        {
            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == EmailAddress && u.Password == password && u.Role == "admin");
            if (user1 == null)
            {
                // Return an error message or handle the case where the user doesn't exist or doesn't have the "admin" role.
                return new NotFoundObjectResult("Incorrect EmailAddress, password, or not an admin user.");
            }
            return user1;


        }*/

        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }

}
