using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Thoughts.Domain.Entities;
using Thoughts.Core.ConfigModels;
using Microsoft.AspNetCore.WebUtilities;
using Logistics.Domain.Exceptions;

namespace Thoughts.Core.Services
{
	public class TokenService
	{
		private readonly AppSettings _appSettings;

		public TokenService(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}
		
		public string CreateToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.AuthTokenKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);
			
			return tokenHandler.WriteToken(token);
		}

		public DateTime GetExpirationDate()
		{
			return DateTime.UtcNow.AddDays(1);
		}

		public string DecodeToken(string token)
		{
			var decodedToken = WebEncoders.Base64UrlDecode(token);
			string tokenString = Encoding.UTF8.GetString(decodedToken);

			return tokenString;
		}

		public string EncodeToken(string token)
		{
			var encodedToken = Encoding.UTF8.GetBytes(token);
			var validToken = WebEncoders.Base64UrlEncode(encodedToken);

			return validToken;
		}

		public string ValidateAndDecodeToken(string token)
		{
			if (token == null)
				throw new ValidationException("Token is required");
			var decodedToken = DecodeToken(token);
			
			return decodedToken;
		}
	}
}
