using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Outages;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class OutageApplicationService : IOutageApplicationService
    {
        private readonly IOutageService _outageService;

        public OutageApplicationService(IOutageService outageService)
        {
            _outageService = outageService;
        }

        public async Task<ResponsePackage<List<OutageDataOut>>> GetAllOutages()
        {
            return await _outageService.GetAllOutages();
        }

        public async Task<ResponsePackage<OutageDataOut?>> GetOutageById(int id)
        {
            return await _outageService.GetOutageById(id);
        }

        public async Task<ResponsePackage<OutageDataOut?>> CreateOutage(OutageDataIn outageDataIn)
        {
            return await _outageService.CreateOutage(outageDataIn);
        }

        public async Task<ResponsePackage<OutageDataOut?>> UpdateOutageStatus(int id, OutageStatusUpdateDataIn statusUpdate)
        {
            return await _outageService.UpdateOutageStatus(id, statusUpdate);
        }

        public async Task<ResponsePackage<string>> DeleteOutage(int id)
        {
            return await _outageService.DeleteOutage(id);
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetOutageStatistics()
        {
            return await _outageService.GetOutageStatistics();
        }
    }
}
