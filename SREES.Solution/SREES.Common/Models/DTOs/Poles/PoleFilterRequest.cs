using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Poles
{
    public class PoleFilterRequest
    {
        public string? SearchTerm { get; set; }
        public PoleType? PoleType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
