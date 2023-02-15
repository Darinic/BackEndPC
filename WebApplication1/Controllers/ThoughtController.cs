using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thought.API.Controllers;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.Thought;
using Thoughts.Infrastructure.Data;
using Thoughts.Infrastructure.Migrations;

namespace Thoughts.API.Controllers
{
	public class ThoughtController : BaseController
	{
        private readonly IThoughtService _thoughtService;
		private readonly IValidationService _validationService;
		private readonly IThoughtCommentService _thoughtCommentService;
		private readonly ThoughtDataContext _context;

		public ThoughtController(IThoughtService thoughtService, IValidationService validationService, IThoughtCommentService thoughtCommentService, ThoughtDataContext context)
		{
            _thoughtService = thoughtService;
			_validationService = validationService;
			_thoughtCommentService = thoughtCommentService;
			_context = context;
		}

		
        [HttpPost]
		public async Task<IActionResult> CreateThought(CreateThoughtRequest request)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);
			request.UserId = userId;

			await _thoughtService.CreateThought(request);
			
			return Ok();
		}
		
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetThoughts([FromQuery] int page = 1, int pageSize = 9)
		{
			var result = await _thoughtService.GetThoughts(page, pageSize);
			
			return Ok(new {
				thoughts = result.Thoughts,
				totalCount = await _context.Thoughts.CountAsync()
			});
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetThought([FromRoute] Guid id)
		{	
			var result = await _thoughtService.GetThoughtById(new GetThoughtRequest { ThoughtId = id});

			return Ok(result);
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> UpdateThought([FromRoute] Guid id, [FromBody] UpdateThoughtRequest request)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);
			request.ThoughtId = id;
			request.UserId = userId;

			await _thoughtService.UpdateThought(request);

			return Ok();
		}
		
		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteThought([FromRoute] Guid id)
		{
			var userId = _validationService.GetUserIdFromClaims(User.Claims);
			await _thoughtService.DeleteThought(new DeleteThoughtRequest { ThoughtId = id, UserId = userId });

			return Ok();
		}

		[AllowAnonymous]
		[HttpGet("user/{username}")]
		public async Task<IActionResult> GetThoughtsByUser([FromRoute] string username, int page = 1, int pageSize = 9)
		{
			var result = await _thoughtService.GetThoughtsByUsername(new ThoughtsByUsernameRequest { Username = username, Page = page, PageSize = pageSize });

			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("search/{searchTerm}")]
		public async Task<IActionResult> GetFilteredThoughtsBySearch([FromRoute] string searchTerm, int page = 1, int pageSize = 9)
		{
			var result = await _thoughtService.GetThoughtsBySearch(new FilteredThoughtsRequest { SearchTerm = searchTerm, Page = page, PageSize = pageSize });

			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("top9alltime")]
		public async Task<IActionResult> GetTop9ThoughtsOfAllTime()
		{
			var result = await _thoughtService.GetTop9ThoughtsOfAllTime();

			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("top9last30days")]
		public async Task<IActionResult> GetTop9ThoughtsOfLast30Days()
		{
			var result = await _thoughtService.GetTop9ThoughtsOfLast30Days();

			return Ok(result);
		}


		[AllowAnonymous]
		[HttpGet("top9lastweek")]
		public async Task<IActionResult> GetTop9ThoughtsOfLastWeek()
		{
			var result = await _thoughtService.GetTop9ThoughtsOfLastWeek();

			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("{thoughtId}/Comments")]
		public async Task<IActionResult> GetThoughtComments([FromRoute] Guid thoughtId)
		{
			var result = await _thoughtCommentService.GetAll(thoughtId);

			return Ok(result);
		}
	}
}
