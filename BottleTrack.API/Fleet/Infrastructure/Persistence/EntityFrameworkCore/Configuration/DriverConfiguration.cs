using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(driver => driver.Id);
        builder.Property(driver => driver.Id).ValueGeneratedNever();
        builder.Property(driver => driver.FirstName).IsRequired().HasMaxLength(80);
        builder.Property(driver => driver.LastName).IsRequired().HasMaxLength(80);
        builder.Property(driver => driver.DocumentNumber).IsRequired().HasMaxLength(8);
        builder.Property(driver => driver.LicenseNumber)
            .HasConversion(licenseNumber => licenseNumber.Value, value => new LicenseNumber(value))
            .IsRequired()
            .HasMaxLength(9);
        builder.Property(driver => driver.Phone).IsRequired().HasMaxLength(30);
        builder.Property(driver => driver.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.HasIndex(driver => driver.DocumentNumber).IsUnique();
        builder.HasIndex(driver => driver.LicenseNumber).IsUnique();
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(driver => driver.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
