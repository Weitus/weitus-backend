#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
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
        [Required]
        public int UserId { get; set; }

        [MaxLength(50)]
        [Column("username")]
        [Required]
        public string UserName { get; set; }

        [Column("email")]
        [Required]
        public string EncryptedEmail { get; set; }

        [MaxLength(PasswordHashLength * 2)]
        [Column("password_hash")]
        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(PasswordHashLength * 2)]
        [Column("password_salt")]
        [Required]
        public string PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<ChatMessage> ChatMessages { get; set; }

        public string GetDecryptedEmail(byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var encrypted = Convert.FromBase64String(EncryptedEmail);
                var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        public void SetEncryptedEmail(string email, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var decrypted = Encoding.UTF8.GetBytes(email);
                var encrypted = encryptor.TransformFinalBlock(decrypted, 0, decrypted.Length);

                this.EncryptedEmail = Convert.ToBase64String(encrypted);
            }
        }
    }
}
