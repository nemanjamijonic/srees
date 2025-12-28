namespace SREES.Common.Models.Dtos.Outages
{
    public class OutageDataIn
    {
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public string? Description { get; set; }
    }
}
