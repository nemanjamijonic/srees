using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Auth;
using SREES.DAL.Models;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserService _userService;

        public UserApplicationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponsePackage<List<User>>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        public async Task<ResponsePackage<User?>> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }

        public async Task<ResponsePackage<User?>> CreateUser(User user)
        {
            return await _userService.CreateUser(user);
        }

        public async Task<ResponsePackage<User?>> UpdateUser(int id, User user)
        {
            return await _userService.UpdateUser(id, user);
        }

        public async Task<ResponsePackage<string>> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }

        public async Task<ResponsePackage<LoginResponse?>> Login(LoginRequest loginRequest)
        {
            return await _userService.Login(loginRequest);
        }

        public async Task<ResponsePackage<LoginResponse?>> Register(RegisterRequest registerRequest)
        {
            return await _userService.Register(registerRequest);
        }
    }
}
