using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.Domain.Entities
{
	public class Like
	{
		public string UserId { get; set; }
		public virtual User User { get; set; }
		public Guid ThoughtId { get; set; }
		public virtual Thought Thought { get; set; }
	}
}
