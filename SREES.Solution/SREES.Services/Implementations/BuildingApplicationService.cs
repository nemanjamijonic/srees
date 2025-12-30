using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Buildings;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class BuildingApplicationService : IBuildingApplicationService
    {
        private readonly IBuildingService _buildingService;

        public BuildingApplicationService(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public async Task<ResponsePackage<List<BuildingDataOut>>> GetAllBuildings()
        {
            return await _buildingService.GetAllBuildings();
        }

        public async Task<ResponsePackage<List<BuildingSelectDataOut>>> GetAllBuildingsForSelect()
        {
            return await _buildingService.GetAllBuildingsForSelect();
        }

        public async Task<ResponsePackage<BuildingDataOut?>> GetBuildingById(int id)
        {
            return await _buildingService.GetBuildingById(id);
        }

        public async Task<ResponsePackage<BuildingDataOut?>> CreateBuilding(BuildingDataIn buildingDataIn)
        {
            return await _buildingService.CreateBuilding(buildingDataIn);
        }

        public async Task<ResponsePackage<BuildingDataOut?>> UpdateBuilding(int id, BuildingDataIn buildingDataIn)
        {
            return await _buildingService.UpdateBuilding(id, buildingDataIn);
        }

        public async Task<ResponsePackage<string>> DeleteBuilding(int id)
        {
            return await _buildingService.DeleteBuilding(id);
        }
    }
}
