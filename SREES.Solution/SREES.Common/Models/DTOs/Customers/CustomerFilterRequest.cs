namespace SREES.Common.Models.Dtos.Customers
{
    public class CustomerFilterRequest
    {
        public string? SearchTerm { get; set; }
        public int? CustomerType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
