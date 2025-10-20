using StringAnalyzerAPI.Models;

namespace StringAnalyzerAPI.Services;

public interface IStringRepository
{
    void Add(AnalyzedString analyzed);
    AnalyzedString? GetByValue(string value);
    IEnumerable<AnalyzedString> GetAll();
    void Delete(string value);
    bool Exists(string hash);
}
