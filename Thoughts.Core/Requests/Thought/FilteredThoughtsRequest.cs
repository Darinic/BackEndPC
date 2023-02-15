using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Thought
{
	public class FilteredThoughtsRequest
	{
		[Required]
		[MinLength(3)]
		public string? SearchTerm { get; set; }

		public int Page { get; set; }

		public int PageSize { get; set; }
	}
}
