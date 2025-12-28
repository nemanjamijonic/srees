using SREES.Common.Models;

namespace SREES.Common.Constants
{
    public class Region : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
