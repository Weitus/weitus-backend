using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data.Dto;
using weitus_backend.Services;

namespace weitus_backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager _userManager;

        public AuthController(ILogger<AuthController> logger, UserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var result = await _userManager.RegisterUserAsync(registerUser);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest((ErrorResponse)result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var authResponse = await _userManager.LoginUserAsync(loginUser);

            if (!authResponse.Success)
            {
                return BadRequest((ErrorResponse)authResponse);
            }

            return Ok((AuthenticationResponse)authResponse);
        }
    }
}
