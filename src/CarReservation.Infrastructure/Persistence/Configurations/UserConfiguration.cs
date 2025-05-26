namespace CarReservation.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasConversion(
                v => v.Value,
                v => UserId.From(v))
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(254);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastLoginAt);

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.OwnsOne(u => u.Profile, profile =>
        {
            profile.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(50)
                .IsRequired();

            profile.Property(p => p.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50)
                .IsRequired();

            profile.Property(p => p.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20);

            profile.Property(p => p.IsProfileComplete)
                .HasColumnName("IsProfileComplete")
                .IsRequired();

            profile.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("AddressStreet")
                    .HasMaxLength(200);

                address.Property(a => a.City)
                    .HasColumnName("AddressCity")
                    .HasMaxLength(100);

                address.Property(a => a.State)
                    .HasColumnName("AddressState")
                    .HasMaxLength(50);

                address.Property(a => a.PostalCode)
                    .HasColumnName("AddressZipCode")
                    .HasMaxLength(20);

                address.Property(a => a.Country)
                    .HasColumnName("AddressCountry")
                    .HasMaxLength(100);
            });
        });

        builder.Ignore(u => u.DomainEvents);
    }
}
