
namespace Thoughts.Core.Responses.ThoughtComment
{
	public class ThoughtCommentsResponse
	{
		public Guid Id { get; set; }
		public string? CommentMessage { get; set; }
		public string? UserName { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
