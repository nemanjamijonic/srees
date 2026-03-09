using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Feeders
{
    public class FeederDataOut
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public FeederType FeederType { get; set; }
        public int? SubstationId { get; set; }
        public List<int>? SuppliedRegions { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
    }
}
