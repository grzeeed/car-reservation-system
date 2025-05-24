# Run in production mode
Write-Host "Starting Car Reservation System in Production mode..." -ForegroundColor Green
Write-Host "Frontend will be available at: http://localhost" -ForegroundColor Cyan
Write-Host "Backend API will be available at: http://localhost:5170" -ForegroundColor Cyan

# Stop existing containers
docker-compose down

# Build and start containers for production
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --build -d

Write-Host "`nProduction services started!" -ForegroundColor Green
Write-Host "Frontend: http://localhost" -ForegroundColor Yellow
Write-Host "API: http://localhost:5170" -ForegroundColor Yellow
