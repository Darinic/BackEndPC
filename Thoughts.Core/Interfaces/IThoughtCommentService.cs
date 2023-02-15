using Thoughts.Core.Requests.ThoughtComment;
using Thoughts.Core.Responses.ThoughtComment;

namespace Thoughts.Core.Interfaces
{
	public interface IThoughtCommentService
	{
		Task<IEnumerable<ThoughtCommentsResponse>> GetAll(Guid thoughtId);
		Task AddComment(AddCommentRequest request);
		Task DeleteComment(DeleteCommentRequest request);
	}
}
