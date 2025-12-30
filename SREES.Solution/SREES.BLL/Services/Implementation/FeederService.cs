using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Feeders;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class FeederService : IFeederService
    {
        private readonly ILogger<FeederService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FeederService(ILogger<FeederService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<FeederDataOut>>> GetAllFeeders()
        {
            try
            {
                var feeders = await _uow.GetFeederRepository().GetAllAsync();
                var feederList = _mapper.Map<List<FeederDataOut>>(feeders.ToList());
                return new ResponsePackage<List<FeederDataOut>>(feederList, "Fideri uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih fidera");
                return new ResponsePackage<List<FeederDataOut>>(null, "Greška pri preuzimanju fidera");
            }
        }

        public async Task<ResponsePackage<List<FeederSelectDataOut>>> GetAllFeedersForSelect()
        {
            try
            {
                var feeders = await _uow.GetFeederRepository().GetAllAsync();
                var feederSelectList = _mapper.Map<List<FeederSelectDataOut>>(feeders.ToList());
                return new ResponsePackage<List<FeederSelectDataOut>>(feederSelectList, "Fideri za select uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju fidera za select");
                return new ResponsePackage<List<FeederSelectDataOut>>(null, "Greška pri preuzimanju fidera za select");
            }
        }

        public async Task<ResponsePackage<FeederDataOut?>> GetFeederById(int id)
        {
            try
            {
                var feeder = await _uow.GetFeederRepository().GetByIdAsync(id);
                if (feeder == null)
                    return new ResponsePackage<FeederDataOut?>(null, "Fider nije pronađen");

                var feederDataOut = _mapper.Map<FeederDataOut>(feeder);
                return new ResponsePackage<FeederDataOut?>(feederDataOut, "Fider uspešno preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju fidera sa ID-om {FeederId}", id);
                return new ResponsePackage<FeederDataOut?>(null, "Greška pri preuzimanju fidera");
            }
        }

        public async Task<ResponsePackage<FeederDataOut?>> CreateFeeder(FeederDataIn feederDataIn)
        {
            try
            {
                // Provera da li substation postoji ako je poslat
                if (feederDataIn.SubstationId.HasValue)
                {
                    var substation = await _uow.GetSubstationRepository().GetByIdAsync(feederDataIn.SubstationId.Value);
                    if (substation == null)
                        return new ResponsePackage<FeederDataOut?>(null, "Transformatorska stanica nije pronađena");
                }

                var feeder = new Feeder
                {
                    Name = feederDataIn.Name,
                    FeederType = feederDataIn.FeederType,
                    SubstationId = feederDataIn.SubstationId,
                    SuppliedRegions = feederDataIn.SuppliedRegions
                };

                await _uow.GetFeederRepository().AddAsync(feeder);
                await _uow.CompleteAsync();

                var feederDataOut = _mapper.Map<FeederDataOut>(feeder);
                return new ResponsePackage<FeederDataOut?>(feederDataOut, "Fider uspešno kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju fidera");
                return new ResponsePackage<FeederDataOut?>(null, "Greška pri kreiranju fidera");
            }
        }

        public async Task<ResponsePackage<FeederDataOut?>> UpdateFeeder(int id, FeederDataIn feederDataIn)
        {
            try
            {
                var feeder = await _uow.GetFeederRepository().GetByIdAsync(id);
                if (feeder == null)
                    return new ResponsePackage<FeederDataOut?>(null, "Fider nije pronađen");

                // Provera da li substation postoji ako je poslat
                if (feederDataIn.SubstationId.HasValue)
                {
                    var substation = await _uow.GetSubstationRepository().GetByIdAsync(feederDataIn.SubstationId.Value);
                    if (substation == null)
                        return new ResponsePackage<FeederDataOut?>(null, "Transformatorska stanica nije pronađena");
                }

                feeder.Name = feederDataIn.Name;
                feeder.FeederType = feederDataIn.FeederType;
                feeder.SubstationId = feederDataIn.SubstationId;
                feeder.SuppliedRegions = feederDataIn.SuppliedRegions;
                feeder.LastUpdateTime = DateTime.Now;

                await _uow.CompleteAsync();

                var feederDataOut = _mapper.Map<FeederDataOut>(feeder);
                return new ResponsePackage<FeederDataOut?>(feederDataOut, "Fider uspešno ažuriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju fidera sa ID-om {FeederId}", id);
                return new ResponsePackage<FeederDataOut?>(null, "Greška pri ažuriranju fidera");
            }
        }

        public async Task<ResponsePackage<string>> DeleteFeeder(int id)
        {
            try
            {
                var feeder = await _uow.GetFeederRepository().GetByIdAsync(id);
                if (feeder == null)
                    return new ResponsePackage<string>(null, "Fider nije pronađen");

                _uow.GetFeederRepository().RemoveEntity(feeder);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Fider uspešno obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju fidera sa ID-om {FeederId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju fidera");
            }
        }
    }
}
