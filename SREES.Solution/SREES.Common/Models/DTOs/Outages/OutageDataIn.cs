namespace SREES.Common.Models.Dtos.Outages
{
    public class OutageDataIn
    {
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public string? Description { get; set; }
        
        // Geolokacija prijave
        public double? ReportedLatitude { get; set; }
        public double? ReportedLongitude { get; set; }
        public string? ReportedAddress { get; set; }
        
        // Povezivanje sa kupcem (opciono)
        public int? CustomerId { get; set; }
        public int? BuildingId { get; set; }
    }
}
