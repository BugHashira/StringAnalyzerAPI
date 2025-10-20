using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace StringAnalyzerAPI.Models;

public class AnalyzedString
{
    [Key]
    public string Id { get; set; }
    public string Value { get; set; }
    public int Length { get; set; }
    public bool IsPalindrome { get; set; }
    public int UniqueCharacters { get; set; }
    public int WordCount { get; set; }
    public string Sha256Hash { get; set; }

    // Store the dictionary as JSON
    public string CharacterFrequencyMapJson
    {
        get => JsonSerializer.Serialize(CharacterFrequencyMap);
        set => CharacterFrequencyMap = string.IsNullOrEmpty(value)
            ? new Dictionary<string, int>()
            : JsonSerializer.Deserialize<Dictionary<string, int>>(value);
    }

    [NotMapped]
    public Dictionary<string, int> CharacterFrequencyMap { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
