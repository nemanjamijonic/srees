using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Buildings;
using SREES.Common.Models.Dtos.Statistics;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class BuildingService : IBuildingService
    {
        private readonly ILogger<BuildingService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BuildingService(ILogger<BuildingService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<BuildingDataOut>>> GetAllBuildings()
        {
            try
            {
                var buildings = await _uow.GetBuildingRepository().GetAllWithPoleAsync();
                var buildingList = _mapper.Map<List<BuildingDataOut>>(buildings.ToList());
                return new ResponsePackage<List<BuildingDataOut>>(buildingList, "Zgrade uspešno preuzete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih zgrada");
                return new ResponsePackage<List<BuildingDataOut>>(null, "Greška pri preuzimanju zgrada");
            }
        }

        public async Task<ResponsePackage<List<BuildingSelectDataOut>>> GetAllBuildingsForSelect()
        {
            try
            {
                var buildings = await _uow.GetBuildingRepository().GetAllAsync();
                var buildingSelectList = _mapper.Map<List<BuildingSelectDataOut>>(buildings.ToList());
                return new ResponsePackage<List<BuildingSelectDataOut>>(buildingSelectList, "Zgrade za select uspešno preuzete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju zgrada za select");
                return new ResponsePackage<List<BuildingSelectDataOut>>(null, "Greška pri preuzimanju zgrada za select");
            }
        }

        public async Task<ResponsePackage<BuildingDataOut?>> GetBuildingById(int id)
        {
            try
            {
                var building = await _uow.GetBuildingRepository().GetByIdAsync(id);
                if (building == null)
                    return new ResponsePackage<BuildingDataOut?>(null, "Zgrada nije prona?ena");

                var buildingDataOut = _mapper.Map<BuildingDataOut>(building);
                return new ResponsePackage<BuildingDataOut?>(buildingDataOut, "Zgrada uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju zgrade sa ID-om {BuildingId}", id);
                return new ResponsePackage<BuildingDataOut?>(null, "Greška pri preuzimanju zgrade");
            }
        }

        public async Task<ResponsePackage<BuildingDataOut?>> CreateBuilding(BuildingDataIn buildingDataIn)
        {
            try
            {
                // Provera da li regija postoji ako je poslata
                if (buildingDataIn.RegionId.HasValue)
                {
                    var region = await _uow.GetRegionRepository().GetByIdAsync(buildingDataIn.RegionId.Value);
                    if (region == null)
                        return new ResponsePackage<BuildingDataOut?>(null, "Regija nije pronađena");
                }

                // Provera da li stub postoji ako je poslat
                if (buildingDataIn.PoleId.HasValue)
                {
                    var pole = await _uow.GetPoleRepository().GetByIdAsync(buildingDataIn.PoleId.Value);
                    if (pole == null)
                        return new ResponsePackage<BuildingDataOut?>(null, "Stub nije pronađen");
                }

                var building = new Building
                {
                    Latitude = buildingDataIn.Latitude,
                    Longitude = buildingDataIn.Longitude,
                    OwnerName = buildingDataIn.OwnerName,
                    Address = buildingDataIn.Address,
                    RegionId = buildingDataIn.RegionId,
                    PoleId = buildingDataIn.PoleId
                };

                await _uow.GetBuildingRepository().AddAsync(building);
                await _uow.CompleteAsync();

                var buildingDataOut = _mapper.Map<BuildingDataOut>(building);
                return new ResponsePackage<BuildingDataOut?>(buildingDataOut, "Zgrada uspešno kreirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju zgrade");
                return new ResponsePackage<BuildingDataOut?>(null, "Greška pri kreiranju zgrade");
            }
        }

        public async Task<ResponsePackage<BuildingDataOut?>> UpdateBuilding(int id, BuildingDataIn buildingDataIn)
        {
            try
            {
                var building = await _uow.GetBuildingRepository().GetByIdAsync(id);
                if (building == null)
                    return new ResponsePackage<BuildingDataOut?>(null, "Zgrada nije prona?ena");

                // Provera da li regija postoji ako je poslata
                if (buildingDataIn.RegionId.HasValue)
                {
                    var region = await _uow.GetRegionRepository().GetByIdAsync(buildingDataIn.RegionId.Value);
                    if (region == null)
                        return new ResponsePackage<BuildingDataOut?>(null, "Regija nije prona?ena");
                }

                // Provera da li stub postoji ako je poslat
                if (buildingDataIn.PoleId.HasValue)
                {
                    var pole = await _uow.GetPoleRepository().GetByIdAsync(buildingDataIn.PoleId.Value);
                    if (pole == null)
                        return new ResponsePackage<BuildingDataOut?>(null, "Stub nije prona?en");
                }

                building.Latitude = buildingDataIn.Latitude;
                building.Longitude = buildingDataIn.Longitude;
                building.OwnerName = buildingDataIn.OwnerName;
                building.Address = buildingDataIn.Address;
                building.RegionId = buildingDataIn.RegionId;
                building.PoleId = buildingDataIn.PoleId;
                building.LastUpdateTime = DateTime.Now;

                await _uow.CompleteAsync();

                var buildingDataOut = _mapper.Map<BuildingDataOut>(building);
                return new ResponsePackage<BuildingDataOut?>(buildingDataOut, "Zgrada uspešno ažurirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju zgrade sa ID-om {BuildingId}", id);
                return new ResponsePackage<BuildingDataOut?>(null, "Greška pri ažuriranju zgrade");
            }
        }

        public async Task<ResponsePackage<string>> DeleteBuilding(int id)
        {
            try
            {
                var building = await _uow.GetBuildingRepository().GetByIdAsync(id);
                if (building == null)
                    return new ResponsePackage<string>(null, "Zgrada nije prona?ena");

                _uow.GetBuildingRepository().RemoveEntity(building);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Zgrada uspešno obrisana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju zgrade sa ID-om {BuildingId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju zgrade");
            }
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetBuildingStatistics()
        {
            try
            {
                var totalCount = await _uow.GetBuildingRepository().GetTotalBuildingCountAsync();
                var statistics = new List<EntityCountStatisticsDataOut>
                {
                    new EntityCountStatisticsDataOut
                    {
                        Name = "Total Buildings",
                        Count = totalCount,
                        Type = "Total"
                    }
                };

                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(statistics, "Statistika zgrada uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju statistike zgrada");
                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(null, "Greška pri preuzimanju statistike zgrada");
            }
        }
    }
}
