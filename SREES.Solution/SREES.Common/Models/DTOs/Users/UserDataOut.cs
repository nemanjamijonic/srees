using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Users
{
    public class UserDataOut
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
    }
}
