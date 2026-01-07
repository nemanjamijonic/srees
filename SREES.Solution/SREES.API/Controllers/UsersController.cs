using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.DAL.Models;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserApplicationService _userApplicationService;

        public UsersController(IUserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsePackage<List<User>>>> GetAllUsers()
        {
            var result = await _userApplicationService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsePackage<User?>>> GetUserById(int id)
        {
            var result = await _userApplicationService.GetUserById(id);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsePackage<User?>>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Guid = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.LastUpdateTime = DateTime.UtcNow;

            var result = await _userApplicationService.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsePackage<User?>>> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userApplicationService.UpdateUser(id, user);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteUser(int id)
        {
            var result = await _userApplicationService.DeleteUser(id);
            if (result.Data == null && result.Message!.Contains("nije prona?en"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
