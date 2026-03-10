using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Common.Models.Dtos.Substations;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje trafo-stanicama
    /// GET: Dostupno svima (Gost, User, Admin)
    /// POST/PUT/DELETE: Samo Admin
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SubstationsController : ControllerBase
    {
        private readonly ISubstationApplicationService _substationApplicationService;

        public SubstationsController(ISubstationApplicationService substationApplicationService)
        {
            _substationApplicationService = substationApplicationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<SubstationDataOut>>>> GetAllSubstations()
        {
            var result = await _substationApplicationService.GetAllSubstations();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<SubstationSelectDataOut>>>> GetAllSubstationsForSelect()
        {
            var result = await _substationApplicationService.GetAllSubstationsForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<SubstationDataOut?>>> GetSubstationById(int id)
        {
            var result = await _substationApplicationService.GetSubstationById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<SubstationDataOut?>>> CreateSubstation([FromBody] SubstationDataIn substationDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _substationApplicationService.CreateSubstation(substationDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetSubstationById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<SubstationDataOut?>>> UpdateSubstation(int id, [FromBody] SubstationDataIn substationDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _substationApplicationService.UpdateSubstation(id, substationDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteSubstation(int id)
        {
            var result = await _substationApplicationService.DeleteSubstation(id);
            if (result.Data == null && result.Message!.Contains("nije pronađena"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetSubstationStatistics()
        {
            var result = await _substationApplicationService.GetSubstationStatistics();
            return Ok(result);
        }

        [HttpGet("filtered")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<PaginatedResponse<List<SubstationDataOut>>>>> GetSubstationsFiltered([FromQuery] SubstationFilterRequest filterRequest)
        {
            var result = await _substationApplicationService.GetSubstationsFiltered(filterRequest);
            return Ok(result);
        }
    }
}
