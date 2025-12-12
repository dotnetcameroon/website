using app.domain.Models.ProjectsAggregate;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Persistence;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title);
            entity.HasIndex(e => e.Title).IsUnique();
        });
    }
}
