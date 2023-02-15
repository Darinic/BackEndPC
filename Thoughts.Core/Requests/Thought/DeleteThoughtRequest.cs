
namespace Thoughts.Core.Requests.Thought
{
	public class DeleteThoughtRequest
	{
		public string? UserId { get; set; }
		public Guid ThoughtId { get; set; }
	}
}
