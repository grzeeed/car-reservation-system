namespace CarReservation.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;
using CarReservation.Domain.Enums;

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
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastLoginAt);

        // Configure UserProfile as owned entity
        builder.OwnsOne(u => u.Profile, profile =>
        {
            profile.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(50);
                
            profile.Property(p => p.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50);
                
            profile.Property(p => p.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);

            // Configure Address as owned entity within Profile
            profile.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("AddressStreet")
                    .HasMaxLength(200);
                    
                address.Property(a => a.City)
                    .HasColumnName("AddressCity")
                    .HasMaxLength(100);
                    
                address.Property(a => a.PostalCode)
                    .HasColumnName("AddressPostalCode")
                    .HasMaxLength(20);
                    
                address.Property(a => a.Country)
                    .HasColumnName("AddressCountry")
                    .HasMaxLength(100);
            });
        });

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Role);

        builder.Ignore(u => u.DomainEvents);
    }
}
