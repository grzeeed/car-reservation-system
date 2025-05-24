namespace CarReservation.Infrastructure.Data;

using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;
using CarReservation.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(CarReservationDbContext context)
    {
        if (context.Cars.Any())
            return; // Database has been seeded

        var cars = new List<Car>
        {
            new Car(
                CarId.Create(),
                "Toyota",
                "Camry",
                "NYC001",
                CarType.Sedan,
                new Money(85.00m, "USD"),
                new Location("New York", "123 Broadway Ave", 40.7589, -73.9851)
            ),
            new Car(
                CarId.Create(),
                "Honda", 
                "CR-V",
                "NYC002",
                CarType.SUV,
                new Money(95.00m, "USD"),
                new Location("New York", "456 Park Ave", 40.7614, -73.9776)
            ),
            new Car(
                CarId.Create(),
                "BMW",
                "3 Series",
                "NYC003", 
                CarType.Sedan,
                new Money(120.00m, "USD"),
                new Location("New York", "789 Fifth Ave", 40.7637, -73.9731)
            ),
            new Car(
                CarId.Create(),
                "Ford",
                "Mustang",
                "LA001",
                CarType.Coupe,
                new Money(110.00m, "USD"),
                new Location("Los Angeles", "100 Hollywood Blvd", 34.1016, -118.3406)
            ),
            new Car(
                CarId.Create(),
                "Tesla",
                "Model 3",
                "SF001",
                CarType.Sedan,
                new Money(130.00m, "USD"),
                new Location("San Francisco", "200 Market St", 37.7749, -122.4194)
            ),
            new Car(
                CarId.Create(),
                "Jeep",
                "Wrangler",
                "CHI001",
                CarType.SUV,
                new Money(100.00m, "USD"),
                new Location("Chicago", "300 Michigan Ave", 41.8781, -87.6298)
            ),
            new Car(
                CarId.Create(),
                "Audi",
                "A4",
                "MIA001",
                CarType.Sedan,
                new Money(105.00m, "USD"),
                new Location("Miami", "400 Ocean Drive", 25.7617, -80.1918)
            ),
            new Car(
                CarId.Create(),
                "Mercedes",
                "C-Class",
                "SEA001",
                CarType.Sedan,
                new Money(115.00m, "USD"),
                new Location("Seattle", "500 Pine St", 47.6062, -122.3321)
            )
        };

        // Clear domain events before saving
        foreach (var car in cars)
        {
            car.ClearDomainEvents();
        }

        context.Cars.AddRange(cars);
        await context.SaveChangesAsync();
    }
}
