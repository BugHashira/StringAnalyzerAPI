# StringAnalyzerAPI

Small ASP.NET Core Web API that analyzes and stores strings.  
Recent changes: persistence moved from in-memory to PostgreSQL via Entity Framework Core; EF migrations are included. The app now builds the DB connection from environment variables and exposes Swagger at /swagger.

Summary
- Analyze strings (length, palindrome, unique characters, word count, character frequency, SHA-256)
- Persist, retrieve, list (with filters), and delete analyzed strings in PostgreSQL via EF Core
- Simple natural-language-like filter parsing
- Connection built from environment variables: DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD
- Swagger UI available at /swagger (enabled for all environments)

Requirements
- .NET 9 SDK
- PostgreSQL (local, remote, or via Docker)
- Visual Studio 2022 (recommended) or VS Code / CLI
- (Optional) Docker

Important files
- Program.cs — app startup, EF registration, builds connection string from env vars
- Context/AppDbContext.cs — EF Core DbContext
- Controllers/StringsController.cs — API endpoints
- Services/, DTOs/, Models/ — core logic and DTOs
- Migrations/ — EF migrations included

Environment variables (required at runtime and for migrations)
- DB_HOST — PostgreSQL host (e.g. localhost)
- DB_PORT — PostgreSQL port (e.g. 5432)
- DB_NAME — database name (e.g. StringAnalyzerDB)
- DB_USER — database username (e.g. postgres)
- DB_PASSWORD — database password

Optional/useful
- ASPNETCORE_ENVIRONMENT — Development/Production
- ASPNETCORE_URLS — override listening URLs (e.g. "http://localhost:5000;https://localhost:5001")

Note: Program.cs currently constructs the Npgsql connection string directly from DB_* env vars and does not use ConnectionStrings in appsettings.json. Set DB_* variables before running migrations or the app.

Dependencies
- Included in project file:
  - Microsoft.AspNetCore.OpenApi
  - Swashbuckle.AspNetCore
  - Microsoft.VisualStudio.Azure.Containers.Tools.Targets (dev)
- EF / provider you should have or add:
  - Npgsql.EntityFrameworkCore.PostgreSQL
  - Microsoft.EntityFrameworkCore.Design
- EF CLI tool (for migrations):
  - dotnet tool install --global dotnet-ef

Install / restore
- dotnet restore
- If missing EF provider or design packages:
  - dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
  - dotnet add package Microsoft.EntityFrameworkCore.Design
- Install EF CLI (if needed):
  - dotnet tool install --global dotnet-ef

Database setup

Local Postgres
- Create DB and user matching DB_* env vars.

Docker quickstart (Postgres)
- docker run --name stringanalyzer-postgres -e POSTGRES_PASSWORD=yourpassword -e POSTGRES_DB=StringAnalyzerDB -p 5432:5432 -d postgres:15
- Then set DB_HOST (localhost or host.docker.internal), DB_PORT=5432, DB_NAME=StringAnalyzerDB, DB_USER=postgres, DB_PASSWORD=yourpassword.

Railway / cloud
- Set DB_* env vars per the platform's provided values.

Apply EF migrations
Set DB_* env vars in your shell (or in Visual Studio debug settings) and run:
- dotnet ef database update --project StringAnalyzerAPI.csproj

Examples (Bash)
- export DB_HOST=localhost
- export DB_PORT=5432
- export DB_NAME=StringAnalyzerDB
- export DB_USER=postgres
- export DB_PASSWORD=yourpassword
- dotnet ef database update --project StringAnalyzerAPI.csproj

Run locally (CLI)
1. Set DB_* env vars (see above).
2. dotnet restore
3. dotnet build
4. dotnet ef database update --project StringAnalyzerAPI.csproj
5. dotnet run --project StringAnalyzerAPI.csproj

To run on a specific URL:
- ASPNETCORE_URLS="http://localhost:5000" dotnet run --project StringAnalyzerAPI.csproj

Run in Visual Studio 2022
1. Open the folder/solution.
2. In __Solution Explorer__ right-click the project and select __Set as Startup Project__.
3. Open project __Properties__ > __Debug__ and add DB_* environment variables (DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD).
4. Press __F5__ to run with debugger or __Ctrl+F5__ to run without debugging.
5. Visit /swagger to explore the API.

Run with Docker (app container)
- Build:
  - docker build -t stringanalyzerapi .
- Run (example passing DB_* env vars):
  - docker run -p 5000:80 \
    -e DB_HOST=host.docker.internal \
    -e DB_PORT=5432 \
    -e DB_NAME=StringAnalyzerDB \
    -e DB_USER=postgres \
    -e DB_PASSWORD=yourpassword \
    -e ASPNETCORE_ENVIRONMENT=Production \
    stringanalyzerapi

(Consider docker-compose for app + Postgres — I can add a sample compose file.)

API examples (assume base URL https://localhost:5001)
- Create (POST /strings)
  - curl -X POST "https://localhost:5001/strings" -H "Content-Type: application/json" -d '{"value":"racecar"}'
- Get (GET /strings/{string_value})
  - curl "https://localhost:5001/strings/racecar"
- List with filters (GET /strings?is_palindrome=true&min_length=3)
  - curl "https://localhost:5001/strings?is_palindrome=true&min_length=3"
- Natural language filter (GET /strings/filter-by-natural-language?query=...)
- Delete (DELETE /strings/{string_value})
  - curl -X DELETE "https://localhost:5001/strings/racecar"

Notes
- contains_character query param must be a single character.
- API returns JSON and uses standard HTTP status codes (400/404/409).
- Swagger is served at /swagger and enabled in Program.cs for all environments.

Troubleshooting
- dotnet-ef missing: install with dotnet tool install --global dotnet-ef.
- EF provider missing: add Npgsql.EntityFrameworkCore.PostgreSQL and Microsoft.EntityFrameworkCore.Design.
- Migration/runtime DB cannot connect: verify DB_* env vars and that Postgres is reachable.
- If you previously relied on ConnectionStrings in appsettings.json: Program.cs now reads DB_* env vars. Either set those env vars or change Program.cs to read configuration keys.

Further help
- I can add docker-compose.yml for both API and Postgres.
- I can add a simple data seeder or a Postman collection.
