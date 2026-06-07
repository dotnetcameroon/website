using app.domain.Models.BannerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.infrastructure.Persistence.Configurations;

public class BannerConfigurations : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("Banners");
        builder.Property(b => b.MessageEn).IsRequired().HasMaxLength(500);
        builder.Property(b => b.MessageFr).IsRequired().HasMaxLength(500);
        builder.Property(b => b.Link).HasMaxLength(2048);
        builder.Property(b => b.LinkLabelEn).HasMaxLength(100);
        builder.Property(b => b.LinkLabelFr).HasMaxLength(100);
        builder.HasIndex(b => new { b.IsEnabled, b.StartDate, b.EndDate });
    }
}
