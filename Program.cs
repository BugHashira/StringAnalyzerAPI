using Microsoft.EntityFrameworkCore;
using StringAnalyzerAPI.Context;

var builder = WebApplication.CreateBuilder(args);

// Read PostgreSQL connection info from environment variables (Railway)
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? builder.Configuration["ConnectionStrings:DefaultConnectionHost"]};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? builder.Configuration["ConnectionStrings:DefaultConnectionPort"]};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? builder.Configuration["ConnectionStrings:DefaultConnectionDatabase"]};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER") ?? builder.Configuration["ConnectionStrings:DefaultConnectionUser"]};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? builder.Configuration["ConnectionStrings:DefaultConnectionPassword"]};" +
                       $"Pooling=true;Trust Server Certificate=true;";

// Configure EF Core with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger for all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "String Analyzer API v1");
    c.RoutePrefix = "swagger"; // ensures it serves at /swagger
});

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
