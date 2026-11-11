using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class TransportOperationConfiguration : IEntityTypeConfiguration<TransportOperation>
{
    public void Configure(EntityTypeBuilder<TransportOperation> builder)
    {
        builder.HasKey(operation => operation.Id);
        builder.Property(operation => operation.Id).ValueGeneratedNever();
        builder.Property(operation => operation.OrganizationId).IsRequired();
        builder.Property(operation => operation.Code).IsRequired().HasMaxLength(10);
        builder.Property(operation => operation.Description).IsRequired().HasMaxLength(255);
        builder.Property(operation => operation.ScheduledDate)
            .HasConversion(date => date.ToDateTime(TimeOnly.MinValue), value => DateOnly.FromDateTime(value))
            .IsRequired();
        builder.Property(operation => operation.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.Property(operation => operation.CancellationReason).HasMaxLength(255);
        builder.HasIndex(operation => operation.Code).IsUnique();
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(operation => operation.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(operation => operation.DeliveryStops)
            .WithOne()
            .HasForeignKey(deliveryStop => deliveryStop.TransportOperationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
