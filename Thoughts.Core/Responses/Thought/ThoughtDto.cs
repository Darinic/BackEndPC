
namespace Thoughts.Core.Responses.Thought
{
	public class ThoughtDto
	{
		public string? Id { get; set; }
		public string? UserName { get; set; }
		public string? ThoughtMessage { get; set; }
		public string? FirstHashtag { get; set; }
		public string? SecondHashtag { get; set; }
		public bool IsLiked { get; set; }
		public int LikesCount { get; set; }
		public int CommentsCount { get; set; }
	}
}
