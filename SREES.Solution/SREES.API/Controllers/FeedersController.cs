using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Feeders;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje fiderima (vodovima)
    /// GET: Dostupno svima (Gost, User, Admin)
    /// POST/PUT/DELETE: Samo Admin
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FeedersController : ControllerBase
    {
        private readonly IFeederApplicationService _feederApplicationService;

        public FeedersController(IFeederApplicationService feederApplicationService)
        {
            _feederApplicationService = feederApplicationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<FeederDataOut>>>> GetAllFeeders()
        {
            var result = await _feederApplicationService.GetAllFeeders();
            return Ok(result);
        }

        [HttpGet("filtered")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<PaginatedResponse<List<FeederDataOut>>>>> GetFeedersFiltered([FromQuery] FeederFilterRequest filterRequest)
        {
            var result = await _feederApplicationService.GetFeedersFiltered(filterRequest);
            return Ok(result);
        }

        [HttpGet("select")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<FeederSelectDataOut>>>> GetAllFeedersForSelect()
        {
            var result = await _feederApplicationService.GetAllFeedersForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<FeederDataOut?>>> GetFeederById(int id)
        {
            var result = await _feederApplicationService.GetFeederById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<FeederDataOut?>>> CreateFeeder([FromBody] FeederDataIn feederDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _feederApplicationService.CreateFeeder(feederDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetFeederById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<FeederDataOut?>>> UpdateFeeder(int id, [FromBody] FeederDataIn feederDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _feederApplicationService.UpdateFeeder(id, feederDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteFeeder(int id)
        {
            var result = await _feederApplicationService.DeleteFeeder(id);
            if (result.Data == null && result.Message.Contains("nije prona?en"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetFeederStatistics()
        {
            var result = await _feederApplicationService.GetFeederStatistics();
            return Ok(result);
        }
    }
}
