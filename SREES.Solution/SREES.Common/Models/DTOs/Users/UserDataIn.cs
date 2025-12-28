using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Users
{
    public class UserDataIn
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public Role Role { get; set; }
    }
}
