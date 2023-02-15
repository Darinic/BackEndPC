
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Interfaces
{
	public interface IThoughtCommentRepository
	{
		Task <IEnumerable<ThoughtComment>> GetAll(Guid thoughtId);

		Task<ThoughtComment> GetById(Guid id);
		
		Task Add(ThoughtComment thoughtComment);

		Task Delete(Guid CommentId);

		Task<ThoughtComment> RetrieveLastComment(string userId);
	}
}
