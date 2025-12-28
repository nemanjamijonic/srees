using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Substations;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class SubstationService : ISubstationService
    {
        private readonly ILogger<SubstationService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SubstationService(ILogger<SubstationService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<SubstationDataOut>>> GetAllSubstations()
        {
            try
            {
                var substations = await _uow.GetSubstationRepository().GetAllAsync();
                var substationList = _mapper.Map<List<SubstationDataOut>>(substations.ToList());
                return new ResponsePackage<List<SubstationDataOut>>(substationList, "Transformatorske stanice uspešno preuzete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih transformatorskih stanica");
                return new ResponsePackage<List<SubstationDataOut>>(null, "Greška pri preuzimanju transformatorskih stanica");
            }
        }

        public async Task<ResponsePackage<SubstationDataOut?>> GetSubstationById(int id)
        {
            try
            {
                var substation = await _uow.GetSubstationRepository().GetByIdAsync(id);
                if (substation == null)
                    return new ResponsePackage<SubstationDataOut?>(null, "Transformatorska stanica nije prona?ena");

                var substationDataOut = _mapper.Map<SubstationDataOut>(substation);
                return new ResponsePackage<SubstationDataOut?>(substationDataOut, "Transformatorska stanica uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju transformatorske stanice sa ID-om {SubstationId}", id);
                return new ResponsePackage<SubstationDataOut?>(null, "Greška pri preuzimanju transformatorske stanice");
            }
        }

        public async Task<ResponsePackage<SubstationDataOut?>> CreateSubstation(SubstationDataIn substationDataIn)
        {
            try
            {
                // Provera da li regija postoji
                var region = await _uow.GetRegionRepository().GetByIdAsync(substationDataIn.RegionId);
                if (region == null)
                    return new ResponsePackage<SubstationDataOut?>(null, "Regija nije prona?ena");

                var substation = new Substation
                {
                    SubstationType = substationDataIn.SubstationType,
                    Latitude = substationDataIn.Latitude,
                    Longitude = substationDataIn.Longitude,
                    Name = substationDataIn.Name,
                    RegionId = substationDataIn.RegionId,
                    Guid = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow
                };

                await _uow.GetSubstationRepository().AddAsync(substation);
                await _uow.CompleteAsync();

                var substationDataOut = _mapper.Map<SubstationDataOut>(substation);
                return new ResponsePackage<SubstationDataOut?>(substationDataOut, "Transformatorska stanica uspešno kreirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju transformatorske stanice");
                return new ResponsePackage<SubstationDataOut?>(null, "Greška pri kreiranju transformatorske stanice");
            }
        }

        public async Task<ResponsePackage<SubstationDataOut?>> UpdateSubstation(int id, SubstationDataIn substationDataIn)
        {
            try
            {
                var substation = await _uow.GetSubstationRepository().GetByIdAsync(id);
                if (substation == null)
                    return new ResponsePackage<SubstationDataOut?>(null, "Transformatorska stanica nije prona?ena");

                // Provera da li regija postoji
                var region = await _uow.GetRegionRepository().GetByIdAsync(substationDataIn.RegionId);
                if (region == null)
                    return new ResponsePackage<SubstationDataOut?>(null, "Regija nije prona?ena");

                substation.SubstationType = substationDataIn.SubstationType;
                substation.Latitude = substationDataIn.Latitude;
                substation.Longitude = substationDataIn.Longitude;
                substation.Name = substationDataIn.Name;
                substation.RegionId = substationDataIn.RegionId;
                substation.LastUpdateTime = DateTime.UtcNow;

                await _uow.CompleteAsync();

                var substationDataOut = _mapper.Map<SubstationDataOut>(substation);
                return new ResponsePackage<SubstationDataOut?>(substationDataOut, "Transformatorska stanica uspešno ažurirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju transformatorske stanice sa ID-om {SubstationId}", id);
                return new ResponsePackage<SubstationDataOut?>(null, "Greška pri ažuriranju transformatorske stanice");
            }
        }

        public async Task<ResponsePackage<string>> DeleteSubstation(int id)
        {
            try
            {
                var substation = await _uow.GetSubstationRepository().GetByIdAsync(id);
                if (substation == null)
                    return new ResponsePackage<string>(null, "Transformatorska stanica nije prona?ena");

                _uow.GetSubstationRepository().RemoveEntity(substation);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Transformatorska stanica uspešno obrisana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju transformatorske stanice sa ID-om {SubstationId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju transformatorske stanice");
            }
        }
    }
}
