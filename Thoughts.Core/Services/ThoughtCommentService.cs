using AutoMapper;
using Logistics.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.ThoughtComment;
using Thoughts.Core.Responses.ThoughtComment;
using Thoughts.Domain.Entities;
using Thoughts.Domain.Exceptions;
using ValidationException = Logistics.Domain.Exceptions.ValidationException;

namespace Thoughts.Core.Services
{
	public class ThoughtCommentService : IThoughtCommentService
	{
		private readonly IThoughtCommentRepository _thoughtCommentRepository;
		private readonly IMapper _mapper;
		private readonly IValidationService _validationService;

		public ThoughtCommentService(IThoughtCommentRepository thoughtCommentRepository, IMapper mapper, IValidationService validationService)
		{
			_thoughtCommentRepository = thoughtCommentRepository;
			_mapper = mapper;
			_validationService = validationService;
		}

		public async Task AddComment(AddCommentRequest request)
		{
			await _validationService.RetrieveAndValidateThought(request.ThoughtId);
			await ValidateThatUserCanPost(request.UserId);
			var comment = _mapper.Map<ThoughtComment>(request);
			await _thoughtCommentRepository.Add(comment);
		}

		public async Task DeleteComment(DeleteCommentRequest request)
		{
			await _validationService.ValidateUserCommentBeforeDeleting(request.CommentId, request.UserId);
			await _thoughtCommentRepository.Delete(request.CommentId);
		}

		public async Task<IEnumerable<ThoughtCommentsResponse>> GetAll([Required] Guid thoughtId)
		{
			var comments = await _thoughtCommentRepository.GetAll(thoughtId);
			var mappedComments = _mapper.Map<IEnumerable<ThoughtCommentsResponse>>(comments);
			return mappedComments;
		}

		private async Task ValidateThatUserCanPost(string userId)
		{
			var lastComment = await _thoughtCommentRepository.RetrieveLastComment(userId);

			if(lastComment != null)
			{
				var timeDifference = (DateTime.Now - lastComment.CreationDate).TotalMinutes;
				if(timeDifference < 5)
				{
					throw new ConflictException("You cannot post another comment yet. Please wait for 5 minutes");
				}
			}
		}
	}
}
