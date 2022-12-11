using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;

namespace weitus_backend.Controllers
{
	[ApiController]
	[Route("chat")]
	public class ChatController : ControllerBase
	{
		private readonly ILogger<ChatController> _logger;
		private readonly IWeitusRepository _repo;
		private readonly UserManager<WeitusUser> _userManager;

		public ChatController(ILogger<ChatController> logger, IWeitusRepository repo, UserManager<WeitusUser> userManager)
		{
			_logger = logger;
			_repo = repo;
			_userManager = userManager;
		}

		[Authorize]
		[HttpPost("message")]
		public async Task<IActionResult> SendMessage([FromBody] SendChatMessage chatMessageDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

            foreach (var claim in User.Claims)
            {
                Console.WriteLine(claim.Type + ": " + claim.Value);
            }

			var user = await _userManager.GetUserAsync(User);

			var chatMessage = new ChatMessage
			{
				ChatterId = user.Id,
				Message = chatMessageDto.Message,
				SentByBot = false,
				TimeStamp = DateTime.Now
			};

			await _repo.Add(chatMessage);

			return Ok(chatMessage);
		}

		[Authorize]
		[HttpGet("messages")]
		public async Task<IActionResult> GetMessages()
		{
			var user = await _userManager.GetUserAsync(User);
			
			var messages = await _repo.GetMessageLog(user);

			return Ok(messages);
		}
	}
}
