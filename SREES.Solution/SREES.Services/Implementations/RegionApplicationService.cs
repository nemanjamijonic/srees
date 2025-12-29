using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class RegionApplicationService : IRegionApplicationService
    {
        private readonly IRegionService _regionService;

        public RegionApplicationService(IRegionService regionService)
        {
            _regionService = regionService;
        }

        public async Task<ResponsePackage<List<RegionDataOut>>> GetAllRegions()
        {
            return await _regionService.GetAllRegions();
        }

        public async Task<ResponsePackage<List<RegionSelectDataOut>>> GetAllRegionsForSelect()
        {
            return await _regionService.GetAllRegionsForSelect();
        }

        public async Task<ResponsePackage<RegionDataOut?>> GetRegionById(int id)
        {
            return await _regionService.GetRegionById(id);
        }

        public async Task<ResponsePackage<RegionDataOut?>> CreateRegion(RegionDataIn regionDataIn)
        {
            return await _regionService.CreateRegion(regionDataIn);
        }

        public async Task<ResponsePackage<RegionDataOut?>> UpdateRegion(int id, RegionDataIn regionDataIn)
        {
            return await _regionService.UpdateRegion(id, regionDataIn);
        }

        public async Task<ResponsePackage<string>> DeleteRegion(int id)
        {
            return await _regionService.DeleteRegion(id);
        }
    }
}
