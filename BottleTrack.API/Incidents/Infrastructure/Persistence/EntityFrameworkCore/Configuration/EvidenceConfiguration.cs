using BottleTrack.API.Incidents.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> builder)
    {
        builder.HasKey(evidence => evidence.Id);
        builder.Property(evidence => evidence.Id).ValueGeneratedNever();
        builder.Property(evidence => evidence.IncidentId).IsRequired();
        builder.Property(evidence => evidence.FileUrl).IsRequired().HasMaxLength(500);
        builder.Property(evidence => evidence.FileType).IsRequired().HasMaxLength(30);
        builder.Property(evidence => evidence.UploadedBy).IsRequired();
    }
}
