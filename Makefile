.PHONY: help build run run-dev run-prod run-frontend stop clean logs test install health

help:
	@echo "🚀 Car Reservation System - Available Commands:"
	@echo ""
	@echo "📦 Docker Commands:"
	@echo "  make build      - Build all Docker images"
	@echo "  make run        - Run with Docker Compose (default)"
	@echo "  make run-dev    - Run in development mode with hot reload"
	@echo "  make run-prod   - Run in production mode"
	@echo "  make run-frontend - Run only frontend container"
	@echo "  make stop       - Stop all containers"
	@echo "  make clean      - Stop containers and remove volumes"
	@echo "  make logs       - View container logs"
	@echo ""
	@echo "🛠️  Development Commands:"
	@echo "  make install    - Install backend and frontend dependencies"
	@echo "  make test       - Run all tests"
	@echo "  make health     - Check API health"
	@echo ""
	@echo "🌐 Available URLs (development):"
	@echo "  Frontend:  http://localhost:3000"
	@echo "  Backend:   http://localhost:5170"
	@echo "  Swagger:   http://localhost:5170/swagger"
	@echo "  Database:  localhost:1433"

install:
	@echo "📦 Installing backend dependencies..."
	dotnet restore
	@echo "📦 Installing frontend dependencies..."
	cd frontend && npm install
	@echo "✅ Dependencies installed successfully!"

build:
	@echo "🔨 Building Docker images..."
	docker build -t car-reservation-api:latest .
	docker build -t car-reservation-frontend:latest ./frontend
	@echo "✅ Images built successfully!"

run:
	@echo "🚀 Starting Car Reservation System..."
	docker-compose up --build -d
	@echo "✅ System started! Check: http://localhost:3000"

run-dev:
	@echo "🔧 Starting development environment..."
	@echo "Frontend: http://localhost:3000"
	@echo "Backend:  http://localhost:5170"
	@echo "Swagger:  http://localhost:5170/swagger"
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build

run-prod:
	@echo "🚀 Starting production environment..."
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --build -d
	@echo "✅ Production started! Frontend: http://localhost"

run-frontend:
	@echo "🎨 Starting frontend only..."
	@echo "Make sure backend is running at http://localhost:5170"
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build frontend

stop:
	@echo "⏹️  Stopping all containers..."
	docker-compose down
	@echo "✅ All containers stopped!"

clean:
	@echo "🧹 Cleaning up containers and volumes..."
	docker-compose down -v
	docker system prune -f
	@echo "✅ Cleanup completed!"

logs:
	@echo "📋 Showing container logs..."
	docker-compose logs -f

test:
	@echo "🧪 Running backend tests..."
	dotnet test
	@echo "✅ Tests completed!"

health:
	@echo "🔍 Checking API health..."
	@curl -f http://localhost:5170/health || echo "❌ API not responding"
	@echo "✅ Health check completed!"

# Quick development shortcuts
dev: run-dev
prod: run-prod
up: run
down: stop