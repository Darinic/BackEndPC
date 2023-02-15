using Microsoft.EntityFrameworkCore;
using Thoughts.Core.Interfaces;
using Thoughts.Domain.Entities;
using Thoughts.Infrastructure.Data;

namespace Thoughts.Infrastructure.Repositories
{
	public class LikesRepository : ILikesRepository
	{
		private readonly ThoughtDataContext _dbContext;

		public LikesRepository(ThoughtDataContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Like> Add(Like like)
		{
			_dbContext.Likes.Add(like);
			await _dbContext.SaveChangesAsync();
			return like;
		}

		public async Task<Like> Get(Guid thoughtId, string userId)
		{
			var like = await _dbContext.Likes.FirstOrDefaultAsync(l => l.ThoughtId == thoughtId && l.UserId == userId);
			return like;
		}

		public async Task<int> Remove(Guid thoughtId, string userId)
		{
			var like = await Get(thoughtId, userId);
			if (like != null)
			{
				_dbContext.Likes.Remove(like);
				return await _dbContext.SaveChangesAsync();
			}
			else
			{
				return 0;
			}		
		}
	}
}
