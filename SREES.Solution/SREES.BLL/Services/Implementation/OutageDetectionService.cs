using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class OutageDetectionService : IOutageDetectionService
    {
        private readonly ILogger<OutageDetectionService> _logger;
        private readonly IUnitOfWork _uow;

        // Maksimalna udaljenost za detekciju (u kilometrima)
        private const double MAX_DETECTION_RADIUS_KM = 5.0;

        public OutageDetectionService(ILogger<OutageDetectionService> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public async Task<ResponsePackage<OutageDetectionResult>> DetectNearestEntities(double latitude, double longitude)
        {
            try
            {
                var result = new OutageDetectionResult();

                // 1. Prona?i najbliži Pole
                var poles = await _uow.GetPoleRepository().GetAllAsync();
                var nearestPole = poles
                    .Select(p => new
                    {
                        Pole = p,
                        Distance = CalculateDistance(latitude, longitude, p.Latitude, p.Longitude)
                    })
                    .Where(x => x.Distance <= MAX_DETECTION_RADIUS_KM)
                    .OrderBy(x => x.Distance)
                    .FirstOrDefault();

                if (nearestPole != null)
                {
                    result.DetectedPoleId = nearestPole.Pole.Id;
                    result.DistanceToNearestPole = nearestPole.Distance;
                    result.DetectedRegionId = nearestPole.Pole.RegionId;
                    
                    _logger.LogInformation("Detektovan najbliži stub: {PoleId}, udaljenost: {Distance} km", 
                        nearestPole.Pole.Id, nearestPole.Distance);
                }

                // 2. Prona?i najbliži Substation
                var substations = await _uow.GetSubstationRepository().GetAllAsync();
                var nearestSubstation = substations
                    .Select(s => new
                    {
                        Substation = s,
                        Distance = CalculateDistance(latitude, longitude, s.Latitude, s.Longitude)
                    })
                    .Where(x => x.Distance <= MAX_DETECTION_RADIUS_KM)
                    .OrderBy(x => x.Distance)
                    .FirstOrDefault();

                if (nearestSubstation != null)
                {
                    result.DetectedSubstationId = nearestSubstation.Substation.Id;
                    
                    // Ako nemamo region iz Pole-a, uzmi iz Substation-a
                    if (result.DetectedRegionId == null)
                    {
                        result.DetectedRegionId = nearestSubstation.Substation.RegionId;
                    }
                    
                    _logger.LogInformation("Detektovana najbliža trafo-stanica: {SubstationId}, udaljenost: {Distance} km", 
                        nearestSubstation.Substation.Id, nearestSubstation.Distance);
                }

                // 3. Prona?i Feeder povezan sa detektovanom Substation
                if (result.DetectedSubstationId.HasValue)
                {
                    var feeders = await _uow.GetFeederRepository().GetAllAsync();
                    var connectedFeeder = feeders
                        .FirstOrDefault(f => f.SubstationId == result.DetectedSubstationId);

                    if (connectedFeeder != null)
                    {
                        result.DetectedFeederId = connectedFeeder.Id;
                        _logger.LogInformation("Detektovan povezani fider: {FeederId}", connectedFeeder.Id);
                    }
                }

                // Kreiraj poruku o detekciji
                result.DetectionMessage = GenerateDetectionMessage(result);

                return new ResponsePackage<OutageDetectionResult>(result, "Detekcija uspešno završena");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri detekciji najbližih entiteta za lokaciju ({Lat}, {Lon})", latitude, longitude);
                return new ResponsePackage<OutageDetectionResult>(null, "Greška pri detekciji lokacije kvara");
            }
        }

        /// <summary>
        /// Haversine formula za izra?unavanje udaljenosti izme?u dve geografske ta?ke
        /// </summary>
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radijus Zemlje u kilometrima

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private string GenerateDetectionMessage(OutageDetectionResult result)
        {
            var messages = new List<string>();

            if (result.DetectedPoleId.HasValue)
                messages.Add($"Stub ID: {result.DetectedPoleId}");
            
            if (result.DetectedFeederId.HasValue)
                messages.Add($"Fider ID: {result.DetectedFeederId}");
            
            if (result.DetectedSubstationId.HasValue)
                messages.Add($"Trafo-stanica ID: {result.DetectedSubstationId}");

            if (messages.Count == 0)
                return "Nije prona?en nijedan entitet u blizini";

            return $"Detektovano: {string.Join(", ", messages)}";
        }
    }
}
