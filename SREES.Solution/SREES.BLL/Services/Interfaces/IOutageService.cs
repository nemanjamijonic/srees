using SREES.Common.Models;
using SREES.Common.Models.Dtos.Outages;
using SREES.Common.Models.Dtos.Statistics;

namespace SREES.BLL.Services.Interfaces
{
    public interface IOutageService
    {
        Task<ResponsePackage<List<OutageDataOut>>> GetAllOutages();
        Task<ResponsePackage<OutageDataOut?>> GetOutageById(int id);
        Task<ResponsePackage<OutageDataOut?>> CreateOutage(OutageDataIn outageDataIn);
        Task<ResponsePackage<OutageDataOut?>> UpdateOutageStatus(int id, OutageStatusUpdateDataIn statusUpdate);
        Task<ResponsePackage<string>> DeleteOutage(int id);
        Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetOutageStatistics();
        
        /// <summary>
        /// Preuzima istoriju kvarova za odre?eni Feeder
        /// </summary>
        Task<ResponsePackage<List<OutageDataOut>>> GetOutagesByFeederId(int feederId);
        
        /// <summary>
        /// Preuzima istoriju kvarova za odre?enu Substation
        /// </summary>
        Task<ResponsePackage<List<OutageDataOut>>> GetOutagesBySubstationId(int substationId);
    }
}
