# Run with Docker Compose
Write-Host "Starting Car Reservation System with Docker Compose..." -ForegroundColor Green

# Stop existing containers
docker-compose down

# Build and start containers
docker-compose up --build -d

Write-Host "`nServices are starting..." -ForegroundColor Yellow
Write-Host "API will be available at: http://localhost:5170" -ForegroundColor Cyan
Write-Host "SQL Server will be available at: localhost,1433" -ForegroundColor Cyan
Write-Host "`nTo view logs: docker-compose logs -f" -ForegroundColor Gray
