using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Building : BaseModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
        public int? PoleId { get; set; }
        public Pole? Pole { get; set; }
    }
}
