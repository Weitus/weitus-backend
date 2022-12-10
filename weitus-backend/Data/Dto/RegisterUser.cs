using System.ComponentModel.DataAnnotations;

namespace weitus_backend.Data.Dto
{
	public class RegisterUser
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
