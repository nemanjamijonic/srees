using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Outages;
using SREES.DAL.UOW.Interafaces;
using OutageModel = SREES.DAL.Models.Outage;

namespace SREES.BLL.Services.Implementation
{
    public class OutageService : IOutageService
    {
        private readonly ILogger<OutageService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OutageService(ILogger<OutageService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<OutageDataOut>>> GetAllOutages()
        {
            try
            {
                var outages = await _uow.GetOutageRepository().GetAllAsync();
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
                var outage = await _uow.GetOutageRepository().GetByIdAsync(id);
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

                var outageModel = new OutageModel
                {
                    UserId = outageDataIn.UserId,
                    RegionId = outageDataIn.RegionId,
                    OutageStatus = OutageStatus.Reported,
                    Description = outageDataIn.Description,
                    Guid = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow
                };

                await _uow.GetOutageRepository().AddAsync(outageModel);
                await _uow.CompleteAsync();

                var outageDataOut = _mapper.Map<OutageDataOut>(outageModel);
                return new ResponsePackage<OutageDataOut?>(outageDataOut, "Prekid napajanja uspešno kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju prekida napajanja");
                return new ResponsePackage<OutageDataOut?>(null, "Greška pri kreiranju prekida napajanja");
            }
        }

        public async Task<ResponsePackage<OutageDataOut?>> UpdateOutageStatus(int id, OutageStatusUpdateDataIn statusUpdate)
        {
            try
            {
                var outage = await _uow.GetOutageRepository().GetByIdAsync(id);
                if (outage == null)
                    return new ResponsePackage<OutageDataOut?>(null, "Prekid napajanja nije prona?en");

                outage.OutageStatus = statusUpdate.NewStatus;
                outage.LastUpdateTime = DateTime.UtcNow;

                // Ako je status postavljen na Resolved, postavi vreme razrešavanja
                if (statusUpdate.NewStatus == OutageStatus.Resolved)
                {
                    outage.ResolvedAt = DateTime.UtcNow;
                }

                await _uow.CompleteAsync();

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
    }
}
