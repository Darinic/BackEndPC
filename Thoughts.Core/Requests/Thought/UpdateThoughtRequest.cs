using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Thought
{
    public class UpdateThoughtRequest
    {	
		public string? UserId { get; set; }
		
		public Guid ThoughtId { get; set; }
		
		[Required]
		[StringLength(200, MinimumLength = 20)]
		public string? ThoughtMessage { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 2)]
		public string? FirstHashtag { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 2)]
		public string? SecondHashtag { get; set; }
	}
}
