using SREES.Common.Constants;

namespace SREES.Common.Models.DTOs.Map
{
    public class MapDataOut
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public MapPin MapPin { get; set; }
        public int RegionId { get; set; }
    }
}
