using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;
using SREES.Common.Models.Dtos.Outages;
using SREES.Common.Models.Dtos.Statistics;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;
using OutageModel = SREES.DAL.Models.Outage;

namespace SREES.BLL.Services.Implementation
{
    public class OutageService : IOutageService
    {
        private readonly ILogger<OutageService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IOutageDetectionService _detectionService;
        private readonly INotificationService _notificationService;

        public OutageService(
            ILogger<OutageService> logger, 
            IUnitOfWork uow, 
            IMapper mapper,
            IOutageDetectionService detectionService,
            INotificationService notificationService)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _detectionService = detectionService;
            _notificationService = notificationService;
        }

        public async Task<ResponsePackage<List<OutageDataOut>>> GetAllOutages()
        {
            try
            {
                var outages = await _uow.GetOutageRepository().GetAllWithIncludesAsync();
                var outageList = _mapper.Map<List<OutageDataOut>>(outages.ToList());
                return new ResponsePackage<List<OutageDataOut>>(outageList, "Prekidi napajanja uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih prekida napajanja");
                return new ResponsePackage<List<OutageDataOut>>(null, "Greška pri preuzimanju prekida napajanja");
            }
        }

        public async Task<ResponsePackage<OutageDataOut?>> GetOutageById(int id)
        {
            try
            {
                var outage = await _uow.GetOutageRepository().GetByIdWithIncludesAsync(id);
                if (outage == null)
                    return new ResponsePackage<OutageDataOut?>(null, "Prekid napajanja nije prona?en");

                var outageDataOut = _mapper.Map<OutageDataOut>(outage);
                return new ResponsePackage<OutageDataOut?>(outageDataOut, "Prekid napajanja uspešno preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju prekida napajanja sa ID-om {OutageId}", id);
                return new ResponsePackage<OutageDataOut?>(null, "Greška pri preuzimanju prekida napajanja");
            }
        }

        public async Task<ResponsePackage<OutageDataOut?>> CreateOutage(OutageDataIn outageDataIn)
        {
            try
            {
                // Provera da li korisnik postoji
                var user = await _uow.GetUserRepository().GetByIdAsync(outageDataIn.UserId);
                if (user == null)
                    return new ResponsePackage<OutageDataOut?>(null, "Korisnik nije prona?en");

                // Provera da li Customer postoji (ako je prosle?en)
                if (outageDataIn.CustomerId.HasValue)
                {
                    var customer = await _uow.GetCustomerRepository().GetByIdAsync(outageDataIn.CustomerId.Value);
                    if (customer == null)
                        return new ResponsePackage<OutageDataOut?>(null, "Kupac nije prona?en");
                }

                var outageModel = new OutageModel
                {
                    UserId = outageDataIn.UserId,
                    RegionId = outageDataIn.RegionId,
                    OutageStatus = OutageStatus.Reported,
                    Description = outageDataIn.Description,
                    ReportedLatitude = outageDataIn.ReportedLatitude,
                    ReportedLongitude = outageDataIn.ReportedLongitude,
                    ReportedAddress = outageDataIn.ReportedAddress,
                    CustomerId = outageDataIn.CustomerId,
                    BuildingId = outageDataIn.BuildingId,
                    Severity = OutageSeverity.Low,
                    IsAutoDetected = false,
                    Priority = 0,
                    Guid = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow
                };

                // Automatska detekcija najbližih entiteta ako imamo koordinate
                if (outageDataIn.ReportedLatitude.HasValue && outageDataIn.ReportedLongitude.HasValue)
                {
                    var detectionResult = await _detectionService.DetectNearestEntities(
                        outageDataIn.ReportedLatitude.Value,
                        outageDataIn.ReportedLongitude.Value);

                    if (detectionResult.Data != null)
                    {
                        outageModel.DetectedPoleId = detectionResult.Data.DetectedPoleId;
                        outageModel.DetectedFeederId = detectionResult.Data.DetectedFeederId;
                        outageModel.DetectedSubstationId = detectionResult.Data.DetectedSubstationId;
                        outageModel.IsAutoDetected = true;

                        // Ako nismo dobili RegionId iz inputa, koristi detektovani
                        if (outageDataIn.RegionId == 0 && detectionResult.Data.DetectedRegionId.HasValue)
                        {
                            outageModel.RegionId = detectionResult.Data.DetectedRegionId.Value;
                        }

                        // Odredi nivo kvara na osnovu detekcije
                        outageModel.DetectedLevel = DetermineOutageLevel(detectionResult.Data);

                        _logger.LogInformation("Auto-detekcija uspešna: {Message}", detectionResult.Data.DetectionMessage);
                    }
                }

                await _uow.GetOutageRepository().AddAsync(outageModel);
                await _uow.CompleteAsync();

                // U?itaj ponovo sa uklju?enim relacijama za mapiranje
                var createdOutage = await _uow.GetOutageRepository().GetByIdWithIncludesAsync(outageModel.Id);
                var outageDataOut = _mapper.Map<OutageDataOut>(createdOutage);
                
                return new ResponsePackage<OutageDataOut?>(outageDataOut, "Prekid napajanja uspešno kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju prekida napajanja");
                return new ResponsePackage<OutageDataOut?>(null, "Greška pri kreiranju prekida napajanja");
            }
        }

        private OutageLevel DetermineOutageLevel(OutageDetectionResult detection)
        {
            // Odre?ivanje nivoa kvara na osnovu detektovanih entiteta
            if (detection.DetectedSubstationId.HasValue && !detection.DetectedFeederId.HasValue)
                return OutageLevel.Substation;
            
            if (detection.DetectedFeederId.HasValue && !detection.DetectedPoleId.HasValue)
                return OutageLevel.Feeder;
            
            if (detection.DetectedPoleId.HasValue)
                return OutageLevel.Pole;
            
            return OutageLevel.Unknown;
        }

        public async Task<ResponsePackage<OutageDataOut?>> UpdateOutageStatus(int id, OutageStatusUpdateDataIn statusUpdate)
        {
            try
            {
                var outage = await _uow.GetOutageRepository().GetByIdAsync(id);
                if (outage == null)
                    return new ResponsePackage<OutageDataOut?>(null, "Prekid napajanja nije prona?en");

                var oldStatus = outage.OutageStatus.ToString();
                outage.OutageStatus = statusUpdate.NewStatus;
                outage.LastUpdateTime = DateTime.UtcNow;

                // Ako je status postavljen na Resolved, postavi vreme razrešavanja
                if (statusUpdate.NewStatus == OutageStatus.Resolved)
                {
                    outage.ResolvedAt = DateTime.UtcNow;
                }

                await _uow.CompleteAsync();

                // Kreiraj obaveštenje o promeni statusa za korisnika koji je prijavio kvar
                try
                {
                    await _notificationService.CreateOutageStatusNotification(
                        id, 
                        outage.UserId, 
                        oldStatus, 
                        statusUpdate.NewStatus.ToString());
                    
                    _logger.LogInformation("Kreirano obaveštenje o promeni statusa kvara {OutageId} za korisnika {UserId}", id, outage.UserId);
                }
                catch (Exception notifEx)
                {
                    // Ne prekidamo operaciju ako kreiranje obaveštenja ne uspe
                    _logger.LogWarning(notifEx, "Neuspešno kreiranje obaveštenja za kvar {OutageId}", id);
                }

                var outageDataOut = _mapper.Map<OutageDataOut>(outage);
                return new ResponsePackage<OutageDataOut?>(outageDataOut, "Status prekida napajanja uspešno ažuriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju statusa prekida napajanja sa ID-om {OutageId}", id);
                return new ResponsePackage<OutageDataOut?>(null, "Greška pri ažuriranju statusa prekida napajanja");
            }
        }

        public async Task<ResponsePackage<string>> DeleteOutage(int id)
        {
            try
            {
                var outage = await _uow.GetOutageRepository().GetByIdAsync(id);
                if (outage == null)
                    return new ResponsePackage<string>(null, "Prekid napajanja nije prona?en");

                _uow.GetOutageRepository().RemoveEntity(outage);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Prekid napajanja uspešno obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju prekida napajanja sa ID-om {OutageId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju prekida napajanja");
            }
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetOutageStatistics()
        {
            try
            {
                var outageCountByStatus = await _uow.GetOutageRepository().GetOutageCountByStatusAsync();
                var statistics = outageCountByStatus
                    .Select(kvp => new EntityCountStatisticsDataOut
                    {
                        Name = kvp.Key.ToString(),
                        Count = kvp.Value,
                        Type = "OutageStatus"
                    })
                    .ToList();

                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(statistics, "Statistika prekida napajanja uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju statistike prekida napajanja");
                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(null, "Greška pri preuzimanju statistike prekida napajanja");
            }
        }

        public async Task<ResponsePackage<List<OutageDataOut>>> GetOutagesByFeederId(int feederId)
        {
            try
            {
                var feeder = await _uow.GetFeederRepository().GetByIdAsync(feederId);
                if (feeder == null)
                    return new ResponsePackage<List<OutageDataOut>>(null, "Fider nije prona?en");

                var outages = await _uow.GetOutageRepository().GetOutagesByFeederIdAsync(feederId);
                var outageList = _mapper.Map<List<OutageDataOut>>(outages.ToList());
                
                return new ResponsePackage<List<OutageDataOut>>(outageList, $"Istorija kvarova za fider '{feeder.Name}' uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju istorije kvarova za fider {FeederId}", feederId);
                return new ResponsePackage<List<OutageDataOut>>(null, "Greška pri preuzimanju istorije kvarova za fider");
            }
        }

        public async Task<ResponsePackage<List<OutageDataOut>>> GetOutagesBySubstationId(int substationId)
        {
            try
            {
                var substation = await _uow.GetSubstationRepository().GetByIdAsync(substationId);
                if (substation == null)
                    return new ResponsePackage<List<OutageDataOut>>(null, "Trafo-stanica nije prona?ena");

                var outages = await _uow.GetOutageRepository().GetOutagesBySubstationIdAsync(substationId);
                var outageList = _mapper.Map<List<OutageDataOut>>(outages.ToList());
                
                return new ResponsePackage<List<OutageDataOut>>(outageList, $"Istorija kvarova za trafo-stanicu '{substation.Name}' uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju istorije kvarova za trafo-stanicu {SubstationId}", substationId);
                return new ResponsePackage<List<OutageDataOut>>(null, "Greška pri preuzimanju istorije kvarova za trafo-stanicu");
            }
        }
    }
}
