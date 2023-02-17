using Moq;
using NUnit.Framework;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Services;
using Thoughts.Domain.Entities;
using Thoughts.Domain.Exceptions;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Logistics.Domain.Exceptions;

namespace Thoughts.Core.Tests.Services
{
	[TestFixture]
	public class ValidationServiceTests
	{
		private Mock<IThoughtRepository> thoughtRepositoryMock;
		private Mock<ILikesRepository> likeRepositoryMock;
		private Mock<UserManager<User>> userManagerMock;
		private Mock<IThoughtCommentRepository> thoughtCommentRepositoryMock;
		private IValidationService validationService;

		[SetUp]
		public void Setup()
		{
			thoughtRepositoryMock = new Mock<IThoughtRepository>();
			likeRepositoryMock = new Mock<ILikesRepository>();
			userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
			thoughtCommentRepositoryMock = new Mock<IThoughtCommentRepository>();
			validationService = new ValidationService(thoughtRepositoryMock.Object, likeRepositoryMock.Object, userManagerMock.Object, thoughtCommentRepositoryMock.Object);
		}

		[Test]
		public void GetUserIdFromClaims_WithValidClaims_ReturnsUserId()
		{
			// Arrange
			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, "1")
		};

			// Act
			var result = validationService.GetUserIdFromClaims(claims);

			// Assert
			Assert.That(result, Is.EqualTo("1"));
		}

		[Test]
		public void GetUserIdFromClaims_WithInvalidClaims_ThrowsArgumentException()
		{
			// Arrange
			var claims = new List<Claim>();

			// Act
			var ex = Assert.Throws<UnauthorizedException>(() => validationService.GetUserIdFromClaims(claims));

			// Assert
			Assert.That(ex.Message, Is.EqualTo("You must login first"));
		}

		[Test]
		public async Task ValidateUserPassword_ValidPassword_ReturnsSuccessfully()
		{
			// Arrange
			var user = new User();
			var password = "validpassword";
			userManagerMock.Setup(u => u.CheckPasswordAsync(user, password)).ReturnsAsync(true);

			// Act
			await validationService.ValidateUserPassword(user, password);

			// Assert
			userManagerMock.Verify(u => u.CheckPasswordAsync(user, password), Times.Once);
		}

		[Test]
		public async Task ValidateUserPassword_InvalidPassword_ThrowsValidationException()
		{
			// Arrange
			var user = new User();
			var password = "invalidpassword";
			userManagerMock.Setup(u => u.CheckPasswordAsync(user, password)).ReturnsAsync(false);

			// Act and Assert
			var ex = Assert.ThrowsAsync<ValidationException>(async () => await validationService.ValidateUserPassword(user, password));
			Assert.That(ex.Message, Is.EqualTo("Password is incorrect"));

			userManagerMock.Verify(u => u.CheckPasswordAsync(user, password), Times.Once);
		}

		[Test]
		public async Task RetrieveAndValidateLike_WithValidLikeForDeleting_ReturnsLike()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();
			var userId = "testUserId";
			var like = new Like { ThoughtId = thoughtId, UserId = userId };
			likeRepositoryMock.Setup(r => r.Get(thoughtId, userId)).ReturnsAsync(like);

			// Act
			var result = await validationService.RetrieveAndValidateLike(thoughtId, userId, isAddRequest: false);

			// Assert
			Assert.That(result, Is.EqualTo(like));
			likeRepositoryMock.Verify(r => r.Get(thoughtId, userId), Times.Once);
		}

		[Test]
		public async Task RetrieveAndValidateLike_WithInvalidLikeForAdding_ThrowsValidationException()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();
			var userId = "testUserId";
			var like = new Like { ThoughtId = thoughtId, UserId = userId };
			likeRepositoryMock.Setup(r => r.Get(thoughtId, userId)).ReturnsAsync(like);

			// Act
			var exception = Assert.ThrowsAsync<ValidationException>(() => validationService.RetrieveAndValidateLike(thoughtId, userId, isAddRequest: true));

			// Assert
			Assert.That(exception.Message, Is.EqualTo("You have already liked the thought"));
			likeRepositoryMock.Verify(r => r.Get(thoughtId, userId), Times.Once);
		}

		[Test]
		public async Task ValidateUserByEmail_ValidEmail_ReturnsUser()
		{
			// Arrange
			var email = "user@example.com";
			var user = new User { Email = email };
			userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);

			// Act
			var result = await validationService.ValidateUserByEmail(email);

			// Assert
			Assert.That(result, Is.EqualTo(user));
			userManagerMock.Verify(m => m.FindByEmailAsync(email), Times.Once);
		}

        [Test]
        public void ValidateUserByEmail_InvalidEmail_ThrowsValidationException()
        {
            // Arrange
            var email = "user@example.com";
            userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync((User)null);

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => validationService.ValidateUserByEmail(email));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("User does not exist"));
            userManagerMock.Verify(m => m.FindByEmailAsync(email), Times.Once);
        }

		[Test]
		public async Task RetrieveAndValidateThought_ThoughtExists_ReturnsThought()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();
			var expectedThought = new Thought { Id = thoughtId};

			var thoughtRepositoryMock = new Mock<IThoughtRepository>();
			thoughtRepositoryMock.Setup(r => r.GetById(thoughtId)).ReturnsAsync(expectedThought);

			var validationService = new ValidationService(thoughtRepositoryMock.Object, null, null, null);

			// Act
			var result = await validationService.RetrieveAndValidateThought(thoughtId);

			// Assert
			Assert.That(result, Is.EqualTo(expectedThought));
		}

		[Test]
		public void RetrieveAndValidateThought_ThoughtDoesNotExist_ThrowsConflictException()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();

			var thoughtRepositoryMock = new Mock<IThoughtRepository>();
			thoughtRepositoryMock.Setup(r => r.GetById(thoughtId)).ReturnsAsync((Thought)null);

			var validationService = new ValidationService(thoughtRepositoryMock.Object, null, null, null);

			// Act and Assert
			Assert.ThrowsAsync<ConflictException>(async () => await validationService.RetrieveAndValidateThought(thoughtId));
		}

		[Test]
		public async Task ValidateUserByUserId_UserExists_ReturnsUser()
		{
			//Arrange
			var userId = "TestUser1";
			var expectedUser = new User { Id = userId };

			userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(expectedUser);

			//Act
			var result = await validationService.ValidateUserByUserId(userId);

			//Assert
			Assert.That(result, Is.EqualTo(expectedUser));
		}
		
		[Test]
		public void ValidateUserByUserId_UserDoesNotExist_ThrowsValidationException()
		{
			// Arrange
			var userId = "user123";

			userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((User)null);

			// Act and Assert
			var ex = Assert.ThrowsAsync<ValidationException>(async () => await validationService.ValidateUserByUserId(userId));
			Assert.That(ex.Message, Is.EqualTo("User does not exist"));
		}

		[Test]
		public async Task ValidateUserCommentBeforeDeleting_CommentExistsAndIsUser_ReturnsSuccessfully()
		{
			// Arrange
			var commentId = Guid.NewGuid();
			var userId = "user123";
			var comment = new ThoughtComment { Id = commentId, UserId = userId };
			thoughtCommentRepositoryMock.Setup(r => r.GetById(commentId)).ReturnsAsync(comment);

			// Act
			await validationService.ValidateUserCommentBeforeDeleting(commentId, userId);

			// Assert
			thoughtCommentRepositoryMock.Verify(r => r.GetById(commentId), Times.Once);
		}

		[Test]
		public void ValidateUserCommentBeforeDeleting_CommentDoesNotExist_ThrowsValidationException()
		{
			// Arrange
			var commentId = Guid.NewGuid();
			var userId = "user123";
			ThoughtComment comment = null;
			thoughtCommentRepositoryMock.Setup(r => r.GetById(commentId)).ReturnsAsync(comment);

			// Act and Assert
			var ex = Assert.ThrowsAsync<ValidationException>(async () => await validationService.ValidateUserCommentBeforeDeleting(commentId, userId));
			Assert.That(ex.Message, Is.EqualTo("Comment not found"));

			thoughtCommentRepositoryMock.Verify(r => r.GetById(commentId), Times.Once);
		}

		[Test]
		public void ValidateThoughtsHashtags_HashtagsAreDifferent_ReturnsSuccessfully()
		{
			// Arrange
			var firstHashtag = "tag1";
			var secondHashtag = "tag2";

			// Act
			validationService.ValidateThoughtsHashtags(firstHashtag, secondHashtag);

			// Assert
			// No exception is thrown
		}

		[Test]
		public void ValidateThoughtsHashtags_HashtagsAreTheSame_ThrowsValidationException()
		{
			// Arrange
			var firstHashtag = "tag";
			var secondHashtag = "tag";

			// Act and Assert
			var ex = Assert.Throws<ValidationException>(() => validationService.ValidateThoughtsHashtags(firstHashtag, secondHashtag));
			Assert.That(ex.Message, Is.EqualTo("Hashtags cannot be the same"));
		}

		[Test]
		public async Task RetrieveAndValidateThoughtBeforeChanges_ThoughtExistsAndIsUser_ReturnsThought()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();
			var userId = "user123";
			var thought = new Thought { Id = thoughtId, UserId = userId };
			thoughtRepositoryMock.Setup(r => r.GetById(thoughtId)).ReturnsAsync(thought);

			// Act
			var result = await validationService.RetrieveAndValidateThoughtBeforeChanges(thoughtId, userId);

			// Assert
			Assert.That(result, Is.EqualTo(thought));
			thoughtRepositoryMock.Verify(r => r.GetById(thoughtId), Times.Once);
		}

		[Test]
		public void RetrieveAndValidateThoughtBeforeChanges_ThoughtExistsButIsNotUser_ThrowsUnauthorizedException()
		{
			// Arrange
			var thoughtId = Guid.NewGuid();
			var userId = "user123";
			var thought = new Thought { Id = thoughtId, UserId = "otherUser" };
			thoughtRepositoryMock.Setup(r => r.GetById(thoughtId)).ReturnsAsync(thought);

			// Act and Assert
			var ex = Assert.ThrowsAsync<UnauthorizedException>(async () => await validationService.RetrieveAndValidateThoughtBeforeChanges(thoughtId, userId));
			Assert.That(ex.Message, Is.EqualTo("You are not allowed to interact with this thought"));

			thoughtRepositoryMock.Verify(r => r.GetById(thoughtId), Times.Once);
		}
	}
}
