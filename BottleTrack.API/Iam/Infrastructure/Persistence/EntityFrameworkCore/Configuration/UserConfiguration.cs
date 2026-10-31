using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BottleTrack.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedNever();
        builder.Property(user => user.FirstName).IsRequired().HasMaxLength(80);
        builder.Property(user => user.LastName).IsRequired().HasMaxLength(80);
        builder.Property(user => user.Email)
            .HasConversion(email => email.Value, value => new EmailAddress(value))
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(user => user.Role).HasConversion<string>().IsRequired().HasMaxLength(30);
        builder.Property(user => user.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(user => user.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
