using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.ThoughtComment
{
	public class AddCommentRequest
	{
		public string? UserId { get; set; }

		public Guid ThoughtId { get; set; }

		[Required]
		[StringLength(40, MinimumLength = 5)]
		[RegularExpression(@"^(?!.*(MMM|WWW|WMW|MWM|W\sW\sW|M\sM\sM|W\sM\sW|M\sW\sW)).+$", ErrorMessage = "Invalid message(Spam Filter)")]
		public string? CommentMessage { get; set; }
	}
}
