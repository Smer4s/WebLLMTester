using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class SkinConfig : IEntityTypeConfiguration<Skin>
{
    public void Configure(EntityTypeBuilder<Skin> builder)
    {
        builder.ToTable(nameof(Skin));
        builder.HasKey(x => x.Id);
    }
}