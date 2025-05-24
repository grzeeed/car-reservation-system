# Run in development mode with hot reload
Write-Host "Starting Car Reservation System in Development mode..." -ForegroundColor Green
Write-Host "Frontend will be available at: http://localhost:3000" -ForegroundColor Cyan
Write-Host "Backend API will be available at: http://localhost:5170" -ForegroundColor Cyan
Write-Host "Swagger UI will be available at: http://localhost:5170/swagger" -ForegroundColor Cyan

# Stop existing containers
docker-compose down

# Build and start containers with override
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build

