namespace SREES.Common.Models.Dtos.Buildings
{
    public class BuildingDataIn
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
        public int? RegionId { get; set; }
        public int? PoleId { get; set; }
    }
}
