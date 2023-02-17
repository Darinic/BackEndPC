using Logistics.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Services;
using Thoughts.Domain.Entities;
using static NUnit.Framework.Constraints.Tolerance;

namespace Thoughts.Tests.Services
{
	[TestFixture]
	public class LikeServiceTests
	{
		private ILikeService likeServiceMock;
		private Mock<IThoughtRepository> thoughtRepositoryMock;
		private Mock<ILikesRepository> likeRepositoryMock;
		private Mock<IValidationService> validationServiceMock;
		private Guid _thoughtId;
		private string _userId;

		[SetUp]
		public void Setup()
		{
			thoughtRepositoryMock = new Mock<IThoughtRepository>();
			likeRepositoryMock = new Mock<ILikesRepository>();
			validationServiceMock = new Mock<IValidationService>();
			likeServiceMock = new LikeService(thoughtRepositoryMock.Object, likeRepositoryMock.Object, validationServiceMock.Object);
			_thoughtId = Guid.NewGuid();
			_userId = "userIdTest";
		}

		[Test]
		public async Task LikeThought_WhenLikeDoesNotExist_ShouldAddLike()
		{
			//Arrange
			var thoughtId = Guid.NewGuid();
			var userId = "testUserId";

			thoughtRepositoryMock.Setup(repo => repo.GetById(_thoughtId)).ReturnsAsync(new Thought { Id = _thoughtId });
			likeRepositoryMock.Setup(repo => repo.Get(_thoughtId, _userId)).ReturnsAsync((Like)null);
			validationServiceMock.Setup(service => service.RetrieveAndValidateThought(thoughtId)).ReturnsAsync(new Thought { Id = thoughtId });
			validationServiceMock.Setup(service => service.RetrieveAndValidateLike(thoughtId, userId, true)).ReturnsAsync((Like)null);

			var likeService = new LikeService(thoughtRepositoryMock.Object, likeRepositoryMock.Object, validationServiceMock.Object);

			//Act
			await likeService.LikeThought(thoughtId, userId);

			//Assert
			likeRepositoryMock.Verify(repo => repo.Add(It.IsAny<Like>()), Times.Once);
			thoughtRepositoryMock.Verify(repo => repo.Update(It.IsAny<Thought>()), Times.Once);
		}
	

	[Test]
		public void LikeThought_WhenThoughtDoesNotExist_ShouldThrowValidationException()
		{
			// Arrange
			validationServiceMock.Setup(v => v.RetrieveAndValidateThought(_thoughtId)).ThrowsAsync(new ValidationException("Thought not found"));

			// Act + Assert
			Assert.ThrowsAsync<ValidationException>(() => likeServiceMock.LikeThought(_thoughtId, _userId));
		}

		[Test]
		public async Task LikeThought_WhenLikeExists_ShouldThrowValidationException()
		{
			// Arrange
			var thought = new Thought { Id = _thoughtId };
			var like = new Like { ThoughtId = _thoughtId, UserId = _userId };
			validationServiceMock.Setup(v => v.RetrieveAndValidateThought(_thoughtId)).ReturnsAsync(thought);
			validationServiceMock.Setup(v => v.RetrieveAndValidateLike(_thoughtId, _userId, true)).ReturnsAsync(like);
			likeRepositoryMock.Setup(r => r.Add(It.IsAny<Like>())).ThrowsAsync(new ValidationException("You have already liked the thought"));

			// Act + Assert
			var ex = Assert.ThrowsAsync<ValidationException>(() => likeServiceMock.LikeThought(_thoughtId, _userId));
			Assert.NotNull(ex);
		}

		[Test]
		public void DeleteLike_WhenLikeDoesNotExist_ShouldThrowValidationException()
		{
			// Arrange
			validationServiceMock.Setup(v => v.RetrieveAndValidateLike(_thoughtId, _userId, false)).ThrowsAsync(new ValidationException("You have already liked the thought"));

			// Act + Assert
			Assert.ThrowsAsync<ValidationException>(() => likeServiceMock.DeleteLike(_thoughtId, _userId));
		}

		[Test]
		public async Task DeleteLike_WhenLikeExists_ShouldRemoveLikeAndUpdateThought()
		{
			// Arrange
			validationServiceMock.Setup(v => v.RetrieveAndValidateLike(_thoughtId, _userId, false)).ReturnsAsync(new Like());
			validationServiceMock.Setup(v => v.RetrieveAndValidateThought(_thoughtId)).ReturnsAsync(new Thought());

			// Act
			await likeServiceMock.DeleteLike(_thoughtId, _userId);

			// Assert
			likeRepositoryMock.Verify(r => r.Remove(_thoughtId, _userId), Times.Once);
			thoughtRepositoryMock.Verify(r => r.Update(It.IsAny<Thought>()), Times.Once);
		}
	}
}
