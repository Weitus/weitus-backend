#nullable disable

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
    public class ChatBot
    {
        [Key]
        [Precision(5)]
        [Column("bot_id")]
        public Int16 ChatBotId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
