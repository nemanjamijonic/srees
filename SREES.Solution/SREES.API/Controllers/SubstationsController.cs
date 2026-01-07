using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Common.Models.Dtos.Substations;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubstationsController : ControllerBase
    {
        private readonly ISubstationApplicationService _substationApplicationService;

        public SubstationsController(ISubstationApplicationService substationApplicationService)
        {
            _substationApplicationService = substationApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<SubstationDataOut>>>> GetAllSubstations()
        {
            var result = await _substationApplicationService.GetAllSubstations();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        public async Task<ActionResult<ResponsePackage<List<SubstationSelectDataOut>>>> GetAllSubstationsForSelect()
        {
            var result = await _substationApplicationService.GetAllSubstationsForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<SubstationDataOut?>>> GetSubstationById(int id)
        {
            var result = await _substationApplicationService.GetSubstationById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
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
    }
}
