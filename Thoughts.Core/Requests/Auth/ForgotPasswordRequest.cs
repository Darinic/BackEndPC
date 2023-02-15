using System;
using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Auth
{
	public class ForgotPasswordRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
