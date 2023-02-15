using Logistics.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thoughts.Core.Interfaces;
using Thoughts.Domain.Entities;
using Thoughts.Domain.Exceptions;
using ArgumentException = Thoughts.Domain.Exceptions.ArgumentException;

namespace Thoughts.Core.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IThoughtRepository _thoughtRepository;
		private readonly ILikesRepository _likeRepository;
		private readonly UserManager<User> _userManager;
		private readonly IThoughtCommentRepository _thoughtCommentRepository;

		public ValidationService(IThoughtRepository thoughtRepository, ILikesRepository likeRepository, UserManager<User> userManager, IThoughtCommentRepository thoughtCommentRepository)
		{
			_thoughtRepository = thoughtRepository;
			_likeRepository = likeRepository;
			_userManager = userManager;
			_thoughtCommentRepository = thoughtCommentRepository;
		}
		public string GetUserIdFromClaims(IEnumerable<Claim> claims)
		{
			var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			ValidateUserId(userId);
			return userId;
		}
		
		public async Task<Like> RetrieveAndValidateLike(Guid thoughtId, string userId, bool isAddRequest)
		{
			var like = await _likeRepository.Get(thoughtId, userId);

			if(isAddRequest)
			{
				ValidateLikeForAdding(like);
			}
			else
			{
				ValidateLikeForDeleting(like);
			}

			return like;
		}

		private void ValidateLikeForAdding(Like like)
		{
			if (like != null)
				throw new ValidationException("You have already liked the thought");
		}

		private void ValidateLikeForDeleting(Like like)
		{
			if (like == null)
				throw new ValidationException("Thought not liked");
		}

		public async Task<Thought> RetrieveAndValidateThought(Guid thoughtId)
		{
			var thought = await _thoughtRepository.GetById(thoughtId);
			if (thought == null)
				throw new ConflictException("Thought not found");
			return thought;
		}

		public async Task<Thought> RetrieveAndValidateThoughtBeforeChanges(Guid thoughtId, string userId)
		{
			var thought = await RetrieveAndValidateThought(thoughtId);
			if (thought.UserId != userId)
				throw new UnauthorizedException("You are not allowed to interact with this thought");

			return thought;
		}

		public async Task ValidateUserCommentBeforeDeleting (Guid commentId, string userId)
		{
			var comment = await _thoughtCommentRepository.GetById(commentId);

			if (comment == null)
				throw new ValidationException("Comment not found");

			if (comment.UserId != userId)
				throw new UnauthorizedException("You are not allowed to delete this comment");
		}

		public async Task<User> ValidateUserByEmail(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
				throw new ValidationException("User does not exist");

			return user;
		}

		public void ValidateThoughtsHashtags(string firstHashtag, string secondHashtag)
		{
			if (firstHashtag.ToLower() == secondHashtag.ToLower())
				throw new ValidationException("Hashtags cannot be the same");
		}

		public async Task ValidateUserPassword(User user, string password)
		{
			var result = await _userManager.CheckPasswordAsync(user, password);
			if (!result)
				throw new ValidationException("Password is incorrect");
		}

		public async Task ValidateUserRegistration(string email, string username)
		{
			await ValidateIfEmailExists(username);
			await ValidateIfUsernameExists(email);
		}

		public async Task<User> ValidateUserByUserId(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
				throw new ValidationException("User does not exist");

			return user;
		}

		private async Task ValidateIfUsernameExists(string username)
		{
			var existingUsername = await _userManager.FindByNameAsync(username);
			if (existingUsername != null)
				throw new ValidationException("Username already in use");
		}

		private async Task ValidateIfEmailExists(string email)
		{
			var existingEmail = await _userManager.FindByEmailAsync(email);

			if (existingEmail != null)
				throw new ValidationException("Email already in use");
		}

		private void ValidateUserId(string userId)
		{
			if (userId == null)
				throw new UnauthorizedException("You must login first");
		}
	}
}
