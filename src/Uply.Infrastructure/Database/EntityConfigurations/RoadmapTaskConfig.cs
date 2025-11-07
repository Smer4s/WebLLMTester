using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class RoadmapTaskConfig : IEntityTypeConfiguration<RoadmapTask>
{
    public void Configure(EntityTypeBuilder<RoadmapTask> builder)
    {
        builder.ToTable(nameof(RoadmapTask));
        builder.HasKey(x => x.Id);
    }
}
