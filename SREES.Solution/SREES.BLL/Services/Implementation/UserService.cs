using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _uow;

        public UserService(ILogger<UserService> logger, IUnitOfWork uow) 
        {
            _logger = logger;
            _uow = uow;
        }

        public async Task<ResponsePackage<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _uow.GetUserRepository().GetAllAsync();
                var userList = users.ToList();
                return new ResponsePackage<List<User>>(userList, "Korisnici uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih korisnika");
                return new ResponsePackage<List<User>>("Greška pri preuzimanju korisnika");
            }
        }

        public async Task<ResponsePackage<User?>> GetUserById(int id)
        {
            try
            {
                var user = await _uow.GetUserRepository().GetByIdAsync(id);
                if (user == null)
                    return new ResponsePackage<User?>("Korisnik nije pronađen");
                
                return new ResponsePackage<User?>(user, "Korisnik uspešno preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju korisnika sa ID-om {UserId}", id);
                return new ResponsePackage<User?>("Greška pri preuzimanju korisnika");
            }
        }

        public async Task<ResponsePackage<User?>> CreateUser(User user)
        {
            try
            {
                await _uow.GetUserRepository().AddAsync(user);
                await _uow.CompleteAsync();
                return new ResponsePackage<User?>(user, "Korisnik uspešno kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju korisnika");
                return new ResponsePackage<User?>("Greška pri kreiranju korisnika");
            }
        }

        public async Task<ResponsePackage<User?>> UpdateUser(int id, User user)
        {
            try
            {
                var existingUser = await _uow.GetUserRepository().GetByIdAsync(id);
                if (existingUser == null)
                    return new ResponsePackage<User?>("Korisnik nije pronađen");

                existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
                existingUser.LastName = user.LastName ?? existingUser.LastName;
                existingUser.Email = user.Email ?? existingUser.Email;
                existingUser.PasswordHash = user.PasswordHash ?? existingUser.PasswordHash;
                existingUser.Role = user.Role;
                existingUser.LastUpdateTime = DateTime.UtcNow;

                await _uow.CompleteAsync();
                return new ResponsePackage<User?>(existingUser, "Korisnik uspešno ažuriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju korisnika sa ID-om {UserId}", id);
                return new ResponsePackage<User?>("Greška pri ažuriranju korisnika");
            }
        }

        public async Task<ResponsePackage<string>> DeleteUser(int id)
        {
            try
            {
                var user = await _uow.GetUserRepository().GetByIdAsync(id);
                if (user == null)
                    return new ResponsePackage<string>("Korisnik nije pronađen");

                _uow.GetUserRepository().RemoveEntity(user);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>("Korisnik uspešno obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju korisnika sa ID-om {UserId}", id);
                return new ResponsePackage<string>("Greška pri brisanju korisnika");
            }
        }
    }
}
