using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.HasKey(incident => incident.Id);
        builder.Property(incident => incident.Id).ValueGeneratedNever();
        builder.Property(incident => incident.Code).IsRequired().HasMaxLength(10);
        builder.Property(incident => incident.TransportOperationId).IsRequired();
        builder.Property(incident => incident.ReportedBy).IsRequired();
        builder.Property(incident => incident.Source).HasConversion<string>().IsRequired().HasMaxLength(30);
        builder.Property(incident => incident.Type).HasConversion<string>().IsRequired().HasMaxLength(30);
        builder.Property(incident => incident.Severity).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.Property(incident => incident.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.Property(incident => incident.Description).IsRequired().HasMaxLength(500);
        builder.Property(incident => incident.Resolution).HasMaxLength(500);
        builder.HasIndex(incident => incident.Code).IsUnique();
        builder.HasMany(incident => incident.Evidences)
            .WithOne()
            .HasForeignKey(evidence => evidence.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
