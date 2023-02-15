
namespace Thoughts.Core.Requests.Auth
{
	public class ConfirmEmailRequest
	{
		public string? UserId { get; set; }
		public string? Token { get; set; }
	}
}
