using SREES.Common.Models;
using SREES.Common.Models.Dtos.Substations;

namespace SREES.Services.Interfaces
{
    public interface ISubstationApplicationService
    {
        Task<ResponsePackage<List<SubstationDataOut>>> GetAllSubstations();
        Task<ResponsePackage<SubstationDataOut?>> GetSubstationById(int id);
        Task<ResponsePackage<SubstationDataOut?>> CreateSubstation(SubstationDataIn substationDataIn);
        Task<ResponsePackage<SubstationDataOut?>> UpdateSubstation(int id, SubstationDataIn substationDataIn);
        Task<ResponsePackage<string>> DeleteSubstation(int id);
    }
}
