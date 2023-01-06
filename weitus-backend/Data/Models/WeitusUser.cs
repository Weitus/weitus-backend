#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace weitus_backend.Data.Models
{
    public class WeitusUser
    {
        public const int PasswordHashLength = 64;

        [Key]
        [Precision(8)]
        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(50)]
        [Column("username")]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(PasswordHashLength * 2)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [MaxLength(PasswordHashLength * 2)]
        [Column("password_salt")]
        public string PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
