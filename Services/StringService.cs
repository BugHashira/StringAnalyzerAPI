using StringAnalyzerAPI.DTOs;
using StringAnalyzerAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace StringAnalyzerAPI.Services;

public class StringService : IStringService
{
    private readonly IStringRepository _repo;

    public StringService(IStringRepository repo)
    {
        _repo = repo;
    }

    public AnalyzedString CreateAnalyzedString(string value)
    {
        var hash = ComputeSha256(value);
        if (_repo.Exists(hash))
            throw new ArgumentException("exists");

        var analyzed = new AnalyzedString
        {
            Id = hash,
            Value = value,
            Length = value.Length,
            IsPalindrome = CheckPalindrome(value),
            UniqueCharacters = value.ToLower().ToHashSet().Count,
            WordCount = value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
            Sha256Hash = hash,
            CharacterFrequencyMap = value.GroupBy(c => c.ToString())
                                         .ToDictionary(g => g.Key, g => g.Count()),
            CreatedAt = DateTime.UtcNow
        };

        _repo.Add(analyzed);
        return analyzed;
    }

    public AnalyzedString? GetByValue(string value) => _repo.GetByValue(value);

    public IEnumerable<AnalyzedString> GetAll(FilterParams filters)
    {
        var all = _repo.GetAll();

        if (filters.IsPalindrome.HasValue)
            all = all.Where(x => x.IsPalindrome == filters.IsPalindrome.Value);

        if (filters.MinLength.HasValue)
            all = all.Where(x => x.Length >= filters.MinLength.Value);

        if (filters.MaxLength.HasValue)
            all = all.Where(x => x.Length <= filters.MaxLength.Value);

        if (filters.WordCount.HasValue)
            all = all.Where(x => x.WordCount == filters.WordCount.Value);

        if (!string.IsNullOrEmpty(filters.ContainsCharacter))
            all = all.Where(x => x.Value.Contains(filters.ContainsCharacter, StringComparison.OrdinalIgnoreCase));

        return all;
    }

    public IEnumerable<AnalyzedString> FilterByNaturalLanguage(string query)
    {
        var parsed = NaturalLanguageParser.Parse(query);
        if (parsed == null)
            throw new ArgumentException("Unable to parse query");

        return GetAll(parsed);
    }

    public void Delete(string value)
    {
        var existing = _repo.GetByValue(value);
        if (existing == null)
            throw new KeyNotFoundException("not found");

        _repo.Delete(value);
    }

    private static string ComputeSha256(string raw)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    private static bool CheckPalindrome(string value)
    {
        var cleaned = new string(value.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());
        return cleaned.SequenceEqual(cleaned.Reverse());
    }
}