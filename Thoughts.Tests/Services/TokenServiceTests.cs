using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Logistics.Domain.Exceptions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Thoughts.Core.ConfigModels;
using Thoughts.Core.Services;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Tests
{
	[TestFixture]
	public class TokenServiceTests
	{
		private TokenService _tokenService;

		[SetUp]
		public void Setup()
		{
			var appSettings = new AppSettings
			{
				AuthTokenKey = "your_auth_token_key_here"
			};
			var appSettingsOptions = Options.Create(appSettings);
			_tokenService = new TokenService(appSettingsOptions);
		}

		[Test]
		public void CreateToken_Returns_TokenString()
		{
			// Arrange
			var user = new User
			{
				Id = "user_id",
				UserName = "user_name",
				Email = "user@example.com"
			};

			// Act
			var token = _tokenService.CreateToken(user);

			// Assert
			Assert.IsNotNull(token);
			Assert.IsNotEmpty(token);
		}

		[Test]
		public void GetExpirationDate_Returns_DateTime()
		{
			// Act
			var expirationDate = _tokenService.GetExpirationDate();

			// Assert
			Assert.IsNotNull(expirationDate);
			Assert.Greater(expirationDate, DateTime.UtcNow);
		}

		[Test]
		public void DecodeToken_Returns_DecodedTokenString()
		{
			// Arrange
			var token = "your_encoded_token_here";

			// Act
			var decodedToken = _tokenService.DecodeToken(token);

			// Assert
			Assert.IsNotNull(decodedToken);
			Assert.IsNotEmpty(decodedToken);
		}

		[Test]
		public void EncodeToken_Returns_EncodedTokenString()
		{
			// Arrange
			var token = "your_token_here";

			// Act
			var encodedToken = _tokenService.EncodeToken(token);

			// Assert
			Assert.IsNotNull(encodedToken);
			Assert.IsNotEmpty(encodedToken);
		}

		[Test]
		public void ValidateAndDecodeToken_Throws_ValidationException_If_Token_Is_Null()
		{
			// Arrange
			string token = null;

			// Assert
			Assert.Throws<ValidationException>(() => _tokenService.ValidateAndDecodeToken(token));
		}

		[Test]
		public void ValidateAndDecodeToken_Returns_DecodedTokenString()
		{
			// Arrange
			var token = "your_encoded_token_here";

			// Act
			var decodedToken = _tokenService.ValidateAndDecodeToken(token);

			// Assert
			Assert.IsNotNull(decodedToken);
			Assert.IsNotEmpty(decodedToken);
		}
	}
}
