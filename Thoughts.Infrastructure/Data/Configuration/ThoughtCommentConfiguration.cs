using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoughts.Domain.Entities;

namespace Thoughts.Infrastructure.Data.Configuration
{
    public class ThoughtCommentConfiguration : IEntityTypeConfiguration<ThoughtComment>
    {
		public void Configure(EntityTypeBuilder<ThoughtComment> builder)
		{
			builder.HasKey(tc => tc.Id);

			builder.Property(x => x.CommentMessage).IsRequired();
			builder.Property(x => x.CreationDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

			builder.HasOne(x => x.Thought)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.ThoughtId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.User)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
