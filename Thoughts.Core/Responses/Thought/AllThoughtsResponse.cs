

namespace Thoughts.Core.Responses.Thought
{
	public class GetAllThoughtsResponse
	{
		public IEnumerable<ThoughtDto>? Thoughts { get; set; }

		public int TotalThoughtCount { get; set; }

		public GetAllThoughtsResponse(IEnumerable<ThoughtDto> thoughts, int totalThoughtCount)
		{
			Thoughts = thoughts;
			TotalThoughtCount = totalThoughtCount;
		}

		public GetAllThoughtsResponse(IEnumerable<ThoughtDto> thoughts)
		{
			Thoughts = thoughts;
		}

		public GetAllThoughtsResponse()
		{
		}
	}
}
