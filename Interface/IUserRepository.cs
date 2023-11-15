using Microsoft.AspNetCore.Mvc;
using MobFDB.Models;

namespace MobFDB.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task PutUser(User user);
        Task<User> PostUser(User user);
        Task DeleteUser(int id);
        /*Task<ActionResult<User>> Login(string EmailAddress, string password);
        Task<ActionResult<User>> CheckAdminLogin(string EmailAddress, string password);*/
        bool UserExists(int id);
    }
}
