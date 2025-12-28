using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Outages
{
    public class OutageDataOut
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public OutageStatus OutageStatus { get; set; }
        public string? Description { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid Guid { get; set; }
    }
}
