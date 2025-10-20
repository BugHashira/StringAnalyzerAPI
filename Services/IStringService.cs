using StringAnalyzerAPI.Models;

namespace StringAnalyzerAPI.Services;

public interface IStringService
{
    AnalyzedString CreateAnalyzedString(string value);
    AnalyzedString? GetByValue(string value);
    IEnumerable<AnalyzedString> GetAll(FilterParams filters);
    IEnumerable<AnalyzedString> FilterByNaturalLanguage(string query);
    void Delete(string value);
}
