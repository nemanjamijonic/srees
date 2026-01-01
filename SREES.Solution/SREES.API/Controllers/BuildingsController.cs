using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Buildings;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingApplicationService _buildingApplicationService;

        public BuildingsController(IBuildingApplicationService buildingApplicationService)
        {
            _buildingApplicationService = buildingApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<BuildingDataOut>>>> GetAllBuildings()
        {
            var result = await _buildingApplicationService.GetAllBuildings();
            return Ok(result);
        }

        [HttpGet("select")]
        public async Task<ActionResult<ResponsePackage<List<BuildingSelectDataOut>>>> GetAllBuildingsForSelect()
        {
            var result = await _buildingApplicationService.GetAllBuildingsForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<BuildingDataOut?>>> GetBuildingById(int id)
        {
            var result = await _buildingApplicationService.GetBuildingById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsePackage<BuildingDataOut?>>> CreateBuilding([FromBody] BuildingDataIn buildingDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _buildingApplicationService.CreateBuilding(buildingDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetBuildingById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsePackage<BuildingDataOut?>>> UpdateBuilding(int id, [FromBody] BuildingDataIn buildingDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _buildingApplicationService.UpdateBuilding(id, buildingDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteBuilding(int id)
        {
            var result = await _buildingApplicationService.DeleteBuilding(id);
            if (result.Data == null && result.Message.Contains("nije prona?ena"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetBuildingStatistics()
        {
            var result = await _buildingApplicationService.GetBuildingStatistics();
            return Ok(result);
        }
    }
}
