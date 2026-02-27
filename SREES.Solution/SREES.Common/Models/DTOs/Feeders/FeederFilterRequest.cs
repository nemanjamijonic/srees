using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Feeders
{
    public class FeederFilterRequest
    {
        public string? SearchTerm { get; set; }
        public FeederType? FeederType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
