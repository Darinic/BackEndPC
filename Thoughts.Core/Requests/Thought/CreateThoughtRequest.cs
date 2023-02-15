using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Thought
{
	public class CreateThoughtRequest
    {
		public string? UserId { get; set; }
		
		[Required]
		[StringLength(200, MinimumLength = 20)]
		[RegularExpression(@"^(?!.*(MMM|WWW|WMW|MWM|W\sW\sW|M\sM\sM|W\sM\sW|M\sW\sW)).+$", ErrorMessage = "Invalid message(Spam Filter)")]
		public string ThoughtMessage { get; set; }
		
		[Required]
		[StringLength(10, MinimumLength = 2)]
		public string FirstHashtag { get; set; }
		
		[Required]
		[StringLength(10, MinimumLength = 2)]
		public string SecondHashtag { get; set; } 
    }
}
