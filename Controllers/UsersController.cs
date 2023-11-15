using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobFDB.Interface;
using MobFDB.Models;
using System.Threading.Tasks;

namespace MobFDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var createdUser = await _userRepository.PostUser(user);
            return CreatedAtAction("GetUser", new { id = createdUser.UserId }, createdUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
            return NoContent();
        }

        /*[HttpPost("login")]
        public async Task<ActionResult<User>> Login(string email, string password)
        {
            
            *//*  if (result.Value == null)
              {
                  return NotFound();
              }
              return Ok(result.Value);
          }*//*
            return await _userRepository.Login(email, password);
        }

        [HttpPost("checkAdminLogin")]
        public async Task<ActionResult<User>> CheckAdminLogin(string email, string password)
        {
            
            *//*if (result.Value == null)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }*//*
            return await _userRepository.CheckAdminLogin(email, password);
        }*/
    }
}
