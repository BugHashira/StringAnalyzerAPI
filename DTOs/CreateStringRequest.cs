using System.ComponentModel.DataAnnotations;

namespace StringAnalyzerAPI.DTOs;

public class CreateStringRequest
{
    [Required]
    public string? Value { get; set; }
}
