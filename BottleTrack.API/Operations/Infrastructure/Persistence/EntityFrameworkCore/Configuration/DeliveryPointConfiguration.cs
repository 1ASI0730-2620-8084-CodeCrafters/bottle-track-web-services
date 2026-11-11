using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class DeliveryPointConfiguration : IEntityTypeConfiguration<DeliveryPoint>
{
    public void Configure(EntityTypeBuilder<DeliveryPoint> builder)
    {
        builder.HasKey(deliveryPoint => deliveryPoint.Id);
        builder.Property(deliveryPoint => deliveryPoint.Id).ValueGeneratedNever();
        builder.Property(deliveryPoint => deliveryPoint.OrganizationId).IsRequired();
        builder.Property(deliveryPoint => deliveryPoint.BusinessName).IsRequired().HasMaxLength(255);
        builder.Property(deliveryPoint => deliveryPoint.Address).IsRequired().HasMaxLength(255);
        builder.Property(deliveryPoint => deliveryPoint.District).IsRequired().HasMaxLength(100);
        builder.Property(deliveryPoint => deliveryPoint.Latitude).HasPrecision(9, 6);
        builder.Property(deliveryPoint => deliveryPoint.Longitude).HasPrecision(9, 6);
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(deliveryPoint => deliveryPoint.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
