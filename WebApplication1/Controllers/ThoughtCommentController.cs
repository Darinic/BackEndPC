using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Thought.API.Controllers;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.ThoughtComment;
using Thoughts.Domain.Exceptions;

namespace Thoughts.API.Controllers
{
	public class ThoughtCommentController : BaseController
	{
		private readonly IThoughtCommentService _thoughtCommentService;
		private readonly IValidationService _validationService;

		public ThoughtCommentController(IThoughtCommentService thoughtCommentService, IValidationService validationService)
		{
			_thoughtCommentService = thoughtCommentService;
			_validationService = validationService;
		}

		[HttpPost("{thoughtId}")]
		public async Task<IActionResult> CreateThoughtComment([FromRoute] Guid thoughtId, [FromBody] AddCommentRequest request)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);
			request.UserId = userId;
			request.ThoughtId = thoughtId;

			await _thoughtCommentService.AddComment(request);

			return Ok();
		}
		
		[HttpDelete("{commentId}")]
		public async Task<IActionResult> DeleteThoughtComment([FromRoute] Guid commentId)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);

			await _thoughtCommentService.DeleteComment(new DeleteCommentRequest { CommentId = commentId, UserId = userId });

			return Ok();
		}
	}
}
