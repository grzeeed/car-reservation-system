namespace CarReservation.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasConversion(
                v => v.Value,
                v => ReservationId.From(v))
            .ValueGeneratedNever();

        builder.Property(r => r.CarId)
            .HasConversion(
                v => v.Value,
                v => CarId.From(v))
            .IsRequired();

        builder.Property(r => r.CustomerId)
            .HasConversion(
                v => v.Value,
                v => CustomerId.From(v))
            .IsRequired();

        builder.OwnsOne(r => r.Period, period =>
        {
            period.Property(p => p.StartDate)
                .HasColumnName("StartDate")
                .IsRequired();
                
            period.Property(p => p.EndDate)
                .HasColumnName("EndDate")
                .IsRequired();
        });

        builder.OwnsOne(r => r.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("TotalCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.ConfirmedAt);

        builder.Property(r => r.CancelledAt);

        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.CarId);
        builder.HasIndex(r => r.Status);

        builder.Ignore(r => r.DomainEvents);
    }
}