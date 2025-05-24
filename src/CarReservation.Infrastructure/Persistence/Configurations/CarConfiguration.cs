namespace CarReservation.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;
using CarReservation.Domain.Enums;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasConversion(
                v => v.Value,
                v => CarId.From(v))
            .ValueGeneratedNever();

        builder.Property(c => c.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.OwnsOne(c => c.PricePerDay, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("PricePerDay")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(c => c.CurrentLocation, location =>
        {
            location.Property(l => l.City)
                .HasColumnName("LocationCity")
                .HasMaxLength(100)
                .IsRequired();
                
            location.Property(l => l.Address)
                .HasColumnName("LocationAddress")
                .HasMaxLength(200)
                .IsRequired();
                
            location.Property(l => l.Latitude)
                .HasColumnName("LocationLatitude")
                .IsRequired();
                
            location.Property(l => l.Longitude)
                .HasColumnName("LocationLongitude")
                .IsRequired();
        });

        builder.HasMany(c => c.Reservations)
            .WithOne()
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(c => c.DomainEvents);
    }
}
