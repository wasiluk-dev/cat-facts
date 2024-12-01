using System.Text.Json.Serialization;

namespace CatFacts.Models;

public class CatFact(string fact, int length)
{
    [JsonPropertyName("fact")]
    public string Fact { get; init; } = fact;
    [JsonPropertyName("length")]
    public int Length { get; init; } = length;
}