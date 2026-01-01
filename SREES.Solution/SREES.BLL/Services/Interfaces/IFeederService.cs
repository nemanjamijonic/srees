using SREES.Common.Models;
using SREES.Common.Models.Dtos.Feeders;
using SREES.Common.Models.Dtos.Statistics;

namespace SREES.BLL.Services.Interfaces
{
    public interface IFeederService
    {
        Task<ResponsePackage<List<FeederDataOut>>> GetAllFeeders();
        Task<ResponsePackage<List<FeederSelectDataOut>>> GetAllFeedersForSelect();
        Task<ResponsePackage<FeederDataOut?>> GetFeederById(int id);
        Task<ResponsePackage<FeederDataOut?>> CreateFeeder(FeederDataIn feederDataIn);
        Task<ResponsePackage<FeederDataOut?>> UpdateFeeder(int id, FeederDataIn feederDataIn);
        Task<ResponsePackage<string>> DeleteFeeder(int id);
        Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetFeederStatistics();
    }
}
