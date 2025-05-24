# Build Docker image
Write-Host "Building Docker image..." -ForegroundColor Green
docker build -t car-reservation-api:latest .

Write-Host "Docker image built successfully!" -ForegroundColor Green
