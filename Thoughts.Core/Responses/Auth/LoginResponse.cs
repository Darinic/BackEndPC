
namespace Thoughts.Core.Responses.Auth
{
	public class LoginResponse
	{
		public string? Token { get; set; }
		public DateTime? ExpireDate { get; set; }
		public string? Message { get; set; }
		public string? UserName { get; set; }

		public LoginResponse()
		{
		}
	}
}
