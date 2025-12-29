using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Poles;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class PoleApplicationService : IPoleApplicationService
    {
        private readonly IPoleService _poleService;

        public PoleApplicationService(IPoleService poleService)
        {
            _poleService = poleService;
        }

        public async Task<ResponsePackage<List<PoleDataOut>>> GetAllPoles()
        {
            return await _poleService.GetAllPoles();
        }

        public async Task<ResponsePackage<List<PoleSelectDataOut>>> GetAllPolesForSelect()
        {
            return await _poleService.GetAllPolesForSelect();
        }

        public async Task<ResponsePackage<PoleDataOut?>> GetPoleById(int id)
        {
            return await _poleService.GetPoleById(id);
        }

        public async Task<ResponsePackage<PoleDataOut?>> CreatePole(PoleDataIn poleDataIn)
        {
            return await _poleService.CreatePole(poleDataIn);
        }

        public async Task<ResponsePackage<PoleDataOut?>> UpdatePole(int id, PoleDataIn poleDataIn)
        {
            return await _poleService.UpdatePole(id, poleDataIn);
        }

        public async Task<ResponsePackage<string>> DeletePole(int id)
        {
            return await _poleService.DeletePole(id);
        }
    }
}
