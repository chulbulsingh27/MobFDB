using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobFDB.Interface;
using MobFDB.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobFDB.Controllers
{
    [Authorize] // Requires authentication for all actions in the controller
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
      

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
        }

        [HttpGet]
       /* [Authorize(Roles = "Admin")]*/
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Requires "Admin" role for accessing this action
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure that the authenticated user can only update their own data
            if (id != user.UserId || id != int.Parse(userId))
            {
                return BadRequest("You can only update your own data.");
            }

            try
            {
                await _userRepository.PutUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userRepository.UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous] // This action can be accessed without authentication
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var createdUser = await _userRepository.PostUser(user);

            return CreatedAtAction("GetUser", new { id = createdUser.UserId }, createdUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Requires "Admin" role for accessing this action
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);

            return NoContent();
        }
    }
}