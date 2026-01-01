using SREES.Common.Models;
using SREES.Common.Models.Dtos.Substations;
using SREES.Common.Models.Dtos.Statistics;

namespace SREES.BLL.Services.Interfaces
{
    public interface ISubstationService
    {
        Task<ResponsePackage<List<SubstationDataOut>>> GetAllSubstations();
        Task<ResponsePackage<List<SubstationSelectDataOut>>> GetAllSubstationsForSelect();
        Task<ResponsePackage<SubstationDataOut?>> GetSubstationById(int id);
        Task<ResponsePackage<SubstationDataOut?>> CreateSubstation(SubstationDataIn substationDataIn);
        Task<ResponsePackage<SubstationDataOut?>> UpdateSubstation(int id, SubstationDataIn substationDataIn);
        Task<ResponsePackage<string>> DeleteSubstation(int id);
        Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetSubstationStatistics();
    }
}
