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
    [Authorize]
    public class OutagesController : ControllerBase
    {
        private readonly IOutageApplicationService _outageApplicationService;

        public OutagesController(IOutageApplicationService outageApplicationService)
        {
            _outageApplicationService = outageApplicationService;
        }

        /// <summary>
        /// Preuzimanje svih prekida napajanja
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<OutageDataOut>>>> GetAllOutages()
        {
            var result = await _outageApplicationService.GetAllOutages();
            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje prekida napajanja po ID-u
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<OutageDataOut?>>> GetOutageById(int id)
        {
            var result = await _outageApplicationService.GetOutageById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Kreiranje novog prekida napajanja
        /// Parametri: userId, regionId, description (opciono)
        /// Status se automatski postavlja na "Reported"
        /// </summary>
        [HttpPost]
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
        /// Mogu?i statusi: Reported, InProgress, Resolved, Cancelled
        /// Ako se postavi Resolved, automatski se postavlja vrijeme razrešavanja
        /// </summary>
        [HttpPut("{id}/status")]
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
        /// Brisanje prekida napajanja (logi?ko brisanje - ozna?avanje kao obrisano)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteOutage(int id)
        {
            var result = await _outageApplicationService.DeleteOutage(id);
            if (result.Data == null && result.Message!.Contains("nije prona?en"))
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje statistike prekida napajanja po statusu
        /// </summary>
        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetOutageStatistics()
        {
            var result = await _outageApplicationService.GetOutageStatistics();
            return Ok(result);
        }
    }
}
