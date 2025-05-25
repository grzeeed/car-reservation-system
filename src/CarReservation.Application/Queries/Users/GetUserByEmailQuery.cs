namespace CarReservation.Application.Queries.Users;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;

public record GetUserByEmailQuery(string Email) : IQuery<UserDto?>;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto?>> Handle(
        GetUserByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        var userDto = user?.ToDto();
        return Result<UserDto?>.Success(userDto);
    }
}
