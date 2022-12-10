using System.ComponentModel.DataAnnotations;

namespace weitus_backend.Data.Dto
{
	public class LoginUser
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
