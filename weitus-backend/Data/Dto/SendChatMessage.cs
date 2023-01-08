#nullable disable

using System.ComponentModel.DataAnnotations;

namespace weitus_backend.Data.Dto
{
	public class SendChatMessage
	{
		[Required]
		[MaxLength(500)]
		public string Message { get; set; }

		[Required]
		public int BotId { get; set; }
	}
}
