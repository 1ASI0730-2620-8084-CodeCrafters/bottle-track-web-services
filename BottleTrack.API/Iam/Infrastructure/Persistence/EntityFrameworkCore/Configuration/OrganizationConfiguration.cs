using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(organization => organization.Id);
        builder.Property(organization => organization.Id).ValueGeneratedNever();
        builder.Property(organization => organization.BusinessName).IsRequired().HasMaxLength(150);
        builder.Property(organization => organization.TaxId)
            .HasConversion(taxId => taxId.Value, value => new TaxId(value))
            .IsRequired()
            .HasMaxLength(11);
        builder.Property(organization => organization.Email)
            .HasConversion(email => email.Value, value => new EmailAddress(value))
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(organization => organization.Phone).IsRequired().HasMaxLength(30);
        builder.Property(organization => organization.Address).IsRequired().HasMaxLength(255);
        builder.HasIndex(organization => organization.TaxId).IsUnique();
    }
}
