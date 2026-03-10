using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Regions;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje regionima
    /// GET: Dostupno svima (Gost, User, Admin)
    /// POST/PUT/DELETE: Samo Admin
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionApplicationService _regionApplicationService;

        public RegionsController(IRegionApplicationService regionApplicationService)
        {
            _regionApplicationService = regionApplicationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<RegionDataOut>>>> GetAllRegions()
        {
            var result = await _regionApplicationService.GetAllRegions();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<RegionSelectDataOut>>>> GetAllRegionsForSelect()
        {
            var result = await _regionApplicationService.GetAllRegionsForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<RegionDataOut?>>> GetRegionById(int id)
        {
            var result = await _regionApplicationService.GetRegionById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [HttpGet("filtered")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<PaginatedResponse<List<RegionDataOut>>>>> GetRegionsFiltered([FromQuery] RegionFilterRequest filterRequest)
        {
            var result = await _regionApplicationService.GetRegionsFiltered(filterRequest);
            return Ok(result);
        }
    }
}
