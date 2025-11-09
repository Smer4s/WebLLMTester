using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        builder.HasMany(u => u.Roadmaps)
            .WithOne(r => r.Issuer)
            .HasForeignKey(r => r.IssuerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
