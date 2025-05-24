namespace CarReservation.Domain.Common;

using MediatR;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn => DateTime.UtcNow;
}
