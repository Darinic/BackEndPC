using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoughts.Domain.Entities;

namespace Thoughts.Infrastructure.Data.Configuration
{
    public class ThoughtConfiguration : IEntityTypeConfiguration<Thought>
    {
        public void Configure(EntityTypeBuilder<Thought> builder)
        {
            builder.HasKey(t => t.Id);
			builder.Property(x => x.CreationDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

			builder.HasOne(x => x.User)
				.WithMany(x => x.Thoughts)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Likes)
				.WithOne(x => x.Thought)
				.HasForeignKey(x => x.ThoughtId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Comments)
				.WithOne(x => x.Thought)
				.HasForeignKey(x => x.ThoughtId)
				.OnDelete(DeleteBehavior.Cascade);
		}
    }
}
