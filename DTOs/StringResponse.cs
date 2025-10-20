using StringAnalyzerAPI.Models;

namespace StringAnalyzerAPI.DTOs;

public class StringResponse
{
    public string Id { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public object Properties { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public static StringResponse FromModel(AnalyzedString model)
    {
        return new StringResponse
        {
            Id = model.Id,
            Value = model.Value,
            CreatedAt = model.CreatedAt,
            Properties = new
            {
                model.Length,
                model.IsPalindrome,
                model.UniqueCharacters,
                model.WordCount,
                model.Sha256Hash,
                model.CharacterFrequencyMap
            }
        };
    }
}
