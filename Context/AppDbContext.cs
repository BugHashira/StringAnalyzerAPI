using Microsoft.EntityFrameworkCore;
using StringAnalyzerAPI.Models;

namespace StringAnalyzerAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AnalyzedString> AnalyzedStrings { get; set; }
}
