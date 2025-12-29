using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Feeder : BaseModel
    {
        public string? Name { get; set; }
        public FeederType FeederType { get; set; }
        public int? SubstationId { get; set; }
        public Substation? Substation { get; set; }
        public List<int>? SuppliedRegions { get; set; } 
    }
}
