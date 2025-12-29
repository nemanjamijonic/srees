using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Poles
{
    public class PoleDataIn
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public PoleType PoleType { get; set; }
        public int? RegionId { get; set; }
    }
}
