#nullable disable

using System.ComponentModel.DataAnnotations;

namespace weitus_backend.Data.Dto
{
	public class SendBotChatMessage
	{
		[Required]
		[MaxLength(500)]
		public string Message { get; set; }

		public int? BotId { get; set; }
	}
}
