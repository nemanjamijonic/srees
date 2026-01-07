using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Auth;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserApplicationService _userApplicationService;

        public AuthController(IUserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        /// <summary>
        /// Login endpoint - returns JWT token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<LoginResponse?>>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userApplicationService.Login(loginRequest);
            
            if (result.Data == null)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Register endpoint - creates new user and returns JWT token
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponsePackage<LoginResponse?>>> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userApplicationService.Register(registerRequest);
            
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(Login), result);
        }
    }
}
