using Microsoft.AspNetCore.Mvc;
using StringAnalyzerAPI.DTOs;
using StringAnalyzerAPI.Models;
using StringAnalyzerAPI.Services;

namespace StringAnalyzerAPI.Controllers;

[ApiController]
[Route("strings")]
public class StringsController(IStringService service) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateStringRequest req)
    {
        if (req == null || req.Value == null)
            return BadRequest(new { error = "Invalid request body or missing 'value' field" });

        try
        {
            var result = service.CreateAnalyzedString(req.Value);
            return CreatedAtAction(nameof(GetByValue), new { string_value = req.Value }, StringResponse.FromModel(result));
        }
        catch (ArgumentException ex) when (ex.Message == "exists")
        {
            return Conflict(new { error = "String already exists in the system" });
        }
    }

    [HttpGet("{string_value}")]
    public IActionResult GetByValue(string string_value)
    {
        var found = service.GetByValue(string_value);
        if (found == null)
            return NotFound(new { error = "String does not exist in the system" });

        return Ok(StringResponse.FromModel(found));
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] bool? is_palindrome, [FromQuery] int? min_length, [FromQuery] int? max_length, [FromQuery] int? word_count, [FromQuery] string? contains_character)
    {
        if (!string.IsNullOrEmpty(contains_character) && contains_character.Length != 1)
            return BadRequest(new { error = "contains_character must be a single character" });

        var filters = new FilterParams
        {
            IsPalindrome = is_palindrome,
            MinLength = min_length,
            MaxLength = max_length,
            WordCount = word_count,
            ContainsCharacter = contains_character
        };

        var data = service.GetAll(filters);
        return Ok(new
        {
            data = data.Select(StringResponse.FromModel),
            count = data.Count(),
            filters_applied = filters
        });
    }

    [HttpGet("filter-by-natural-language")]
    public IActionResult NaturalLanguage([FromQuery] string query)
    {
        try
        {
            var result = service.FilterByNaturalLanguage(query);
            return Ok(new
            {
                data = result.Select(StringResponse.FromModel),
                count = result.Count(),
                interpreted_query = new
                {
                    original = query,
                    parsed_filters = NaturalLanguageParser.Parse(query)
                }
            });
        }
        catch (ArgumentException)
        {
            return BadRequest(new { error = "Unable to parse natural language query" });
        }
    }

    [HttpDelete("{string_value}")]
    public IActionResult Delete(string string_value)
    {
        try
        {
            service.Delete(string_value);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = "String does not exist in the system" });
        }
    }
}
