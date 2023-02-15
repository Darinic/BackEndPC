

namespace Thoughts.Core.Interfaces
{
    public interface ILikeService
    {
        Task LikeThought(Guid thoughtId, string userId);
		Task DeleteLike(Guid thoughtId, string userId);
    }
}
