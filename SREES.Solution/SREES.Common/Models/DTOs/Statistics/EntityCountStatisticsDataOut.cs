namespace SREES.Common.Models.Dtos.Statistics
{
    public class EntityCountStatisticsDataOut
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public string? Type { get; set; }
    }
}
