using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Outage : BaseModel
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public OutageStatus OutageStatus { get; set; }
        public string? Description { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
