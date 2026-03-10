using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Outages;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutagesController : ControllerBase
    {
        private readonly IOutageApplicationService _outageApplicationService;

        public OutagesController(IOutageApplicationService outageApplicationService)
        {
            _outageApplicationService = outageApplicationService;
        }

        /// <summary>
        /// Preuzimanje svih prekida napajanja
        /// Dostupno: Admin, User, Gost (AllowAnonymous)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<OutageDataOut>>>> GetAllOutages()
        {
            var result = await _outageApplicationService.GetAllOutages();
            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje prekida napajanja po ID-u
        /// Dostupno: Admin, User, Gost
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<OutageDataOut?>>> GetOutageById(int id)
        {
            var result = await _outageApplicationService.GetOutageById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Kreiranje novog prekida napajanja (prijava kvara)
        /// Dostupno: Admin, User (Kupac)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponsePackage<OutageDataOut?>>> CreateOutage([FromBody] OutageDataIn outageDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _outageApplicationService.CreateOutage(outageDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetOutageById), new { id = result.Data.Id }, result);
        }

        /// <summary>
        /// Ažuriranje statusa prekida napajanja
        /// Dostupno: Samo Admin
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<OutageDataOut?>>> UpdateOutageStatus(int id, [FromBody] OutageStatusUpdateDataIn statusUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _outageApplicationService.UpdateOutageStatus(id, statusUpdate);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Brisanje prekida napajanja
        /// Dostupno: Samo Admin
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteOutage(int id)
        {
            var result = await _outageApplicationService.DeleteOutage(id);
            if (result.Data == null && result.Message!.Contains("nije prona?en"))
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje statistike prekida napajanja po statusu
        /// Dostupno: Admin, User, Gost
        /// </summary>
        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetOutageStatistics()
        {
            var result = await _outageApplicationService.GetOutageStatistics();
            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje istorije kvarova za odre?eni Feeder (vod)
        /// Dostupno: Admin, User, Gost
        /// </summary>
        [HttpGet("by-feeder/{feederId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<OutageDataOut>>>> GetOutagesByFeederId(int feederId)
        {
            var result = await _outageApplicationService.GetOutagesByFeederId(feederId);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje istorije kvarova za odre?enu Substation (trafo-stanicu)
        /// Dostupno: Admin, User, Gost
        /// </summary>
        [HttpGet("by-substation/{substationId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<OutageDataOut>>>> GetOutagesBySubstationId(int substationId)
        {
            var result = await _outageApplicationService.GetOutagesBySubstationId(substationId);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }
    }
}
