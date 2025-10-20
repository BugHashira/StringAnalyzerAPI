using Microsoft.EntityFrameworkCore;
using StringAnalyzerAPI.Context;
using StringAnalyzerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core with Npgsql using the connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// Register your custom services
builder.Services.AddScoped<IStringService, StringService>();
builder.Services.AddScoped<IStringRepository, StringRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "String Analyzer API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
