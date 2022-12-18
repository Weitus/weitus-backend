#nullable disable

namespace weitus_backend.Data.Dto
{
	public class AuthenticationResponse
	{
		public string Token { get; set; }

		public DateTime Expiration { get; set; }
	}
}
