using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Buildings
{
    public class BuildingDataOut
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
        public int? RegionId { get; set; }
        public int? PoleId { get; set; }
        public PoleType? PoleType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
    }
}
