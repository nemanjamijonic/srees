using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Customer : BaseModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public int BuildingId { get; set; }
    }
}
