# GameLibrary API

A RESTful API for managing a video game library with JWT authentication, built with .NET 10 and Entity Framework Core. Containerized with Docker for easy deployment.

## 🚀 Features
 * CRUD Operations - Create, Read, Update, Delete games
 * Search - Search games by name (case-insensitive)
 * Authentication - JWT token-based authentication
 * Authorization - Protected endpoints with role-based access
 * Persistence - SQLite database with Entity Framework Core
 * Testing - 12+ unit tests with xUnit
 * Containerization - Docker support (Linux containers)
 * API Documentation - Swagger/OpenAPI
 * Multiple Repositories - JSON, InMemory, and SQL implementations

## 🛠️ Tech Stack
 - Technology	Purpose
 - .NET 10	Runtime & Framework
 - ASP.NET Core Web API	REST API
 - Entity Framework Core	ORM for database access
 - SQLite	Database
 - JWT	Authentication
 - xUnit	Unit testing
 - Docker	Containerization
 - Swagger/Scalar	API documentation

## 📋 Prerequisites
.NET 10 SDK

Docker Desktop (for containerized execution)

Git (optional)

## 🚀 Running the Application
### Option 1: Run with Docker (Recommended)

1. **Build the Docker image**\
docker build -t gamelibrary-api .

2. **Run the container with Swagger enabled**\
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development --name gamelibrary-api gamelibrary-api

3. **Check if the container is running**\
docker ps

- Access the API:

Swagger UI: http://localhost:5059/swagger

Base URL: http://localhost:5059/api

### Option 2: Run with .NET SDK (Without Docker)

1. **Navigate to the API project**\
cd GameLibrary.API

2. **Restore dependencies**\
dotnet restore

3. **Run the application**\
dotnet run

- Access:

Swagger UI: https://localhost:5001/swagger

Base URL: https://localhost:5001/api

### Option 3: Run the Console Application

1. **Navigate to the console project**\
cd GameLibrary.ConsoleApp

2. **Run the console app**\
dotnet run

## 🔑 Authentication
The API uses JWT (JSON Web Tokens) for authentication.

Default Credentials\
json\
{\
  "username": "admin",\
  "password": "admin123"\
}

### Getting a Token

**Using curl (PowerShell)**\
curl.exe -X POST http://localhost:5059/api/auth/login -H "Content-Type: application/json" -d "{\"username\":\"admin\",\"password\":\"admin123\"}"

**Using PowerShell Invoke-WebRequest**
$body = @{ username = "admin"; password = "admin123" } | ConvertTo-Json
Invoke-WebRequest -Uri http://localhost:5059/api/auth/login -Method POST -Body $body -ContentType "application/json"

- *Response*: 

json\
{\
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
}

### Using the Token
Include the token in the Authorization header for protected endpoints: 

**Add a game (requires authentication)** \
curl.exe -X POST http://localhost:5059/api/games -H "Authorization: Bearer **YOUR_TOKEN**" -H "Content-Type: application/json" -d "{\"name\":\"Hollow Knight\",\"genre\":7,\"releaseYear\":2017,\"isCompleted\":false}"  

**📡 API Endpoints**
Method | Endpoint | Description | Auth Required \
POST | /api/auth/login | Get JWT token | ❌ \
GET | /api/games | Get all games | ❌\
GET | /api/games/search?name={keyword} | Search games by name | ❌ \
POST | /api/games | Add a new game | ✅ \
PUT | /api/games/{name} | Update a game | ✅ \
DELETE | /api/games/{name} | Delete a game | ✅ \
\**Game Model**\
json\
{\
  "id": 1, \
  "name": "Hollow Knight", \
  "genre": 7, \
  "releaseYear": 2017, \
  "isCompleted": false \
}

**Genres (Enum Values)**:\
0: Action \
1: Adventure \
2: RPG \
3: Strategy \
4: Simulation \
5: Sports \
6: Puzzle \
7: MetroidVania \
8: Other 

## 🧪 Running Tests

**Run all tests**
dotnet test

**Run specific test**
dotnet test --filter "FullyQualifiedName~SearchGames" 

**Expected Output:**

Total tests: 12 Passed: 12 Failed: 0 Skipped: 0 

## Build the image
docker build -t gamelibrary-api .

## Run the container (Development mode - Swagger enabled)
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development --name gamelibrary-api gamelibrary-api

## Run in Production mode (Swagger disabled)
docker run -d -p 5059:8080 --name gamelibrary-api gamelibrary-api

## View running containers
docker ps

## Run with custom environment variable
docker run -d -p 5059:8080 -e ASPNETCORE_ENVIRONMENT=Development -e Jwt__Key="your-secret-key" --name gamelibrary-api gamelibrary-api

## Clean solution
dotnet clean

## Restore dependencies
dotnet restore

## Build solution
dotnet build

## Run API locally (without Docker)
1. cd GameLibrary.API
2. dotnet run

## Run console app
1. cd GameLibrary.ConsoleApp 
2. dotnet run

## 📄 License
This project is for educational purposes.

## 👨‍💻 Author
**Genaro Nicolás Cravero** 
Developed as part of a .NET learning journey.
