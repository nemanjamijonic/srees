using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionApplicationService _regionApplicationService;

        public RegionsController(IRegionApplicationService regionApplicationService)
        {
            _regionApplicationService = regionApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<RegionDataOut>>>> GetAllRegions()
        {
            var result = await _regionApplicationService.GetAllRegions();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        public async Task<ActionResult<ResponsePackage<List<RegionSelectDataOut>>>> GetAllRegionsForSelect()
        {
            var result = await _regionApplicationService.GetAllRegionsForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<RegionDataOut?>>> GetRegionById(int id)
        {
            var result = await _regionApplicationService.GetRegionById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsePackage<RegionDataOut?>>> CreateRegion([FromBody] RegionDataIn regionDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _regionApplicationService.CreateRegion(regionDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetRegionById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsePackage<RegionDataOut?>>> UpdateRegion(int id, [FromBody] RegionDataIn regionDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _regionApplicationService.UpdateRegion(id, regionDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteRegion(int id)
        {
            var result = await _regionApplicationService.DeleteRegion(id);
            if (result.Data == null && result.Message!.Contains("nije pronađena"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetRegionStatistics()
        {
            var result = await _regionApplicationService.GetRegionStatistics();
            return Ok(result);
        }
    }
}
