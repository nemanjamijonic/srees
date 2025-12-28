using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class RegionService : IRegionService
    {
        private readonly ILogger<RegionService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RegionService(ILogger<RegionService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<RegionDataOut>>> GetAllRegions()
        {
            try
            {
                var regions = await _uow.GetRegionRepository().GetAllAsync();
                var regionList = _mapper.Map<List<RegionDataOut>>(regions.ToList());
                return new ResponsePackage<List<RegionDataOut>>(regionList, "Regije uspešno preuzete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih regija");
                return new ResponsePackage<List<RegionDataOut>>(null, "Greška pri preuzimanju regija");
            }
        }

        public async Task<ResponsePackage<RegionDataOut?>> GetRegionById(int id)
        {
            try
            {
                var region = await _uow.GetRegionRepository().GetByIdAsync(id);
                if (region == null)
                    return new ResponsePackage<RegionDataOut?>(null, "Regija nije pronađena");

                var regionDataOut = _mapper.Map<RegionDataOut>(region);
                return new ResponsePackage<RegionDataOut?>(regionDataOut, "Regija uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju regije sa ID-om {RegionId}", id);
                return new ResponsePackage<RegionDataOut?>(null, "Greška pri preuzimanju regije");
            }
        }

        public async Task<ResponsePackage<RegionDataOut?>> CreateRegion(RegionDataIn regionDataIn)
        {
            try
            {
                var region = new Region
                {
                    Name = regionDataIn.Name,
                    Latitude = regionDataIn.Latitude,   
                    Longitude = regionDataIn.Longitude
                };

                await _uow.GetRegionRepository().AddAsync(region);
                await _uow.CompleteAsync();

                var regionDataOut = _mapper.Map<RegionDataOut>(region);
                return new ResponsePackage<RegionDataOut?>(regionDataOut, "Regija uspešno kreirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju regije");
                return new ResponsePackage<RegionDataOut?>(null, "Greška pri kreiranju regije");
            }
        }

        public async Task<ResponsePackage<RegionDataOut?>> UpdateRegion(int id, RegionDataIn regionDataIn)
        {
            try
            {
                var region = await _uow.GetRegionRepository().GetByIdAsync(id);
                if (region == null)
                    return new ResponsePackage<RegionDataOut?>(null, "Regija nije pronađena");

                region.Name = regionDataIn.Name;
                region.Latitude = regionDataIn.Latitude;
                region.Longitude = regionDataIn.Longitude;

                await _uow.CompleteAsync();

                var regionDataOut = _mapper.Map<RegionDataOut>(region);
                return new ResponsePackage<RegionDataOut?>(regionDataOut, "Regija uspešno ažurirana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju regije sa ID-om {RegionId}", id);
                return new ResponsePackage<RegionDataOut?>(null, "Greška pri ažuriranju regije");
            }
        }

        public async Task<ResponsePackage<string>> DeleteRegion(int id)
        {
            try
            {
                var region = await _uow.GetRegionRepository().GetByIdAsync(id);
                if (region == null)
                    return new ResponsePackage<string>(null, "Regija nije pronađena");

                _uow.GetRegionRepository().RemoveEntity(region);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Regija uspešno obrisana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju regije sa ID-om {RegionId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju regije");
            }
        }
    }
}
