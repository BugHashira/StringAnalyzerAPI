using System.Text.RegularExpressions;

namespace StringAnalyzerAPI.Models;

public static class NaturalLanguageParser
{
    public static FilterParams? Parse(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return null;

        query = query.ToLowerInvariant();
        var filters = new FilterParams();

        if (query.Contains("palindromic"))
            filters.IsPalindrome = true;

        var wordCountMatch = Regex.Match(query, @"(single|one) word");
        if (wordCountMatch.Success)
            filters.WordCount = 1;

        var longerThan = Regex.Match(query, @"longer than (\\d+)");
        if (longerThan.Success)
            filters.MinLength = int.Parse(longerThan.Groups[1].Value) + 1;

        var containsLetter = Regex.Match(query, @"letter ([a-z])");
        if (containsLetter.Success)
            filters.ContainsCharacter = containsLetter.Groups[1].Value;

        if (filters.IsPalindrome == null && filters.MinLength == null && filters.WordCount == null && filters.ContainsCharacter == null)
            return null; // no recognizable intent

        return filters;
    }
}
