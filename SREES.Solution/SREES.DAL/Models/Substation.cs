using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Substation : BaseModel
    {
        public SubstationType SubstationType { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
