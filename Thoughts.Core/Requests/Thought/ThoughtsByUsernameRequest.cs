
namespace Thoughts.Core.Requests.Thought
{
	public class ThoughtsByUsernameRequest
	{
		public string Username { get; set; }

		public int Page { get; set; }

		public int PageSize { get; set; }
	}
}
