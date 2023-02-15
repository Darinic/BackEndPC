using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoughts.Domain.Entities;

namespace Thoughts.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

			builder.HasMany(x => x.Thoughts)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId);

			builder.HasMany(x => x.Likes)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId);

		}
    }
}
