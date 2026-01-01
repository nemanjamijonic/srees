using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Feeders;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class FeederApplicationService : IFeederApplicationService
    {
        private readonly IFeederService _feederService;

        public FeederApplicationService(IFeederService feederService)
        {
            _feederService = feederService;
        }

        public async Task<ResponsePackage<List<FeederDataOut>>> GetAllFeeders()
        {
            return await _feederService.GetAllFeeders();
        }

        public async Task<ResponsePackage<List<FeederSelectDataOut>>> GetAllFeedersForSelect()
        {
            return await _feederService.GetAllFeedersForSelect();
        }

        public async Task<ResponsePackage<FeederDataOut?>> GetFeederById(int id)
        {
            return await _feederService.GetFeederById(id);
        }

        public async Task<ResponsePackage<FeederDataOut?>> CreateFeeder(FeederDataIn feederDataIn)
        {
            return await _feederService.CreateFeeder(feederDataIn);
        }

        public async Task<ResponsePackage<FeederDataOut?>> UpdateFeeder(int id, FeederDataIn feederDataIn)
        {
            return await _feederService.UpdateFeeder(id, feederDataIn);
        }

        public async Task<ResponsePackage<string>> DeleteFeeder(int id)
        {
            return await _feederService.DeleteFeeder(id);
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetFeederStatistics()
        {
            return await _feederService.GetFeederStatistics();
        }
    }
}
