using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(alert => alert.Id);
        builder.Property(alert => alert.Id).ValueGeneratedNever();
        builder.Property(alert => alert.IotDeviceId).IsRequired();
        builder.Property(alert => alert.TransportOperationId).IsRequired();
        builder.Property(alert => alert.Type).HasConversion<string>().IsRequired().HasMaxLength(30);
        builder.Property(alert => alert.RaisedAt).IsRequired();
        builder.HasOne<IotDevice>()
            .WithMany()
            .HasForeignKey(alert => alert.IotDeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
