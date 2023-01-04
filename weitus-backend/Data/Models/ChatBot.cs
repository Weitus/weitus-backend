#nullable disable

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
	public class ChatBot
	{
		[Key]
		public int ChatBotId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[JsonIgnore]
		public ICollection<ChatMessage> ChatMessages { get; set; }
	}
}
