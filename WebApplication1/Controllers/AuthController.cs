using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Thought.API.Controllers;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.Auth;

namespace Thoughts.API.Controllers
{
	public class AuthController : BaseController
	{
		private readonly IUserService _userService;

		public AuthController(IUserService userService)
		{
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost("Register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
		{
			await _userService.Register(request);

			return Ok();
		}
		
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
		{
			var result = await _userService.Login(request);

			return Ok(result);
		}
		
		[AllowAnonymous]
		[HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
			await _userService.ConfirmEmail(request);

			return Ok();
        }
		
		[AllowAnonymous]
		[HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
			await _userService.ForgotPassword(request);

			return Ok();
        }

		[AllowAnonymous]
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			await _userService.ResetPassword(request);

			return Ok();
		}
	}
}
