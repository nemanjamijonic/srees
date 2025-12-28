using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Substations
{
    public class SubstationDataIn
    {
        public SubstationType SubstationType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RegionId { get; set; }
    }
}
