using System.ComponentModel.DataAnnotations;
namespace Thoughts.Core.Requests.Thought
{
	public class GetThoughtRequest
	{
		[Required]
		public Guid ThoughtId { get; set; }
	}
}
