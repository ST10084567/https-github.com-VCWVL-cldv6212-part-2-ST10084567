using CLOUD_STORAGE_2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CLOUD_STORAGE_2.Repositories
{
    public interface IUserRepository
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task AddUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int userId);
    }
}


