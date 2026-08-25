# GameLibrary API

A RESTful API for managing a video game library with JWT authentication, built with .NET 10 and Entity Framework Core. Containerized with Docker for easy deployment.

# 🚀 Features
 * CRUD Operations - Create, Read, Update, Delete games
 * Search - Search games by name (case-insensitive)
 * Authentication - JWT token-based authentication
 * Authorization - Protected endpoints with role-based access
 * Persistence - SQLite database with Entity Framework Core
 * Testing - 12+ unit tests with xUnit
 * Containerization - Docker support (Linux containers)
 * API Documentation - Swagger/OpenAPI
 * Multiple Repositories - JSON, InMemory, and SQL implementations

# 🛠️ Tech Stack
 - Technology	Purpose
 - .NET 10	Runtime & Framework
 - ASP.NET Core Web API	REST API
 - Entity Framework Core	ORM for database access
 - SQLite	Database
 - JWT	Authentication
 - xUnit	Unit testing
 - Docker	Containerization
 - Swagger/Scalar	API documentation

# 📋 Prerequisites
.NET 10 SDK

Docker Desktop (for containerized execution)

Git (optional)

🚀 Running the Application
Option 1: Run with Docker (Recommended)
bash
# Build the Docker image
docker build -t gamelibrary-api .

# Run the container with Swagger enabled
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development --name gamelibrary-api gamelibrary-api

# Check if the container is running
docker ps

# View logs
docker logs gamelibrary-api

# Stop the container
docker stop gamelibrary-api

# Remove the container
docker rm gamelibrary-api

# Restart the container
docker start gamelibrary-api
Access the API:

Swagger UI: http://localhost:5059/swagger

Scalar UI: http://localhost:5059/scalar/v1

Base URL: http://localhost:5059/api

Option 2: Run with .NET SDK (Without Docker)
bash
# Navigate to the API project
cd GameLibrary.API

# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Or run with Development environment
dotnet run --environment=Development
Access:

Swagger UI: https://localhost:5001/swagger

Base URL: https://localhost:5001/api

Option 3: Run the Console Application
bash
# Navigate to the console project
cd GameLibrary.ConsoleApp

# Run the console app
dotnet run
🔑 Authentication
The API uses JWT (JSON Web Tokens) for authentication.

Default Credentials
json
{
  "username": "admin",
  "password": "admin123"
}
Getting a Token
bash
# Using curl (PowerShell)
curl.exe -X POST http://localhost:5059/api/auth/login -H "Content-Type: application/json" -d "{\"username\":\"admin\",\"password\":\"admin123\"}"

# Using PowerShell Invoke-WebRequest
$body = @{ username = "admin"; password = "admin123" } | ConvertTo-Json
Invoke-WebRequest -Uri http://localhost:5059/api/auth/login -Method POST -Body $body -ContentType "application/json"
Response:

json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
Using the Token
Include the token in the Authorization header for protected endpoints:

bash
# Add a game (requires authentication)
curl.exe -X POST http://localhost:5059/api/games -H "Authorization: Bearer YOUR_TOKEN" -H "Content-Type: application/json" -d "{\"name\":\"Hollow Knight\",\"genre\":7,\"releaseYear\":2017,\"isCompleted\":false}"
📡 API Endpoints
Method	Endpoint	Description	Auth Required
POST	/api/auth/login	Get JWT token	❌
GET	/api/games	Get all games	❌
GET	/api/games/search?name={keyword}	Search games by name	❌
POST	/api/games	Add a new game	✅
PUT	/api/games/{name}	Update a game	✅
DELETE	/api/games/{name}	Delete a game	✅
Game Model
json
{
  "id": 1,
  "name": "Hollow Knight",
  "genre": 7,
  "releaseYear": 2017,
  "isCompleted": false
}
Genres (Enum Values)
text
0: Action
1: Adventure
2: RPG
3: Strategy
4: Simulation
5: Sports
6: Puzzle
7: MetroidVania
8: Other
🧪 Running Tests
bash
# Run all tests
dotnet test

# Run tests with output
dotnet test --verbosity normal

# Run specific test
dotnet test --filter "FullyQualifiedName~SearchGames"
Expected Output:

text
Total tests: 12
Passed: 12
Failed: 0
Skipped: 0
🐳 Docker Commands Reference
bash
# Build the image
docker build -t gamelibrary-api .

# Run the container (Development mode - Swagger enabled)
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development --name gamelibrary-api gamelibrary-api

# Run in Production mode (Swagger disabled)
docker run -d -p 5059:8080 --name gamelibrary-api gamelibrary-api

# View running containers
docker ps

# View all containers (including stopped)
docker ps -a

# View logs
docker logs gamelibrary-api

# Stop the container
docker stop gamelibrary-api

# Remove the container
docker rm gamelibrary-api

# Remove the image
docker rmi gamelibrary-api

# Clean up unused resources
docker system prune -a -f

# View images
docker images

# Run with custom environment variable
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development -e Jwt__Key="your-secret-key" --name gamelibrary-api gamelibrary-api
🔧 Configuration
JWT Configuration
The JWT settings are in GameLibrary.API/appsettings.json:

json
{
  "Jwt": {
    "Key": "esta-es-una-clave-secreta-de-al-menos-32-caracteres-para-jwt",
    "Issuer": "GameLibraryAPI",
    "Audience": "GameLibraryClient"
  }
}
For production, always use environment variables:

bash
# Docker environment variable
docker run -e Jwt__Key="your-production-secret-key" ...

# Or set in appsettings.Production.json
🚀 Deployment
Deploy to Azure App Service
bash
# Build and publish
dotnet publish GameLibrary.API -c Release -o ./publish

# Deploy to Azure (using Azure CLI)
az webapp deploy --resource-group YourResourceGroup --name YourAppName --src-path ./publish
Deploy to Azure Container Instances (ACI)
bash
# Build and push image to Azure Container Registry (ACR)
docker tag gamelibrary-api:latest youracregistry.azurecr.io/gamelibrary-api:latest
docker push youracregistry.azurecr.io/gamelibrary-api:latest

# Deploy to ACI
az container create --resource-group YourResourceGroup --name gamelibrary-api --image youracregistry.azurecr.io/gamelibrary-api:latest --dns-name-label gamelibrary-api --ports 8080
🧰 Useful Commands
bash
# Clean solution
dotnet clean

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run API locally (without Docker)
cd GameLibrary.API && dotnet run

# Run console app
cd GameLibrary.ConsoleApp && dotnet run

# Run all tests
dotnet test

# Remove all bin/obj folders (PowerShell)
Get-ChildItem -Path . -Recurse -Directory -Include bin, obj | Remove-Item -Recurse -Force

# Check API health
curl.exe http://localhost:5059/api/games

# Login and get token
curl.exe -X POST http://localhost:5059/api/auth/login -H "Content-Type: application/json" -d "{\"username\":\"admin\",\"password\":\"admin123\"}"
🐛 Troubleshooting
Docker Error: "no matching manifest for windows/amd64"
Solution: Switch Docker to Linux containers.

bash
# Via GUI (recommended)
# Right-click Docker icon → "Switch to Linux containers..."

# Via CLI
& "C:\Program Files\Docker\Docker\DockerCli.exe" -SwitchLinuxEngine
Docker Error: "read-only file system"
Solution: Restart Docker and WSL.

bash
# Restart Docker
wsl --shutdown
docker system prune -a -f
# Rebuild the image
docker build -t gamelibrary-api .
Swagger Not Loading
Solution: Run with Development environment.

bash
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development --name gamelibrary-api gamelibrary-api
Port Already in Use
bash
# Find process using the port
netstat -ano | findstr :5059

# Kill the process (replace PID)
taskkill /PID 1234 /F

# Or use a different port
docker run -d -p 5060:8080 --name gamelibrary-api gamelibrary-api
📄 License
This project is for educational purposes.

👨‍💻 Author
Developed as part of a .NET learning journey.
