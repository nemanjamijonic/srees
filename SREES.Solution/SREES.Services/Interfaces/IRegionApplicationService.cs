using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;

namespace SREES.Services.Interfaces
{
    public interface IRegionApplicationService
    {
        Task<ResponsePackage<List<RegionDataOut>>> GetAllRegions();
        Task<ResponsePackage<List<RegionSelectDataOut>>> GetAllRegionsForSelect();
        Task<ResponsePackage<RegionDataOut?>> GetRegionById(int id);
        Task<ResponsePackage<RegionDataOut?>> CreateRegion(RegionDataIn regionDataIn);
        Task<ResponsePackage<RegionDataOut?>> UpdateRegion(int id, RegionDataIn regionDataIn);
        Task<ResponsePackage<string>> DeleteRegion(int id);
    }
}
