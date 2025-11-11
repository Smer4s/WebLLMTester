using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class TaskReportConfig : IEntityTypeConfiguration<TaskReport>
{
    public void Configure(EntityTypeBuilder<TaskReport> builder)
    {
        builder.ToTable(nameof(TaskReport));
        builder.HasKey(x => x.Id);
    }
}

