using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Thought.API.Controllers;
using Thoughts.Core.Interfaces;

namespace Thoughts.API.Controllers
{
	public class LikeController : BaseController
	{
		private readonly ILikeService _likeService;
		private readonly IValidationService _validationService;

		public LikeController(ILikeService likeService, IValidationService validationService)
		{
			_likeService = likeService;
			_validationService = validationService;
		}

		[HttpPost("{thoughtId}")]
		public async Task<IActionResult> LikeThought([FromRoute][Required] Guid thoughtId)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);

			await _likeService.LikeThought(thoughtId, userId);

			return Ok();
		}

		[HttpDelete("{thoughtId}")]
		public async Task<IActionResult> UnlikeThought([FromRoute][Required] Guid thoughtId)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);

			await _likeService.DeleteLike(thoughtId, userId);

			return Ok();
		}
	}
}
