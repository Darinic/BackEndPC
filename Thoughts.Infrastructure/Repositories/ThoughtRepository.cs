using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Drawing.Printing;
using System.Linq.Expressions;
using Thoughts.Core.Interfaces;
using Thoughts.Domain.Entities;
using Thoughts.Infrastructure.Data;

namespace Thoughts.Infrastructure.Repositories
{
    public class ThoughtRepository : IThoughtRepository
    {
        private readonly ThoughtDataContext _dbContext;

        public ThoughtRepository(ThoughtDataContext dbContext)
        {
            _dbContext = dbContext;
        }

		public async Task Add(Thought thought)
		{
			await _dbContext.Thoughts.AddAsync(thought);
			await _dbContext.SaveChangesAsync();
		}
		
		public async Task<IEnumerable<Thought?>> GetAll(int page, int pageSize)
		{
			var thoughts = await _dbContext.Thoughts
				.Include(x => x.User)
				.OrderByDescending(x => x.CreationDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return thoughts;
		}
		
		public async Task<Thought?> GetById(Guid thoughtId)
		{
			var thought = await _dbContext.Thoughts
				.Include(x => x.User)
				.OrderByDescending(x => x.CreationDate)
				.FirstOrDefaultAsync(x => x.Id == thoughtId);

			return thought;
		}
		
		public async Task Update(Thought thought)
		{
			_dbContext.Thoughts.Attach(thought);
			_dbContext.Thoughts.Update(thought);
			await _dbContext.SaveChangesAsync();
		}
	
		public async Task Delete(Thought thought)
		{
			_dbContext.Thoughts.Remove(thought);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<Thought>> GetUserThoughts(string username, int page, int pageSize)
		{
			var thoughts = await _dbContext.Thoughts
				.Include(x => x.User)
				.OrderByDescending(x => x.CreationDate)
				.Where(x => x.User.UserName == username)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return thoughts;
		}

		public async Task<IEnumerable<Thought>> GetThoughtsBySearch(string searchTerm, int page, int pageSize)
		{
			var thoughts = await _dbContext.Thoughts
			.OrderByDescending(x => x.CreationDate)
			.Where(t => t.ThoughtMessage.ToLower().Contains(searchTerm.ToLower()) ||
						t.FirstHashtag.ToLower().Contains(searchTerm.ToLower()) ||
						t.SecondHashtag.ToLower().Contains(searchTerm.ToLower()))
						.Skip((page - 1) * pageSize)
						.Take(pageSize)
						.ToListAsync();
			
			return thoughts;
		}

		public async Task<IEnumerable<Thought>> GetTop9ThoughtsOfAllTime()
		{
			var thoughts = await _dbContext.Thoughts
				.Include(x => x.User)
				.OrderByDescending(x => x.Likes.Count)
				.Take(9)
				.ToListAsync();

			return thoughts;
		}

		public async Task<IEnumerable<Thought>> GetTop9ThoughtsOfLast30Days()
		{
			var thirtyDaysAgo = DateTime.Now.AddDays(-30);
			var thoughts = await _dbContext.Thoughts
				.Include(x => x.User)
				.Where(x => x.CreationDate >= thirtyDaysAgo)
				.OrderByDescending(x => x.Likes.Count)
				.Take(9)
				.ToListAsync();

			return thoughts;
		}

		public async Task<IEnumerable<Thought>> GetTop9ThoughtsOfLastWeek()
		{
			var sevenDaysAgo = DateTime.Now.AddDays(-7);
			var thoughts = await _dbContext.Thoughts
				.Include(x => x.User)
				.Where(x => x.CreationDate >= sevenDaysAgo)
				.OrderByDescending(x => x.Likes.Count)
				.Take(9)
				.ToListAsync();

			return thoughts;
		}

		public async Task<int> GetThoughtCount(Expression<Func<Thought, bool>> filter = null)
		{
			return await _dbContext.Thoughts.CountAsync();
		}
	}
}
