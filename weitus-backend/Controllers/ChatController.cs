using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;
using weitus_backend.Services;

namespace weitus_backend.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IWeitusRepository _repo;
        private readonly UserManager _userManager;

        public ChatController(ILogger<ChatController> logger, IWeitusRepository repo, UserManager userManager)
        {
            _logger = logger;
            _repo = repo;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] SendChatMessage chatMessageDto)
        {
            if (chatMessageDto.BotId == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "BotId is required"
                });
            }

            if (await _repo.GetChatBot(chatMessageDto.BotId.Value) == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "BotId is invalid"
                });
            }

            var user = await _userManager.GetUserAsync(User);

            var chatMessage = new ChatMessage
            {
                ChatterId = user.UserId,
                Message = chatMessageDto.Message,
                BotId = chatMessageDto.BotId.Value,
                SentByBot = false,
                TimeStamp = DateTime.Now
            };

            await _repo.Add(chatMessage);

            return Ok(chatMessage);
        }

        [Authorize]
        [HttpPost("botMessage")]
        public async Task<IActionResult> RegisterBotMessage([FromBody] SendChatMessage chatMessageDto)
        {
            if (chatMessageDto.BotId == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "BotId is required"
                });
            }

            if (await _repo.GetChatBot(chatMessageDto.BotId.Value) == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "BotId is invalid"
                });
            }

            var user = await _userManager.GetUserAsync(User);

            var chatMessage = new ChatMessage
            {
                ChatterId = user.UserId,
                Message = chatMessageDto.Message,
                BotId = chatMessageDto.BotId.Value,
                SentByBot = true,
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
