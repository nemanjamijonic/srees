using SREES.Common.Models;
using SREES.Common.Models.Dtos.Feeders;

namespace SREES.Services.Interfaces
{
    public interface IFeederApplicationService
    {
        Task<ResponsePackage<List<FeederDataOut>>> GetAllFeeders();
        Task<ResponsePackage<List<FeederSelectDataOut>>> GetAllFeedersForSelect();
        Task<ResponsePackage<FeederDataOut?>> GetFeederById(int id);
        Task<ResponsePackage<FeederDataOut?>> CreateFeeder(FeederDataIn feederDataIn);
        Task<ResponsePackage<FeederDataOut?>> UpdateFeeder(int id, FeederDataIn feederDataIn);
        Task<ResponsePackage<string>> DeleteFeeder(int id);
    }
}
