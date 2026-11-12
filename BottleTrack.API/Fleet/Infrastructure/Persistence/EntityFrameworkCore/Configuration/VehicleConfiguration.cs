using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(vehicle => vehicle.Id);
        builder.Property(vehicle => vehicle.Id).ValueGeneratedNever();
        builder.Property(vehicle => vehicle.PlateNumber)
            .HasConversion(plateNumber => plateNumber.Value, value => new PlateNumber(value))
            .IsRequired()
            .HasMaxLength(7);
        builder.Property(vehicle => vehicle.Brand).IsRequired().HasMaxLength(60);
        builder.Property(vehicle => vehicle.Model).IsRequired().HasMaxLength(60);
        builder.Property(vehicle => vehicle.CapacityInBoxes).IsRequired();
        builder.Property(vehicle => vehicle.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.HasIndex(vehicle => vehicle.PlateNumber).IsUnique();
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(vehicle => vehicle.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
