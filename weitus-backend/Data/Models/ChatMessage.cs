#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace weitus_backend.Data.Models
{
    public class ChatMessage
    {
        [Key]
        [Precision(10)]
        [Column("chat_message_id")]
        public int ChatMessageId { get; set; }

        [Required]
        [Column("timestamp")]
        public DateTime TimeStamp { get; set; }

        [StringLength(500)]
        [ProtectedPersonalData]
        [Required]
        [Column("message")]
        public string Message { get; set; }

        [Required]
        [Column("chatter_id")]
        public int ChatterId { get; set; }

        [JsonIgnore]
        public WeitusUser Chatter { get; set; }

        [Column("bot_id")]
        public int? BotId { get; set; }

        [JsonIgnore]
        public ChatBot Bot { get; set; }
    }
}
