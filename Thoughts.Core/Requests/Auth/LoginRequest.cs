using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Auth
{
    public class LoginRequest
    {
		[Required]
		[EmailAddress]
		[StringLength(50)]
		public string Email { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 7)]
		public string Password { get; set; }
    }
}
