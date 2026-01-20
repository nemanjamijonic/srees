using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class OutageGroup : BaseModel
    {
        public string GroupName { get; set; } = string.Empty;
        public OutageLevel DetectedLevel { get; set; }
        public OutageSeverity Severity { get; set; }
        public int AffectedCustomersCount { get; set; }
        public int? DetectedSubstationId { get; set; }
        public virtual Substation? DetectedSubstation { get; set; }
        public int? DetectedFeederId { get; set; }
        public virtual Feeder? DetectedFeeder { get; set; }
        public int? DetectedPoleId { get; set; }
        public virtual Pole? DetectedPole { get; set; }
        public int RegionId { get; set; }
        public virtual Region? Region { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public bool IsResolved { get; set; }
        
        // Navigation property za sve prijave u grupi
        public virtual ICollection<Outage>? Outages { get; set; }
    }
}
