using Microsoft.AspNetCore.Identity;

namespace Thoughts.Domain.Entities
{
    public class User : IdentityUser
    {
		public virtual ICollection<Thought> Thoughts { get; set; }

		public virtual ICollection<Like>? Likes { get; set; }

		public virtual ICollection<ThoughtComment> Comments { get; set; }

		//public byte[]? ProfilePicture { get; set; }

		public User() 
		{
			Thoughts = new List<Thought>();
			Likes = new List<Like>();
			Comments = new List<ThoughtComment>();
		}
	}

}

