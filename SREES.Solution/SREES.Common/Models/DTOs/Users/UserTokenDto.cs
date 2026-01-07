using SREES.Common.Constants;

namespace SREES.Common.Models.DTOs.Users
{
    public class UserTokenDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
