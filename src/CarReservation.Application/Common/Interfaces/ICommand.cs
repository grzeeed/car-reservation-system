namespace CarReservation.Application.Common.Interfaces;

using MediatR;
using CarReservation.Domain.Common;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
