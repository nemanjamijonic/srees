using SREES.Common.Models;

namespace SREES.BLL.Services.Interfaces
{
    public class OutageDetectionResult
    {
        public int? DetectedPoleId { get; set; }
        public int? DetectedFeederId { get; set; }
        public int? DetectedSubstationId { get; set; }
        public int? DetectedRegionId { get; set; }
        public double DistanceToNearestPole { get; set; }
        public string? DetectionMessage { get; set; }
    }

    public interface IOutageDetectionService
    {
        /// <summary>
        /// Automatski detektuje najbliži Pole, Feeder i Substation na osnovu geografske lokacije
        /// </summary>
        /// <param name="latitude">Geografska širina prijave</param>
        /// <param name="longitude">Geografska dužina prijave</param>
        /// <returns>Rezultat detekcije sa ID-ovima najbližih entiteta</returns>
        Task<ResponsePackage<OutageDetectionResult>> DetectNearestEntities(double latitude, double longitude);

        /// <summary>
        /// Izra?unava udaljenost izme?u dve ta?ke koriste?i Haversine formulu
        /// </summary>
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
    }
}
