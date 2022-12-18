#nullable disable

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
	public class ChatMessage
	{
		[Key]
		public int Id { get; set; }

		public DateTime TimeStamp { get; set; }

		[StringLength(500)]
		[ProtectedPersonalData]
		public string Message { get; set; }

		public string ChatterId { get; set; }

		[JsonIgnore]
		public WeitusUser Chatter { get; set; }

		public bool SentByBot { get; set; }
	}
}
