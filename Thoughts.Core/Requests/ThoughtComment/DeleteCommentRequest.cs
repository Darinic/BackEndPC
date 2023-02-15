using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.ThoughtComment
{
	public class DeleteCommentRequest
	{
		public string? UserId { get; set; }
		
		[Required]
		public Guid CommentId { get; set; }
	}
}
