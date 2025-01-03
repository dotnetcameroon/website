using app.domain.Models.ExternalAppAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.infrastructure.Persistence.Configurations;
public class ApplicationConfigurations : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("ExternalApplications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ApplicationId");
        builder.HasIndex(x => x.ClientId).IsUnique();
        builder.HasIndex(x => x.ClientSecret).IsUnique();
    }
}
