using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
{
    public void Configure(EntityTypeBuilder<SensorReading> builder)
    {
        builder.HasKey(reading => reading.Id);
        builder.Property(reading => reading.Id).ValueGeneratedNever();
        builder.Property(reading => reading.IotDeviceId).IsRequired();
        builder.Property(reading => reading.RecordedAt).IsRequired();
        builder.Property(reading => reading.Latitude).HasPrecision(9, 6);
        builder.Property(reading => reading.Longitude).HasPrecision(9, 6);
        builder.Property(reading => reading.TemperatureCelsius).HasPrecision(5, 2);
        builder.Property(reading => reading.ImpactForce).HasPrecision(5, 2);
        builder.HasOne<IotDevice>()
            .WithMany()
            .HasForeignKey(reading => reading.IotDeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
