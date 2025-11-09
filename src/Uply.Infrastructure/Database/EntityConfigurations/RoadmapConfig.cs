using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class RoadmapConfig : IEntityTypeConfiguration<Roadmap>
{
    public void Configure(EntityTypeBuilder<Roadmap> builder)
    {
        builder.ToTable(nameof(Roadmap));
        builder.HasKey(x => x.Id);

        builder.HasMany(r => r.Tasks)
            .WithOne(t => t.Roadmap)
            .HasForeignKey(t => t.RoadmapId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.CurrentTask);
        builder.Ignore(x => x.NextTask);
    }
}
