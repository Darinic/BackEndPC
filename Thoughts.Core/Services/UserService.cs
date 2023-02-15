using Logistics.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.Auth;
using Thoughts.Core.Responses.Auth;
using Thoughts.Domain.Entities;
using Thoughts.Domain.Exceptions;

namespace Thoughts.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;
        private readonly TokenService _tokenService;
		private readonly IValidationService _validationService;

		public UserService(UserManager<User> userManager, IMailService mailService, TokenService tokenService, IValidationService validationService)
        {
            _userManager = userManager;
			_mailService = mailService;
            _tokenService = tokenService;
			_validationService = validationService;
		}

        public async Task Register(RegisterRequest request)
        {       
			await _validationService.ValidateUserRegistration(request.Email, request.UserName);

			var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
			};

            var createdUser = await _userManager.CreateAsync(user, request.Password);

			if (createdUser.Succeeded)
			{
				await _mailService.SendEmailConfirmation(request.Email, user);
				return;
			}

			throw new ValidationException(createdUser.Errors.First().Description);
		}

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _validationService.ValidateUserByEmail(request.Email);

			await _validationService.ValidateUserPassword(user, request.Password);

			return new LoginResponse
            {
                Message = "Login Successfull",
                Token = _tokenService.CreateToken(user),
                ExpireDate = _tokenService.GetExpirationDate(),
                UserName = user.UserName,
			};
        }
		
        public async Task ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _validationService.ValidateUserByUserId(request.UserId);
			
			var decodedToken = _tokenService.ValidateAndDecodeToken(request.Token);

			var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
				throw new ServerSideException("Email could not be confirmed");
        }

        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
			var user = await _validationService.ValidateUserByEmail(request.Email);
			
            await _mailService.SendEmailForgotPassword(request.Email, user);
        }
		
		public async Task ResetPassword(ResetPasswordRequest request)
		{
			var user = await _validationService.ValidateUserByEmail(request.Email);

			var decodedToken = _tokenService.ValidateAndDecodeToken(request.Token);

			var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);

			if (!result.Succeeded)
				throw new ServerSideException("Password could not be reset");
		}
	}
}
