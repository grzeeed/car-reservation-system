# Stop Docker containers
Write-Host "Stopping Car Reservation System..." -ForegroundColor Yellow
docker-compose down

Write-Host "All services stopped!" -ForegroundColor Green
