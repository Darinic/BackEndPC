using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.Domain.Entities
{
    public class Thought : BaseEntity
    {
		public string? UserId { get; set; }
		public virtual User? User { get; set; }
		public string? ThoughtMessage { get; set; }
		public string? FirstHashtag { get; set; }
		public string? SecondHashtag { get; set; }
		public virtual ICollection<Like>? Likes { get; set; } = new List<Like>();
		public virtual ICollection<ThoughtComment>? Comments { get; set; } = new List<ThoughtComment>();
	}
}
