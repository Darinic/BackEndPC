using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thoughts.Domain.Entities;
using Thoughts.Infrastructure.Data.Configuration;

namespace Thoughts.Infrastructure.Data
{
	public class ThoughtDataContext : IdentityDbContext<User>
	{

		public DbSet<ThoughtComment> ThoughtComments { get; set; }

		public ThoughtDataContext(DbContextOptions<ThoughtDataContext> options) : base(options)
		{
		}
		public DbSet<Thought> Thoughts { get; set; }

		public DbSet<Like> Likes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThoughtConfiguration).Assembly);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(LikeConfiguration).Assembly);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThoughtCommentConfiguration).Assembly);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
		}
	}
}
