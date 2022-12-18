#nullable disable

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
	public class WeitusUser : IdentityUser
	{
		[JsonIgnore]
		public ICollection<ChatMessage> ChatMessages { get; set; }
	}
}
