using SREES.Common.Models;
using SREES.Common.Models.Dtos.Poles;

namespace SREES.BLL.Services.Interfaces
{
    public interface IPoleService
    {
        Task<ResponsePackage<List<PoleDataOut>>> GetAllPoles();
        Task<ResponsePackage<List<PoleSelectDataOut>>> GetAllPolesForSelect();
        Task<ResponsePackage<PoleDataOut?>> GetPoleById(int id);
        Task<ResponsePackage<PoleDataOut?>> CreatePole(PoleDataIn poleDataIn);
        Task<ResponsePackage<PoleDataOut?>> UpdatePole(int id, PoleDataIn poleDataIn);
        Task<ResponsePackage<string>> DeletePole(int id);
    }
}
