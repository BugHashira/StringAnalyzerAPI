using StringAnalyzerAPI.Context;
using StringAnalyzerAPI.Models;

namespace StringAnalyzerAPI.Services;

public class StringRepository(AppDbContext context) : IStringRepository
{
    public bool Exists(string hash) => context.AnalyzedStrings.Any(x => x.Id == hash);
    public void Add(AnalyzedString str)
    {
        context.AnalyzedStrings.Add(str);
        context.SaveChanges();
    }
    public AnalyzedString? GetByValue(string value) => context.AnalyzedStrings.FirstOrDefault(x => x.Value == value);
    public IEnumerable<AnalyzedString> GetAll() => context.AnalyzedStrings.ToList();
    public void Delete(string value)
    {
        var entity = context.AnalyzedStrings.FirstOrDefault(x => x.Value == value);
        if (entity != null)
        {
            context.AnalyzedStrings.Remove(entity);
            context.SaveChanges();
        }
    }
}
