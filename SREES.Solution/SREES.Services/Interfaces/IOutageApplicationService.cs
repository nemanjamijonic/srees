using SREES.Common.Models;
using SREES.Common.Models.Dtos.Outages;

namespace SREES.Services.Interfaces
{
    public interface IOutageApplicationService
    {
        Task<ResponsePackage<List<OutageDataOut>>> GetAllOutages();
        Task<ResponsePackage<OutageDataOut?>> GetOutageById(int id);
        Task<ResponsePackage<OutageDataOut?>> CreateOutage(OutageDataIn outageDataIn);
        Task<ResponsePackage<OutageDataOut?>> UpdateOutageStatus(int id, OutageStatusUpdateDataIn statusUpdate);
        Task<ResponsePackage<string>> DeleteOutage(int id);
    }
}
