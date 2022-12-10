using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;
using weitus_backend.Services;

namespace weitus_backend.Controllers
{
	[ApiController]
	[Route("auth")]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;
		private readonly UserManager<WeitusUser> _userManager;
		private readonly JwtService _jwtService;

		public AuthController(ILogger<AuthController> logger, UserManager<WeitusUser> userManager, JwtService jwtService)
		{
			_logger = logger;
			_userManager = userManager;
			_jwtService = jwtService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			
			var user = new WeitusUser
			{
				UserName = registerUser.UserName,
				Email = registerUser.Email
			};

			var result = await _userManager.CreateAsync(user, registerUser.Password);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Bad credentials.");
			}

			var user = await _userManager.FindByNameAsync(loginUser.UserName);

			if (user == null)
			{
				return BadRequest("Bad credentials");
			}

			var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUser.Password);

			if (!isPasswordValid)
			{
				return BadRequest("Bad credentials");
			}

			var token = _jwtService.CreateToken(user);

			return Ok(token);
		}
	}
}