using Thoughts.Core.Interfaces;
using Thoughts.Core.ConfigModels;
using Microsoft.Extensions.Options;
using Thoughts.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Security;

namespace Thoughts.Core.Services
{
	public class MailService : IMailService
    {
        private readonly AppSettings _appSettings;
		private readonly UserManager<User> _userManager;
		private readonly TokenService _tokenService;

		public MailService(IOptions<AppSettings> appSettings, UserManager<User> userManager, TokenService tokenService)
		{
			_appSettings = appSettings.Value;
			_userManager = userManager;
			_tokenService = tokenService;
		}
		
		private void SendEmail(string toEmail, string subject, string content)
		{
			var message = new MimeMessage();
			message.From.Add(MailboxAddress.Parse(_appSettings.EmailUsername));
			message.To.Add(MailboxAddress.Parse(toEmail));
			message.Subject = subject;
			message.Body = new TextPart("html") { Text = content };

			using var smtp = new MailKit.Net.Smtp.SmtpClient();
			smtp.Connect(_appSettings.EmailServer, _appSettings.EmailServerPort, SecureSocketOptions.StartTls);
			smtp.Authenticate(_appSettings.MailJetApiKey, _appSettings.MailJetSecretKey);

			smtp.Send(message);
			smtp.Disconnect(true);
			
		}

		public async Task SendEmailConfirmation(string email, User userEntity)
        {
			var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);

			var validToken = _tokenService.EncodeToken(confirmEmailToken);

			string callbackUrl = $"{_appSettings.BackEndUrl}/api/auth/confirmemail?userid={userEntity.Id}&token={validToken}";

			SendEmail(email, "Confirm your email", $"<h1>Welcome to Rain Of Thoughts</h1> <p>Please confirm your email by <a href='{callbackUrl}'>clicking here</a></p>");
		}
		
		public async Task SendEmailForgotPassword(string email, User user)
		{
			var generatedToken = await _userManager.GeneratePasswordResetTokenAsync(user);
				
			var validToken = _tokenService.EncodeToken(generatedToken);

			string callbackUrl = $"{_appSettings.FrontEndUrl}/resetpassword/{email}/{validToken}";

			SendEmail(email, "Reset password", $"<h1>Reset password</h1> + <p>Please reset your password by <a href='{callbackUrl}'>clicking here</a></p>");
			
			
		}
    }
}
