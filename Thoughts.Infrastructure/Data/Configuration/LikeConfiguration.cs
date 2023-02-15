using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thoughts.Domain.Entities;

namespace Thoughts.Infrastructure.Data.Configuration
{
	public class LikeConfiguration : IEntityTypeConfiguration<Like>
	{
		public void Configure(EntityTypeBuilder<Like> builder)
		{
			builder.ToTable("Likes");

			builder.HasKey(l => new { l.ThoughtId, l.UserId });

			builder.HasOne(l => l.Thought)
				.WithMany(t => t.Likes)
				.HasForeignKey(l => l.ThoughtId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(l => l.User)
				.WithMany(u => u.Likes)
				.HasForeignKey(l => l.UserId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
