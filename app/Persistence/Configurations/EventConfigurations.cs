using app.Models.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.Configurations;
public class EventConfigurations : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.ComplexProperty(e => e.Schedule);
        builder.HasMany(e => e.Partners).WithMany();
        builder.HasMany(e => e.Activities).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Activities).Metadata.SetField("_activities");
        builder.Metadata.FindNavigation(nameof(Event.Activities))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(c => c.Partners).Metadata.SetField("_partners");
        builder.Metadata.FindNavigation(nameof(Event.Partners))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
