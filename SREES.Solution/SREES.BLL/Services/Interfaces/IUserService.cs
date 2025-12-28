using SREES.Common.Models;
using SREES.DAL.Models;

namespace SREES.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponsePackage<List<User>>> GetAllUsers();
        Task<ResponsePackage<User?>> GetUserById(int id);
        Task<ResponsePackage<User?>> CreateUser(User user);
        Task<ResponsePackage<User?>> UpdateUser(int id, User user);
        Task<ResponsePackage<string>> DeleteUser(int id);
    }
}
