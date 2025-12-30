using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Customers
{
    public class CustomerDataIn
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? BuildingId { get; set; }
        public bool IsActive { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
