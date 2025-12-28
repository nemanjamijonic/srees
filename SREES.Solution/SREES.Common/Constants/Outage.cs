using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SREES.Common.Models;

namespace SREES.Common.Constants
{
    public class Outage : BaseModel
    {
        public int RegionId { get; set; }
        public int UserId { get; set; }
        public OutageStatus OutageStatus { get;set; }
    }
}
