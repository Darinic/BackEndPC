using Thoughts.Core.Responses.ThoughtComment;

namespace Thoughts.Core.Responses.Thought
{
	public class ThoughtResponse : ThoughtDto
	{
		public IEnumerable<ThoughtCommentsResponse> Comments { get; set; }

		public DateTime CreationDate { get; set; }
	}
}
