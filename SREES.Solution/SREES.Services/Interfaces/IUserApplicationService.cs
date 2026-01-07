using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Auth;
using SREES.DAL.Models;

namespace SREES.Services.Interfaces
{
    public interface IUserApplicationService
    {
        Task<ResponsePackage<List<User>>> GetAllUsers();
        Task<ResponsePackage<User?>> GetUserById(int id);
        Task<ResponsePackage<User?>> CreateUser(User user);
        Task<ResponsePackage<User?>> UpdateUser(int id, User user);
        Task<ResponsePackage<string>> DeleteUser(int id);
        Task<ResponsePackage<LoginResponse?>> Login(LoginRequest loginRequest);
        Task<ResponsePackage<LoginResponse?>> Register(RegisterRequest registerRequest);
    }
}
