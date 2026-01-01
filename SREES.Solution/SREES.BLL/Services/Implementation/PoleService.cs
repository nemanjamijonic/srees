using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Poles;
using SREES.Common.Models.Dtos.Statistics;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class PoleService : IPoleService
    {
        private readonly ILogger<PoleService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PoleService(ILogger<PoleService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<PoleDataOut>>> GetAllPoles()
        {
            try
            {
                var poles = await _uow.GetPoleRepository().GetAllAsync();
                var poleList = _mapper.Map<List<PoleDataOut>>(poles.ToList());
                return new ResponsePackage<List<PoleDataOut>>(poleList, "Stubovi uspe�no preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju svih stubova");
                return new ResponsePackage<List<PoleDataOut>>(null, "Gre�ka pri preuzimanju stubova");
            }
        }

        public async Task<ResponsePackage<List<PoleSelectDataOut>>> GetAllPolesForSelect()
        {
            try
            {
                var poles = await _uow.GetPoleRepository().GetAllAsync();
                var poleSelectList = _mapper.Map<List<PoleSelectDataOut>>(poles.ToList());
                return new ResponsePackage<List<PoleSelectDataOut>>(poleSelectList, "Stubovi za select uspe�no preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju stubova za select");
                return new ResponsePackage<List<PoleSelectDataOut>>(null, "Gre�ka pri preuzimanju stubova za select");
            }
        }

        public async Task<ResponsePackage<PoleDataOut?>> GetPoleById(int id)
        {
            try
            {
                var pole = await _uow.GetPoleRepository().GetByIdAsync(id);
                if (pole == null)
                    return new ResponsePackage<PoleDataOut?>(null, "Stub nije prona?en");

                var poleDataOut = _mapper.Map<PoleDataOut>(pole);
                return new ResponsePackage<PoleDataOut?>(poleDataOut, "Stub uspe�no preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju stuba sa ID-om {PoleId}", id);
                return new ResponsePackage<PoleDataOut?>(null, "Gre�ka pri preuzimanju stuba");
            }
        }

        public async Task<ResponsePackage<PoleDataOut?>> CreatePole(PoleDataIn poleDataIn)
        {
            try
            {
                // Provera da li regija postoji ako je poslata
                if (poleDataIn.RegionId.HasValue)
                {
                    var region = await _uow.GetRegionRepository().GetByIdAsync(poleDataIn.RegionId.Value);
                    if (region == null)
                        return new ResponsePackage<PoleDataOut?>(null, "Regija nije prona?ena");
                }

                var pole = new Pole
                {
                    Name = poleDataIn.Name,
                    Latitude = poleDataIn.Latitude,
                    Longitude = poleDataIn.Longitude,
                    Address = poleDataIn.Address,
                    PoleType = poleDataIn.PoleType,
                    RegionId = poleDataIn.RegionId
                };

                await _uow.GetPoleRepository().AddAsync(pole);
                await _uow.CompleteAsync();

                var poleDataOut = _mapper.Map<PoleDataOut>(pole);
                return new ResponsePackage<PoleDataOut?>(poleDataOut, "Stub uspe�no kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri kreiranju stuba");
                return new ResponsePackage<PoleDataOut?>(null, "Gre�ka pri kreiranju stuba");
            }
        }

        public async Task<ResponsePackage<PoleDataOut?>> UpdatePole(int id, PoleDataIn poleDataIn)
        {
            try
            {
                var pole = await _uow.GetPoleRepository().GetByIdAsync(id);
                if (pole == null)
                    return new ResponsePackage<PoleDataOut?>(null, "Stub nije prona?en");

                // Provera da li regija postoji ako je poslata
                if (poleDataIn.RegionId.HasValue)
                {
                    var region = await _uow.GetRegionRepository().GetByIdAsync(poleDataIn.RegionId.Value);
                    if (region == null)
                        return new ResponsePackage<PoleDataOut?>(null, "Regija nije prona?ena");
                }

                pole.Name = poleDataIn.Name;
                pole.Latitude = poleDataIn.Latitude;
                pole.Longitude = poleDataIn.Longitude;
                pole.Address = poleDataIn.Address;
                pole.PoleType = poleDataIn.PoleType;
                pole.RegionId = poleDataIn.RegionId;
                pole.LastUpdateTime = DateTime.Now;

                await _uow.CompleteAsync();

                var poleDataOut = _mapper.Map<PoleDataOut>(pole);
                return new ResponsePackage<PoleDataOut?>(poleDataOut, "Stub uspe�no a�uriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri a�uriranju stuba sa ID-om {PoleId}", id);
                return new ResponsePackage<PoleDataOut?>(null, "Gre�ka pri a�uriranju stuba");
            }
        }

        public async Task<ResponsePackage<string>> DeletePole(int id)
        {
            try
            {
                var pole = await _uow.GetPoleRepository().GetByIdAsync(id);
                if (pole == null)
                    return new ResponsePackage<string>(null, "Stub nije prona?en");

                _uow.GetPoleRepository().RemoveEntity(pole);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Stub uspe�no obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri brisanju stuba sa ID-om {PoleId}", id);
                return new ResponsePackage<string>(null, "Gre�ka pri brisanju stuba");
            }
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetPoleStatistics()
        {
            try
            {
                var poleCountByType = await _uow.GetPoleRepository().GetPoleCountByTypeAsync();
                var statistics = poleCountByType
                    .Select(kvp => new EntityCountStatisticsDataOut
                    {
                        Name = kvp.Key.ToString(),
                        Count = kvp.Value,
                        Type = "PoleType"
                    })
                    .ToList();

                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(statistics, "Statistika stubova uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju statistike stubova");
                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(null, "Greška pri preuzimanju statistike stubova");
            }
        }
    }
}
