using Thoughts.Domain.Entities;

namespace Thoughts.Core.Interfaces
{
	public interface ILikesRepository
	{
		Task<Like> Add(Like like);
		Task<Like> Get(Guid thoughtId, string userId);
		Task<int> Remove(Guid thoughtId, string userId);
	}
}
