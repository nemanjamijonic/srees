using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Helpers;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Auth;
using SREES.Common.Models.DTOs.Users;
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

        public async Task<ResponsePackage<LoginResponse?>> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = await _uow.GetUserRepository().GetByEmailAsync(loginRequest.Email);
                
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", loginRequest.Email);
                    return new ResponsePackage<LoginResponse?>(null, "Nevažeće korisničko ime ili lozinka");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", loginRequest.Email);
                    return new ResponsePackage<LoginResponse?>(null, "Nevažeće korisničko ime ili lozinka");
                }

                // Generate JWT token
                var userToken = new UserTokenDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    UserName = user.FirstName + " " + user.LastName,
                    Role = user.Role
                };

                var token = JwtManager.GetToken(userToken);

                var loginResponse = new LoginResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Role = user.Role.ToString()
                };

                _logger.LogInformation("Successful login for user: {Email}", user.Email);
                return new ResponsePackage<LoginResponse?>(loginResponse, "Uspešno prijavljivanje");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", loginRequest.Email);
                return new ResponsePackage<LoginResponse?>(null, "Greška pri prijavljivanju");
            }
        }

        public async Task<ResponsePackage<LoginResponse?>> Register(RegisterRequest registerRequest)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _uow.GetUserRepository().GetByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt with existing email: {Email}", registerRequest.Email);
                    return new ResponsePackage<LoginResponse?>(null, "Korisnik sa ovom email adresom već postoji");
                }

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                // Create new user
                var newUser = new User
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    PasswordHash = passwordHash,
                    Role = registerRequest.Role,
                    Guid = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _uow.GetUserRepository().AddAsync(newUser);
                await _uow.CompleteAsync();

                // Generate JWT token

                var userToken = new UserTokenDto
                {
                    Id = newUser.Id,
                    Email = newUser.Email ?? string.Empty,
                    UserName = newUser.FirstName + " " + newUser.LastName,
                    Role = newUser.Role
                };

                var token = JwtManager.GetToken(userToken);

                var loginResponse = new LoginResponse
                {
                    Token = token,
                    UserId = newUser.Id,
                    Email = newUser.Email ?? string.Empty,
                    FirstName = newUser.FirstName ?? string.Empty,
                    LastName = newUser.LastName ?? string.Empty,
                    Role = newUser.Role.ToString()
                };

                _logger.LogInformation("New user registered: {Email}", newUser.Email);
                return new ResponsePackage<LoginResponse?>(loginResponse, "Uspešno registrovanje");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", registerRequest.Email);
                return new ResponsePackage<LoginResponse?>(null, "Greška pri registrovanju");
            }
        }
    }
}
