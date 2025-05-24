namespace CarReservation.Application.Common.Interfaces;

using MediatR;
using CarReservation.Domain.Common;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
