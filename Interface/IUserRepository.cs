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
        bool UserExists(int id);
    }
}
