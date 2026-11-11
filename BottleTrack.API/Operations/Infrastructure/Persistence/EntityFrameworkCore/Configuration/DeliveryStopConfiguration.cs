using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class DeliveryStopConfiguration : IEntityTypeConfiguration<DeliveryStop>
{
    public void Configure(EntityTypeBuilder<DeliveryStop> builder)
    {
        builder.HasKey(deliveryStop => deliveryStop.Id);
        builder.Property(deliveryStop => deliveryStop.Id).ValueGeneratedNever();
        builder.Property(deliveryStop => deliveryStop.TransportOperationId).IsRequired();
        builder.Property(deliveryStop => deliveryStop.DeliveryPointId).IsRequired();
        builder.Property(deliveryStop => deliveryStop.SequenceNumber).IsRequired();
        builder.Property(deliveryStop => deliveryStop.PlannedQuantity).IsRequired();
        builder.Property(deliveryStop => deliveryStop.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.Property(deliveryStop => deliveryStop.FailureReason).HasMaxLength(255);
        builder.HasOne<DeliveryPoint>()
            .WithMany()
            .HasForeignKey(deliveryStop => deliveryStop.DeliveryPointId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
