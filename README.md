# StringAnalyzerAPI

Small ASP.NET Core Web API that analyzes and stores strings.  
Originally stored data in-memory; recent changes add a persistent PostgreSQL-backed store via Entity Framework Core and include EF migrations.

Highlights
- Analyze strings (length, palindrome, unique characters, word count, character frequency, sha256)
- Store/retrieve/delete analyzed strings in PostgreSQL
- List with filters and simple natural-language-like filter parsing
- Swagger UI for interactive exploration

What changed (quick)
- Persistence switched from in-memory to EF Core with a PostgreSQL database (see Context/AppDbContext.cs)
- appsettings.json now contains a DefaultConnection connection string and EF Core migrations are included (Migrations/)
- Add instructions below to install EF tools and run migrations locally

Requirements
- .NET 9 SDK (https://dotnet.microsoft.com)
- PostgreSQL (local, remote, or via Docker)
- Visual Studio 2022 (recommended) or VS Code / CLI
- (Optional) Docker if you want to run in a container

## Project structure (important files)
- Program.cs — app startup / DI
- Controllers/StringsController.cs — API endpoints
- Services/* — repository and service implementations (in-memory)
- Models/*, DTOs/* — domain models and request/response DTOs
- StringAnalyzerAPI.csproj — project file and NuGet references

## Dependencies and how to install them
The project targets .NET 9 and references these NuGet packages (already included in the project file):
- Microsoft.AspNetCore.OpenApi (v9.0.9)
- Swashbuckle.AspNetCore (v9.0.6)
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets (dev / docker support)

To add or restore packages via CLI:
1. Restore packages:
   - dotnet restore
2. (If you need to add a package manually)
   - dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.9
   - dotnet add package Swashbuckle.AspNetCore --version 9.0.6

## Environment variables
This project does not require any application-specific environment variables to run. You can optionally set:
- ASPNETCORE_ENVIRONMENT — e.g. Development, Production (controls whether Swagger is enabled)
- ASPNETCORE_URLS — override listening URLs, e.g. `http://localhost:5000;https://localhost:5001`
- PORT — commonly used by hosting setups (Kestrel respects ASPNETCORE_URLS more directly)

Note: The csproj contains a __UserSecretsId__ but there are no configured user secrets or configuration values required by the code.

## Run locally — CLI
1. Clone the repo
2. From the repository root:
   - dotnet restore
   - dotnet build
   - dotnet run --project StringAnalyzerAPI.csproj

If you want a specific listening URL (example: HTTP 5000), you can run:
- Windows / macOS / Linux (bash):
  - ASPNETCORE_URLS="http://localhost:5000" dotnet run --project StringAnalyzerAPI.csproj

When running in __Development__ environment, Swagger UI is enabled. Open the URL shown in the console and visit `/swagger` to explore the API.

## Run in Visual Studio 2022
1. Open the solution or folder.
2. In __Solution Explorer__ right-click the project and select __Set as Startup Project__.
3. Press __F5__ to run with the debugger or __Ctrl+F5__ to run without debugging.
4. Use the browser that opens or navigate to `/swagger` to interact with the API.

## Run with Docker (optional)
1. Build the Docker image:
   - docker build -t stringanalyzerapi .
2. Run the container (map ports and optionally set environment):
   - docker run -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Production stringanalyzerapi
3. Access API at http://localhost:5000 (or the mapped ports you chose)

## API Usage Examples
Assume base URL is https://localhost:5001 or the URL shown in your console.

- Create an analyzed string (POST /strings)
  curl -X POST "https://localhost:5001/strings" -H "Content-Type: application/json" -d '{"value":"racecar"}'

- Get by value (GET /strings/{string_value})
  curl "https://localhost:5001/strings/racecar"

- List with query filters (GET /strings)
  curl "https://localhost:5001/strings?is_palindrome=true&min_length=3"

- Filter by natural language (GET /strings/filter-by-natural-language)
  curl "https://localhost:5001/strings/filter-by-natural-language?query=palindromes%20longer%20than%205%20characters"

- Delete (DELETE /strings/{string_value})
  curl -X DELETE "https://localhost:5001/strings/racecar"

Notes:
- contains_character query param must be a single character when used.
- The API returns JSON responses; error cases return appropriate status codes (400/404/409).

## Tests
There are no unit tests in this repository by default. Add a test project and use `dotnet test` if you add tests.

## Troubleshooting
- If Swagger doesn't appear in Production, set __ASPNETCORE_ENVIRONMENT__ to Development or launch with debugger.
- If ports conflict, set __ASPNETCORE_URLS__ or use a different port mapping in Docker.
- Add a small Postman collection / HTTPie examples
- Add a Dockerfile or tweak launch settings
- Create a small unit test project scaffold
