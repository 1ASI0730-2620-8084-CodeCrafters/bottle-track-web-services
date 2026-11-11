using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class IotDeviceConfiguration : IEntityTypeConfiguration<IotDevice>
{
    public void Configure(EntityTypeBuilder<IotDevice> builder)
    {
        builder.HasKey(device => device.Id);
        builder.Property(device => device.Id).ValueGeneratedNever();
        builder.Property(device => device.SerialNumber).IsRequired().HasMaxLength(60);
        builder.Property(device => device.SafeTemperatureMin).HasPrecision(5, 2).IsRequired();
        builder.Property(device => device.SafeTemperatureMax).HasPrecision(5, 2).IsRequired();
        builder.Property(device => device.ImpactThreshold).HasPrecision(5, 2).IsRequired();
        builder.Property(device => device.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.HasIndex(device => device.SerialNumber).IsUnique();
    }
}
