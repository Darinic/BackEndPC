using System.Security.Claims;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Interfaces
{
	public interface IValidationService
	{
		string GetUserIdFromClaims(IEnumerable<Claim> claims);
		Task<Like> RetrieveAndValidateLike(Guid thoughtId, string userId, bool isAddRequest);
		Task<Thought> RetrieveAndValidateThought(Guid thoughtId);
		Task<User> ValidateUserByEmail(string email);
		Task ValidateUserRegistration(string email, string username);
		Task ValidateUserPassword(User user, string password);
		Task<User> ValidateUserByUserId(string userId);
		Task ValidateUserCommentBeforeDeleting(Guid commentId, string userId);
		Task<Thought> RetrieveAndValidateThoughtBeforeChanges(Guid thoughtId, string userId);
		void ValidateThoughtsHashtags(string firstHashtag, string secondHashtag);
	}
}