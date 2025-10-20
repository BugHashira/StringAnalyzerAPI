using StringAnalyzerAPI.Models;
using System.Collections.Concurrent;

namespace StringAnalyzerAPI.Services;

public class InMemoryStringRepository : IStringRepository
{
    private readonly ConcurrentDictionary<string, AnalyzedString> _store = new();

    public void Add(AnalyzedString analyzed)
    {
        _store[analyzed.Id] = analyzed;
    }

    public AnalyzedString? GetByValue(string value)
    {
        return _store.Values.FirstOrDefault(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<AnalyzedString> GetAll() => _store.Values;

    public void Delete(string value)
    {
        var item = GetByValue(value);
        if (item != null)
            _store.TryRemove(item.Id, out _);
    }

    public bool Exists(string hash) => _store.ContainsKey(hash);
}
