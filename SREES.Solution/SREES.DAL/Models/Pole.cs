using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Pole : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public PoleType PoleType { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
