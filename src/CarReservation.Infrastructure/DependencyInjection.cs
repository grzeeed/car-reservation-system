namespace CarReservation.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarReservation.Domain.Interfaces;
using CarReservation.Infrastructure.Persistence;
using CarReservation.Infrastructure.Persistence.Repositories;
using CarReservation.Application.Interfaces;

using CarReservation.Infrastructure.Services;
using CarReservation.Application.Interfaces;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CarReservationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CarReservationDbContext).Assembly.FullName)));

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICarReadRepository, CarReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Authentication services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
