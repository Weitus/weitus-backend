#nullable disable

namespace weitus_backend.Data.Dto
{
	public class AuthenticationResponse
	{
		public string Token { get; set; }

		public string ChatIdentifier { get; set; }

		public DateTime Expiration { get; set; }
	}
}
