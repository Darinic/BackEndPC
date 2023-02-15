using Thoughts.Core.Interfaces;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Services
{
	public class LikeService : ILikeService
	{
		private readonly IThoughtRepository _thoughtRepository;
		private readonly ILikesRepository _likeRepository;
		private readonly IValidationService _validationService;

		public LikeService(IThoughtRepository thoughtRepository, ILikesRepository likeRepository, IValidationService validationService)
		{
			_thoughtRepository = thoughtRepository;
			_likeRepository = likeRepository;
			_validationService = validationService;
		}


		public async Task LikeThought(Guid thoughtId, string userId)
		{
			var thought = await _validationService.RetrieveAndValidateThought(thoughtId);

			await _validationService.RetrieveAndValidateLike(thoughtId, userId, true);

			var like = new Like
				{
					ThoughtId = thoughtId,
					UserId = userId
				};

			await _likeRepository.Add(like);
			await _thoughtRepository.Update(thought);

		}
			
		
		public async Task DeleteLike(Guid thoughtId, string userId)
		{
			await _validationService.RetrieveAndValidateLike(thoughtId, userId, false);

			var thought = await _validationService.RetrieveAndValidateThought(thoughtId);

			await _likeRepository.Remove(thoughtId, userId);
			await _thoughtRepository.Update(thought);
		}
	}
}
