using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Substations
{
    public class SubstationDataOut
    {
        public int Id { get; set; }
        public SubstationType SubstationType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
    }
}
