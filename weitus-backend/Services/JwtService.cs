using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;

namespace weitus_backend.Services
{
	public class JwtService
	{
		private const int EXPIRATION_MINUTES = 1;

		private readonly IConfiguration _configuration;

		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public AuthenticationResponse CreateToken(WeitusUser user)
		{
			var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);

			var token = CreateJwtToken(
				CreateClaims(user),
				CreateSigningCredentials(),
				expiration
			);

			var tokenHandler = new JwtSecurityTokenHandler();

			return new AuthenticationResponse
			{
				Token = tokenHandler.WriteToken(token),
				Expiration = expiration
			};
		}

		private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
			new JwtSecurityToken(
				_configuration["Jwt:Issuer"],
				_configuration["Jwt:Audience"],
				claims,
				expires: expiration,
				signingCredentials: credentials
			);

		public Claim[] CreateClaims(WeitusUser user) =>
			new[] {
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			};
		
		public Claim[] GetClaims(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			return jwtToken.Claims.ToArray();
		}

		public bool ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = CreateValidationParameters(_configuration);

			try
			{
				tokenHandler.ValidateToken(token, validationParameters, out _);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static TokenValidationParameters CreateValidationParameters(IConfiguration configuration) {
			return new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = configuration["Jwt:Issuer"],
				ValidAudience = configuration["Jwt:Issuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
			};
		}

		private SigningCredentials CreateSigningCredentials() =>
			new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
				),
				SecurityAlgorithms.HmacSha256
			);
	}
}
