using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Feeders
{
    public class FeederDataIn
    {
        public string? Name { get; set; }
        public FeederType FeederType { get; set; }
        public int? SubstationId { get; set; }
        public List<int>? SuppliedRegions { get; set; }
    }
}
