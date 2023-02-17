using AutoMapper;
using Logistics.Domain.Exceptions;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Requests.ThoughtComment;
using Thoughts.Core.Responses.ThoughtComment;
using Thoughts.Core.Services;
using Thoughts.Domain.Entities;

namespace Thoughts.Tests.Services
{
	[TestFixture]
	public class ThoughtCommentServiceTests
	{
		private Mock<IThoughtCommentRepository> _thoughtCommentRepository;
		private Mock<IMapper> _mapper;
		private Mock<IValidationService> _validationService;
		private Guid _ThoughtId;
		private string _UserId;
		private string _CommentMessage;

		private ThoughtCommentService _thoughtCommentService;

		[SetUp]
		public void SetUp()
		{
			_thoughtCommentRepository = new Mock<IThoughtCommentRepository>();
			_mapper = new Mock<IMapper>();
			_validationService = new Mock<IValidationService>();
			_thoughtCommentService = new ThoughtCommentService(_thoughtCommentRepository.Object, _mapper.Object, _validationService.Object);
			_ThoughtId = Guid.NewGuid();
			_UserId = "testUserId";
			_CommentMessage = "This is a test comment";
		}

		[Test]
		public async Task AddComment_WhenValidationSucceeds_ShouldAddComment()
		{
			// Arrange
			var request = new AddCommentRequest
			{
				ThoughtId = _ThoughtId,
				UserId = _UserId,
				CommentMessage = _CommentMessage
			};

			_validationService.Setup(vs => vs.RetrieveAndValidateThought(request.ThoughtId)).Returns(Task.FromResult<Thought>(null));
			_validationService.Setup(vs => vs.ValidateUserCommentBeforeDeleting(It.IsAny<Guid>(), request.UserId)).Returns(Task.CompletedTask);

			var comment = new ThoughtComment();
			_mapper.Setup(m => m.Map<ThoughtComment>(request)).Returns(comment);

			// Act
			await _thoughtCommentService.AddComment(request);

			// Assert
			_thoughtCommentRepository.Verify(tc => tc.Add(comment), Times.Once);
		}

		[Test]
		public void AddComment_WhenUserPostsAgainBefore5Minutes_ShouldThrowConflictException()
		{
			// Arrange
			var request = new AddCommentRequest
			{
				ThoughtId = _ThoughtId,
				UserId = _UserId,
				CommentMessage = _CommentMessage
			};

			_validationService.Setup(vs => vs.RetrieveAndValidateThought(request.ThoughtId)).Returns(Task.FromResult<Thought>(null));


			var lastComment = new ThoughtComment
			{
				CreationDate = DateTime.Now.AddMinutes(-1)
			};
			_thoughtCommentRepository.Setup(r => r.RetrieveLastComment(request.UserId)).ReturnsAsync(lastComment);

			// Act & Assert
			Assert.ThrowsAsync<ConflictException>(() => _thoughtCommentService.AddComment(request));
		}

		[Test]
		public async Task AddComment_WhenUserPostsAgainAfter5Minutes_ShouldAddComment()
		{
			// Arrange
			var request = new AddCommentRequest
			{
				ThoughtId = _ThoughtId,
				UserId = _UserId,
				CommentMessage = _CommentMessage
			};

			_validationService.Setup(vs => vs.RetrieveAndValidateThought(request.ThoughtId)).Returns(Task.FromResult<Thought>(null));

			var lastComment = new ThoughtComment
			{
				CreationDate = DateTime.Now.AddMinutes(-5)
			};
			_thoughtCommentRepository.Setup(r => r.RetrieveLastComment(request.UserId)).ReturnsAsync(lastComment);

			var comment = new ThoughtComment();
			_mapper.Setup(m => m.Map<ThoughtComment>(request)).Returns(comment);

			// Act
			await _thoughtCommentService.AddComment(request);

			// Assert
			_thoughtCommentRepository.Verify(tc => tc.Add(comment), Times.Once);
		}

		[Test]
		public async Task DeleteComment_WhenValidationSucceeds_ShouldDeleteComment()
		{
			// Arrange
			var request = new DeleteCommentRequest
			{
				CommentId = Guid.NewGuid(),
				UserId = _UserId
			};

			_validationService.Setup(vs => vs.ValidateUserCommentBeforeDeleting(request.CommentId, request.UserId)).Returns(Task.CompletedTask);

			// Act
			await _thoughtCommentService.DeleteComment(request);

			// Assert
			_thoughtCommentRepository.Verify(tc => tc.Delete(request.CommentId), Times.Once);
		}

		[Test]
		public async Task GetAll_WhenThoughtDoesNotExist_ShouldReturnEmptyList()
		{
			// Arrange
			var thoughtId = _ThoughtId;
			_thoughtCommentRepository.Setup(tc => tc.GetAll(thoughtId)).ReturnsAsync(new List<ThoughtComment>());

			// Act
			var actualResponse = await _thoughtCommentService.GetAll(thoughtId);

			// Assert
			Assert.That(actualResponse, Is.Empty);
		}
	}
}
