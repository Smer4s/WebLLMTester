using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database.EntityConfigurations;

public class UserSkinConfig : IEntityTypeConfiguration<UserSkin>
{
    public void Configure(EntityTypeBuilder<UserSkin> builder)
    {
        builder.ToTable(nameof(UserSkin));
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Skin)
            .WithMany()
            .HasForeignKey(x => x.SkinId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}