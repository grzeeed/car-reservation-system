# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["CarReservationSystem.sln", "./"]
COPY ["src/CarReservation.Domain/CarReservation.Domain.csproj", "src/CarReservation.Domain/"]
COPY ["src/CarReservation.Application/CarReservation.Application.csproj", "src/CarReservation.Application/"]
COPY ["src/CarReservation.Infrastructure/CarReservation.Infrastructure.csproj", "src/CarReservation.Infrastructure/"]
COPY ["src/CarReservation.API/CarReservation.API.csproj", "src/CarReservation.API/"]

# Restore packages
RUN dotnet restore "src/CarReservation.API/CarReservation.API.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/src/CarReservation.API"
RUN dotnet build "CarReservation.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CarReservation.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "CarReservation.API.dll"]
