using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Poles;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje stubovima
    /// GET: Dostupno svima (Gost, User, Admin)
    /// POST/PUT/DELETE: Samo Admin
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PolesController : ControllerBase
    {
        private readonly IPoleApplicationService _poleApplicationService;

        public PolesController(IPoleApplicationService poleApplicationService)
        {
            _poleApplicationService = poleApplicationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<PoleDataOut>>>> GetAllPoles()
        {
            var result = await _poleApplicationService.GetAllPoles();
            return Ok(result);
        }

        [HttpGet("getAllForSelect")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<PoleSelectDataOut>>>> GetAllPolesForSelect()
        {
            var result = await _poleApplicationService.GetAllPolesForSelect();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<PoleDataOut?>>> GetPoleById(int id)
        {
            var result = await _poleApplicationService.GetPoleById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<PoleDataOut?>>> CreatePole([FromBody] PoleDataIn poleDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _poleApplicationService.CreatePole(poleDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetPoleById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<PoleDataOut?>>> UpdatePole(int id, [FromBody] PoleDataIn poleDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _poleApplicationService.UpdatePole(id, poleDataIn);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<string>>> DeletePole(int id)
        {
            var result = await _poleApplicationService.DeletePole(id);
            if (result.Data == null && result.Message!.Contains("nije pronađen"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<List<EntityCountStatisticsDataOut>>>> GetPoleStatistics()
        {
            var result = await _poleApplicationService.GetPoleStatistics();
            return Ok(result);
        }

        [HttpGet("filtered")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<PaginatedResponse<List<PoleDataOut>>>>> GetPolesFiltered([FromQuery] PoleFilterRequest filterRequest)
        {
            var result = await _poleApplicationService.GetPolesFiltered(filterRequest);
            return Ok(result);
        }
    }
}
