using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Outages
{
    public class OutageDataOut
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public string? RegionName { get; set; }
        public OutageStatus OutageStatus { get; set; }
        public string? Description { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
        
        // Geolokacija prijave
        public double? ReportedLatitude { get; set; }
        public double? ReportedLongitude { get; set; }
        public string? ReportedAddress { get; set; }
        
        // Povezivanje sa kupcem
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int? BuildingId { get; set; }
        public string? BuildingAddress { get; set; }
        
        // Automatski detektovani entiteti
        public OutageLevel? DetectedLevel { get; set; }
        public int? DetectedSubstationId { get; set; }
        public string? DetectedSubstationName { get; set; }
        public int? DetectedFeederId { get; set; }
        public string? DetectedFeederName { get; set; }
        public int? DetectedPoleId { get; set; }
        public string? DetectedPoleName { get; set; }
        
        // Severity i prioritet
        public OutageSeverity Severity { get; set; }
        public int Priority { get; set; }
        public bool IsAutoDetected { get; set; }
        
        // Grupacija
        public int? OutageGroupId { get; set; }
    }
}
