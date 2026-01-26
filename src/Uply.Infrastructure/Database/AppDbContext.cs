using Microsoft.EntityFrameworkCore;
using Uply.Domain.Entities;

namespace Uply.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Roadmap> Roadmaps { get; set; } = null!;
    public DbSet<RoadmapTask> RoadmapTasks { get; set; } = null!;
    public DbSet<Skin> Skins { get; set; } = null!;
    public DbSet<UserSkin> UserSkins { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
