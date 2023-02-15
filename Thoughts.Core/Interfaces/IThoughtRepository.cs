using System.Linq.Expressions;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Interfaces
{
	public interface IThoughtRepository
    {
		Task Add(Thought thought);
		Task<IEnumerable<Thought?>> GetAll(int page, int pageSize);
		Task<Thought?> GetById(Guid thoughtId);
		Task Update(Thought thought);
		Task Delete(Thought thought);
		Task<IEnumerable<Thought>> GetUserThoughts(string username, int page, int pageSize);
		Task<IEnumerable<Thought>> GetThoughtsBySearch(string searchTerm, int page, int pageSize);
		Task<IEnumerable<Thought>> GetTop9ThoughtsOfAllTime();
		Task<IEnumerable<Thought>> GetTop9ThoughtsOfLast30Days();
		Task<IEnumerable<Thought>> GetTop9ThoughtsOfLastWeek();
		Task<int> GetThoughtCount(Expression<Func<Thought, bool>> filter = null);
	}
}
