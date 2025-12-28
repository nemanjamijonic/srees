using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;

namespace SREES.BLL.Services.Interfaces
{
    public interface IRegionService
    {
        Task<ResponsePackage<List<RegionDataOut>>> GetAllRegions();
        Task<ResponsePackage<RegionDataOut?>> GetRegionById(int id);
        Task<ResponsePackage<RegionDataOut?>> CreateRegion(RegionDataIn regionDataIn);
        Task<ResponsePackage<RegionDataOut?>> UpdateRegion(int id, RegionDataIn regionDataIn);
        Task<ResponsePackage<string>> DeleteRegion(int id);
    }
}
