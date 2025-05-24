namespace CarReservation.Application.Common.Interfaces;

using MediatR;
using CarReservation.Domain.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
