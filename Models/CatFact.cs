using System.Text.Json.Serialization;

namespace CatFacts.Models;

/// <summary>
/// Represents a sentence containing a cat fact.
/// </summary>
/// <param name="fact">Cat fact sentence.</param>
/// <param name="length">Length of the sentence.</param>
public class CatFact(string fact, int length)
{
    [JsonPropertyName("fact")]
    public string Fact { get; init; } = fact;
    [JsonPropertyName("length")]
    public int Length { get; init; } = length;
}