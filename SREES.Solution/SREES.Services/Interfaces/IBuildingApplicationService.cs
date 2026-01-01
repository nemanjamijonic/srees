using SREES.Common.Models;
using SREES.Common.Models.Dtos.Buildings;
using SREES.Common.Models.Dtos.Statistics;

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
        Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetBuildingStatistics();
    }
}
