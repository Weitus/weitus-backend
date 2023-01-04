#nullable disable

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
	public class ChatMessage
	{
		[Key]
		public int ChatMessageId { get; set; }

		public DateTime TimeStamp { get; set; }

		[StringLength(500)]
		[ProtectedPersonalData]
		[Required]
		public string Message { get; set; }

		[Required]
		public string ChatterId { get; set; }

		[JsonIgnore]
		public WeitusUser Chatter { get; set; }

		public int? BotId { get; set; }

		[JsonIgnore]
		public ChatBot Bot { get; set; }
	}
}
