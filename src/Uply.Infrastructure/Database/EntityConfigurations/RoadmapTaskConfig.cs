using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class RoadmapTaskConfig : IEntityTypeConfiguration<RoadmapTask>
{
    public void Configure(EntityTypeBuilder<RoadmapTask> builder)
    {
        builder.ToTable(nameof(RoadmapTask));
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.TaskReport)
            .WithOne(x => x.Task)
            .HasForeignKey<RoadmapTask>(x => x.TaskReportId)
            .IsRequired(false);

        builder.Ignore(x => x.IsActiveTask);
    }
}
