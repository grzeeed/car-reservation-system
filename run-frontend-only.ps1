# Run only frontend in development mode
Write-Host "Starting Frontend only in Development mode..." -ForegroundColor Green
Write-Host "Frontend will be available at: http://localhost:3000" -ForegroundColor Cyan
Write-Host "Make sure backend API is running at: http://localhost:5170" -ForegroundColor Yellow

# Stop existing frontend container
docker-compose stop frontend
docker-compose rm -f frontend

# Build and start only frontend
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build frontend

