using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Mappings;
using Thoughts.Core.Requests.Thought;
using Thoughts.Core.Responses.Thought;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Services
{
	public class ThoughtService : IThoughtService
	{
		private readonly IThoughtRepository _thoughtRepository;
		private readonly IMapper _mapper;
		private readonly IValidationService _validationService;
		private readonly IHttpContextAccessor _context;

		public ThoughtService(IThoughtRepository thoughtRepository, IMapper mapper, IValidationService validationService, IHttpContextAccessor context)
		{
			_thoughtRepository = thoughtRepository;
			_mapper = mapper;
			_validationService = validationService;
			_context = context;
		}



		public async Task CreateThought(CreateThoughtRequest request)
		{
			_validationService.ValidateThoughtsHashtags(request.FirstHashtag, request.SecondHashtag);

			var thought = _mapper.Map<Thought?>(request);
			
			await _thoughtRepository.Add(thought);
		}

		public async Task<GetAllThoughtsResponse> GetThoughts(int page, int pageSize)
		{
			var thoughts = await _thoughtRepository.GetAll(page, pageSize);
			
			var mappedThoughts = await GetThoughtsResponse(thoughts);
			mappedThoughts.TotalThoughtCount = await _thoughtRepository.GetThoughtCount();

			return mappedThoughts;
		}

		public async Task<ThoughtResponse> GetThoughtById(GetThoughtRequest request)
		{
			var thought = await _validationService.RetrieveAndValidateThought(request.ThoughtId);
			
			var mappedThought = _mapper.Map<ThoughtResponse>(thought);

			var userId = GetCurrentUserId();
			mappedThought.IsLiked = thought.Likes?.Any(x => x.UserId == userId) ?? false;

			return mappedThought;
		}

		public async Task UpdateThought(UpdateThoughtRequest request)
		{
			var existingThought = await _validationService.RetrieveAndValidateThoughtBeforeChanges(request.ThoughtId, request.UserId);

			var thought = _mapper.Map(request, existingThought);
			
			await _thoughtRepository.Update(thought);
		}

		public async Task DeleteThought(DeleteThoughtRequest request)
		{
			var existingThought = await _validationService.RetrieveAndValidateThoughtBeforeChanges(request.ThoughtId, request?.UserId);

			await _thoughtRepository.Delete(existingThought);
		}

		public async Task<GetAllThoughtsResponse> GetThoughtsByUsername(ThoughtsByUsernameRequest request)
		{
			var thoughts = await _thoughtRepository.GetUserThoughts(request.Username, request.Page, request.PageSize);
			
			var mappedThoughts = await GetThoughtsResponse(thoughts);
			
			mappedThoughts.TotalThoughtCount = await _thoughtRepository.GetThoughtCount(x => x.User.UserName == request.Username);

			return mappedThoughts;

		}

		public async Task<GetAllThoughtsResponse> GetThoughtsBySearch(FilteredThoughtsRequest request)
		{
			var thoughts = await _thoughtRepository.GetThoughtsBySearch(request.SearchTerm, request.Page, request.PageSize);
			
			var mappedThoughts= await GetThoughtsResponse(thoughts);
			
			mappedThoughts.TotalThoughtCount = await _thoughtRepository.GetThoughtCount(t => t.ThoughtMessage.ToLower().Contains(request.SearchTerm.ToLower()) ||
																		  t.FirstHashtag.ToLower().Contains(request.SearchTerm.ToLower()) ||
																		  t.SecondHashtag.ToLower().Contains(request.SearchTerm.ToLower()));
			return mappedThoughts;

		}

		public async Task<GetAllThoughtsResponse> GetTop9ThoughtsOfAllTime()
		{
			var thoughts = await _thoughtRepository.GetTop9ThoughtsOfAllTime();
			
			return await GetThoughtsResponse(thoughts);
		}

		public async Task<GetAllThoughtsResponse> GetTop9ThoughtsOfLast30Days()
		{
			var thoughts = await _thoughtRepository.GetTop9ThoughtsOfLast30Days();
			
			return await GetThoughtsResponse(thoughts);
		}

		public async Task<GetAllThoughtsResponse> GetTop9ThoughtsOfLastWeek()
		{
			var thoughts = await _thoughtRepository.GetTop9ThoughtsOfLastWeek();
			
			return await GetThoughtsResponse(thoughts);
		}

		private async Task<GetAllThoughtsResponse> GetThoughtsResponse(IEnumerable<Thought> thoughts)
		{
			if (!thoughts.Any())
				return new GetAllThoughtsResponse();

			var currentUserId = GetCurrentUserId();

			var mappedThoughts = await Task.Run(() => thoughts.Select(x =>
			{
				var dto = _mapper.Map<ThoughtDto>(x);
				dto.IsLiked = x.Likes?.Any(y => y.UserId == currentUserId) ?? false;
				return dto;
			}));
			var result = new GetAllThoughtsResponse(mappedThoughts);
			return result;
		}

		private string GetCurrentUserId()
		{
			var userId = _context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return userId;
		}


	}
}
