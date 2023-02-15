using Thoughts.Core.Requests.Thought;
using Thoughts.Core.Responses.Thought;

namespace Thoughts.Core.Interfaces
{
	public interface IThoughtService
    {
		Task CreateThought(CreateThoughtRequest request);
		Task<GetAllThoughtsResponse> GetThoughts(int page, int pageSize);
		Task<ThoughtResponse> GetThoughtById(GetThoughtRequest request);
		Task UpdateThought(UpdateThoughtRequest request);
		Task DeleteThought(DeleteThoughtRequest request);
		Task<GetAllThoughtsResponse> GetThoughtsByUsername(ThoughtsByUsernameRequest request);
		Task<GetAllThoughtsResponse> GetThoughtsBySearch(FilteredThoughtsRequest request);
		Task<GetAllThoughtsResponse> GetTop9ThoughtsOfAllTime();
		Task<GetAllThoughtsResponse> GetTop9ThoughtsOfLast30Days();
		Task<GetAllThoughtsResponse> GetTop9ThoughtsOfLastWeek();
	}
}
