using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Outage : BaseModel
    {
        // Existing fields
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public OutageStatus OutageStatus { get; set; }
        public string? Description { get; set; }
        public DateTime? ResolvedAt { get; set; }
        
        // ✅ NOVA POLJA - Geolokacija prijave
        public double? ReportedLatitude { get; set; }
        public double? ReportedLongitude { get; set; }
        public string? ReportedAddress { get; set; }
        
        // ✅ NOVA POLJA - Povezivanje sa korisnikom
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        
        public int? BuildingId { get; set; }
        public virtual Building? Building { get; set; }
        
        // ✅ NOVA POLJA - Automatski detektovani entiteti
        public OutageLevel? DetectedLevel { get; set; }
        
        public int? DetectedSubstationId { get; set; }
        public virtual Substation? DetectedSubstation { get; set; }
        
        public int? DetectedFeederId { get; set; }
        public virtual Feeder? DetectedFeeder { get; set; }
        
        public int? DetectedPoleId { get; set; }
        public virtual Pole? DetectedPole { get; set; }
        
        // ✅ NOVA POLJA - Grupacija prijava
        public int? OutageGroupId { get; set; }
        public virtual OutageGroup? OutageGroup { get; set; }
        
        // ✅ NOVA POLJA - Severity
        public OutageSeverity Severity { get; set; } = OutageSeverity.Low;
        
        // ✅ NOVA POLJA - Dodatne informacije
        public bool IsAutoDetected { get; set; } = true;  // Da li je automatski detektovano ili ručno
        public int Priority { get; set; } = 0;  // 0-100, viši broj = viši prioritet
    }
}
