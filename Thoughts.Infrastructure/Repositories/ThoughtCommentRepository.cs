using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using Thoughts.Core.Interfaces;
using Thoughts.Domain.Entities;
using Thoughts.Infrastructure.Data;

namespace Thoughts.Infrastructure.Repositories
{
	public class ThoughtCommentRepository : IThoughtCommentRepository
	{
		private readonly ThoughtDataContext _dbContext;

		public ThoughtCommentRepository(ThoughtDataContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public async Task Add(ThoughtComment thoughtComment)
		{
			await _dbContext.ThoughtComments.AddAsync(thoughtComment);
			await _dbContext.SaveChangesAsync();
		}
		
		public async Task Delete(Guid id)
		{
			var thoughtComment = _dbContext.ThoughtComments.Find(id);
			_dbContext.ThoughtComments.Remove(thoughtComment);
			await _dbContext.SaveChangesAsync();
		}
		
		public async Task<IEnumerable<ThoughtComment>> GetAll(Guid thoughtId)
		{
			var thoughtComments = await _dbContext.ThoughtComments
				.Where(x => x.ThoughtId == thoughtId)
				.OrderByDescending(x => x.CreationDate)
				.ToListAsync();

			return thoughtComments;
		}

		public async Task<ThoughtComment> GetById(Guid commentId)
		{
			var thoughtComment = await _dbContext.ThoughtComments
			.FirstOrDefaultAsync(c => c.Id == commentId);
			return thoughtComment;
		}

		public async Task<ThoughtComment> RetrieveLastComment(string userId)
		{
			var lastComment = await _dbContext.ThoughtComments
				.OrderByDescending(tc => tc.CreationDate)
				.FirstOrDefaultAsync(tc => tc.UserId == userId);

			return lastComment;
		}
	}
}
