using SREES.Common.Models;
using SREES.Common.Models.Dtos.Buildings;

namespace SREES.Services.Interfaces
{
    public interface IBuildingApplicationService
    {
        Task<ResponsePackage<List<BuildingDataOut>>> GetAllBuildings();
        Task<ResponsePackage<List<BuildingSelectDataOut>>> GetAllBuildingsForSelect();
        Task<ResponsePackage<BuildingDataOut?>> GetBuildingById(int id);
        Task<ResponsePackage<BuildingDataOut?>> CreateBuilding(BuildingDataIn buildingDataIn);
        Task<ResponsePackage<BuildingDataOut?>> UpdateBuilding(int id, BuildingDataIn buildingDataIn);
        Task<ResponsePackage<string>> DeleteBuilding(int id);
    }
}
